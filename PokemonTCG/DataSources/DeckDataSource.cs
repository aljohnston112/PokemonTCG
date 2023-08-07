using PokemonTCG.Models;
using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace PokemonTCG.DataSources
{
    internal class DeckDataSource
    {

        private static readonly Mutex FileMutex = new();
        private static readonly Dictionary<string, PokemonDeck> Decks = new();

        public static ImmutableDictionary<string, PokemonDeck> GetDecks()
        {
            return Decks.ToImmutableDictionary();
        }

        public static async Task LoadDecks()
        {
            FileMutex.WaitOne();
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string deckFile = baseFolder + "AppData\\decks.decks";

            StorageFile decksFile = await FileUtil.GetFile(deckFile);
            if (decksFile == null)
            {
                FileMutex.ReleaseMutex();
                SavePreMadeDecks();
                FileMutex.WaitOne();
                decksFile = await FileUtil.GetFile(deckFile);
            }

            string jsonDecks = await FileIO.ReadTextAsync(decksFile);
            using JsonDocument jsonDecksDocument = JsonDocument.Parse(jsonDecks);
            JsonElement root = jsonDecksDocument.RootElement;
            foreach (JsonElement jsonDeck in root.EnumerateArray())
            {
                string name = jsonDeck.GetProperty("name").GetString();
                List<string> ids = new();
                foreach (JsonElement id in jsonDeck.GetProperty("ids").EnumerateArray())
                {
                    ids.Add(id.GetString());
                }
                PokemonDeck deck = new(name, ids);
                if (!Decks.TryAdd(name, deck))
                {
                    Decks.Remove(name);
                    Decks.Add(name, deck);
                }
            }
        }

        private static async void SavePreMadeDecks()
        {
            await SaveDeck(PokemonDeck.BLACKOUT_DECK);
        }

        public static async Task SaveDeck(PokemonDeck deck)
        {
            FileMutex.WaitOne();
            Decks[deck.Name] = deck;

            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string deckFilePath = baseFolder + "AppData\\";
            Directory.CreateDirectory(deckFilePath);
            StorageFolder deckFolder = await FileUtil.GetFolder(deckFilePath);
            StorageFile deckFile = await deckFolder.CreateFileAsync("decks.decks", CreationCollisionOption.ReplaceExisting);
            Stream fileStream = await deckFile.OpenStreamForWriteAsync();

            var options = new JsonWriterOptions
            {
                Indented = true
            };
            using var writer = new Utf8JsonWriter(fileStream, options);

            writer.WriteStartArray();
            foreach (KeyValuePair<string, PokemonDeck> deckEntry in Decks)
            {
                writer.WriteStartObject();
                writer.WriteString("name", deckEntry.Key);
                PokemonDeck pokemonDeck = deckEntry.Value;
                writer.WriteStartArray("ids");
                foreach (string id in pokemonDeck.CardIds)
                {
                    writer.WriteStringValue(id);
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.Flush();
            FileMutex.ReleaseMutex();
        }

    }

}
