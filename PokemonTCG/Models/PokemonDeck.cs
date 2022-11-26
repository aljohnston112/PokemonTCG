using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace PokemonTCG.Models
{
    public class PokemonDeck
    {

        public readonly ImmutableArray<string> Deck;
        public readonly string Name;

        /// <summary>
        /// Creates a Deck given the ids of Pokemon cards.
        /// </summary>
        /// <param name="ids">The ids of Pokemon cards</param>
        public PokemonDeck(string name, ICollection<string> ids)
        {
            Debug.Assert(ids.Count == 60);
            Deck = ids.ToImmutableArray();
            Name = name;
        }

        public static readonly PokemonDeck BLACKOUT_DECK = new(
            "Blackout",
            ImmutableList.Create(
                "base1-7", "base1-79", "base1-93", "base1-84", "base1-88", "base1-34",
                "base1-34", "base1-42", "base1-42", "base1-27", "base1-27", "base1-62",
                "base1-62", "base1-62", "base1-56", "base1-56", "base1-56", "base1-65",
                "base1-65", "base1-65", "base1-52", "base1-52", "base1-52", "base1-52",
                "base1-63", "base1-63", "base1-63", "base1-63", "base1-92", "base1-92",
                "base1-92", "base1-92", "base1-102", "base1-102", "base1-102", "base1-102",
                "base1-102", "base1-102", "base1-102", "base1-102", "base1-102", "base1-102",
                "base1-102", "base1-102", "base1-97", "base1-97", "base1-97", "base1-97",
                "base1-97", "base1-97", "base1-97", "base1-97", "base1-97", "base1-97",
                "base1-97", "base1-97", "base1-97", "base1-97", "base1-97", "base1-97"
                )
            );

        private static Dictionary<string, PokemonDeck> _decks = new();
        private static CountdownEvent countDownEvent = new(1);

        public static async Task<ImmutableDictionary<string, PokemonDeck>> GetDecks()
        {
            countDownEvent.Wait();
            return _decks.ToImmutableDictionary();
        }

        // TODO handle double loading
        public static async Task LoadDecks()
        {
            if (countDownEvent.CurrentCount != 0)
            {
                string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
                string deckFile = baseFolder + "AppData\\decks.decks";
                StorageFile decksFile = await FileUtil.getFile(deckFile);
                string json = await FileIO.ReadTextAsync(decksFile);
                var options = new JsonWriterOptions
                {
                    Indented = true
                };
                using JsonDocument document = JsonDocument.Parse(json);
                JsonElement root = document.RootElement;
                foreach (JsonElement deck in root.EnumerateArray())
                {
                    string name = deck.GetProperty("name").GetString();
                    List<string> ids = new();
                    foreach (JsonElement id in deck.GetProperty("ids").EnumerateArray())
                    {
                        ids.Add(id.GetString());
                    }
                    PokemonDeck d = new PokemonDeck(name, ids);
                    if (!_decks.TryAdd(name, d))
                    {
                        _decks.Remove(name);
                        _decks.Add(name, d);
                    }
                }
                countDownEvent.Signal();
            }
        }


        public static async Task SaveDeck(PokemonDeck deck)
        {
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string deckFile = baseFolder + "AppData\\";
            Directory.CreateDirectory(deckFile);
            StorageFolder deckFolder = await FileUtil.GetFolder(deckFile);
            StorageFile sampleFile = await deckFolder.CreateFileAsync("decks.decks", CreationCollisionOption.ReplaceExisting);
            Stream fileStream = await sampleFile.OpenStreamForWriteAsync();
            _decks[deck.Name] = deck;


            var options = new JsonWriterOptions
            {
                Indented = true
            };
            using var writer = new Utf8JsonWriter(fileStream, options);

            writer.WriteStartArray();
            foreach (KeyValuePair<string, PokemonDeck> d in _decks)
            {
                writer.WriteStartObject();
                writer.WriteString("name", d.Key);
                PokemonDeck pd = d.Value;
                writer.WriteStartArray("ids");
                foreach (string id in pd.Deck)
                {
                    writer.WriteStringValue(id);
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }

    }

}
