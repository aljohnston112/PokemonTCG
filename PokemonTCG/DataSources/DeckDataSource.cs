using PokemonTCG.Models;
using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace PokemonTCG.DataSources
{
    internal class DeckDataSource
    {
        // TODO File backups in case of failure.

        private static readonly Mutex FileMutex = new();
        private static readonly string DECK_FILE = "\\AppData\\decks.decks";
        private static readonly Dictionary<string, PokemonDeck> DeckNamesToDecks = new();

        internal static IImmutableDictionary<string, PokemonDeck> GetDecks()
        {
            return DeckNamesToDecks.ToImmutableDictionary();
        }

        internal static async Task SaveDeck(PokemonDeck deck, bool decksFileExists = true)
        {
            // Current decks must be loaded before writing a new file or they will get overitten.
            if (decksFileExists)
            {
                await LoadDecks();
            }

            FileMutex.WaitOne();
            DeckNamesToDecks[deck.Name] = deck;
            string deckFilePath = "\\AppData\\";
            Directory.CreateDirectory(deckFilePath);
            StorageFolder deckFolder = await StorageFolder.GetFolderFromPathAsync(FileUtil.GetFullPath(deckFilePath));
            StorageFile deckFile = await deckFolder.CreateFileAsync("decks.decks", CreationCollisionOption.ReplaceExisting);
            Stream fileStream = await deckFile.OpenStreamForWriteAsync();

            var options = new JsonWriterOptions
            {
                Indented = true
            };
            using var jsonWriter = new Utf8JsonWriter(fileStream, options);

            jsonWriter.WriteStartArray();
            foreach (KeyValuePair<string, PokemonDeck> deckEntry in DeckNamesToDecks)
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteString("name", deckEntry.Key);

                PokemonDeck pokemonDeck = deckEntry.Value;
                jsonWriter.WriteStartArray("ids");
                foreach (string cardId in pokemonDeck.CardIds)
                {
                    jsonWriter.WriteStringValue(cardId);
                }
                jsonWriter.WriteEndArray();
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
            jsonWriter.Flush();
            jsonWriter.Dispose();

            fileStream.Flush();
            fileStream.Dispose();

            FileMutex.ReleaseMutex();
        }

        internal static async Task LoadDecks()
        {
            FileMutex.WaitOne();
            StorageFile decksFile = await GetDecksFile();

            string jsonDecksText = await FileIO.ReadTextAsync(decksFile);
            JsonArray jObject = JsonArray.Parse(jsonDecksText);
            foreach (IJsonValue jsonDecksValue in jObject)
            {
                JsonObject jsonDecksObject = jsonDecksValue.GetObject();
                string name = jsonDecksObject.GetNamedString("name");
                List<string> cardIds = new();
                foreach (IJsonValue cardId in jsonDecksObject.GetNamedArray("ids"))
                {
                    cardIds.Add(cardId.GetString());
                }

                PokemonDeck deck = new(name, cardIds.ToImmutableArray());
                if (!DeckNamesToDecks.TryAdd(name, deck))
                {
                    DeckNamesToDecks.Remove(name);
                    DeckNamesToDecks.Add(name, deck);
                }
            }
        }

        private static async Task<StorageFile> GetDecksFile()
        {
            StorageFile decksFile;
            try
            {
                decksFile = await FileUtil.GetFile(DECK_FILE);
            }
            catch (FileNotFoundException)
            {
                FileMutex.ReleaseMutex();
                SavePreMadeDecks();
                FileMutex.WaitOne();
                decksFile = await FileUtil.GetFile(DECK_FILE);
            }
            return decksFile;
        }

        private static async void SavePreMadeDecks()
        {
            await SaveDeck(PokemonDeck.BLACKOUT_DECK, decksFileExists: false);
        }

    }

}