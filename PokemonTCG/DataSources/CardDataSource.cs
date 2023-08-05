using PokemonTCG.Models;
using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Search;
using Type = PokemonTCG.Models.Type;

namespace PokemonTCG.DataSources
{

    // TODO Caching individual set images to save memory
    // TODO Caching individual deck images to save memory
    // TODO Cache that depends on whether in the deck builder or game

    /// <summary>
    /// For getting <c>Card</c> instances.
    /// First the instances must be loaded from a given folder; <see cref="LoadSets"/>.
    /// Then instances are retrieved by the url of the corresponding png file with the card picture; <see cref="GetInstance(string)"/>.
    /// </summary>
    internal class CardDataSource
    {

        // Contains the card instances used for GamePage
        private static readonly Dictionary<string, Card> _instances = new();

        // The card instances will not be available till the count down reaches 0
        private static readonly CountdownEvent _countDownEventForCardsLoaded = new(1);

        /// <summary>
        /// Gets a <c>Card</c> instance based on the card's id.
        /// The <c>Card</c> class is used for the <c>GamePage</c>.
        /// This method will block until all Card instances are loaded from memory.
        /// You must call <see cref="LoadSets"/> to load the Card instances.
        /// </summary>
        /// <param name="id">The id of the card</param>
        /// <returns>The <c>Card</c> instance the corresponds to the id</returns>
        public static Card GetInstance(string id)
        {
            _countDownEventForCardsLoaded.Wait();

            return _instances[id];
        }

        /// <summary>
        /// Loads all <c>Card</c> instances from folder.
        /// </summary>
        /// <param name="folder">The folder that contains the json files with the <c>Card</c> data</param>
        public static async Task LoadSets(string folder)
        {
            // Get the json files from the folder
            StorageFolder storageFolder = await FileUtil.GetFolder(folder);
            List<string> fileTypeFilter = new() { ".json" };
            QueryOptions queryOptions = new(CommonFileQuery.OrderByName, fileTypeFilter);
            StorageFileQueryResult query = storageFolder.CreateFileQueryWithOptions(queryOptions);
            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            // Read the json files
            foreach (StorageFile file in fileList)
            {
                if (!file.Name.Contains("sets.json"))
                {
                    await LoadCards(file);
                }
            }


            // Cards have been loaded
            if (_countDownEventForCardsLoaded.CurrentCount != 0)
            {
                _countDownEventForCardsLoaded.Signal();
            }
        }

        /// <summary>
        /// Creates a <c>Card</c> intance from a json file. 
        /// </summary>
        /// <param name="file">The json file containing the card information</param>
        /// <returns>A Task that returns the Card when it is ready</returns>
        /// <exception cref="Exception">Throws an exception if the supertype of the card is not "Pok\u00e9mon", "Trainer", or "Energy"</exception>
        private async static Task LoadCards(StorageFile file)
        {
            // Read the file
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jObject = JsonObject.Parse(jsonText);
            JsonArray jArray = jObject.GetNamedArray("data");
            for (int i = 0; i < jArray.Count; i++)
            {
                // For whatever reason, a foreach loop did not work
                JsonObject jsonCardItem = JsonObject.Parse(jArray[i].Stringify());
                string path = file.Path[..(file.Path.LastIndexOf(".") + 1)] + "\\";
                string id = jsonCardItem.GetNamedString("id");
                string imageUrl = path + jsonCardItem.GetNamedString("id") + "-small.png";

                if (!_instances.ContainsKey(id))
                {
                    // Create the card instance
                    Card card;
                    string supertype = jsonCardItem.GetNamedString("supertype");
                    string name = jsonCardItem.GetNamedString("name");
                    if (supertype == "Pok\u00e9mon")
                    {
                        card = LoadPokemonCard(jsonCardItem, imageUrl, id, name);
                    }
                    else if (supertype == "Trainer")
                    {
                        card = new TrainerCard(imageUrl, id, name);
                    }
                    else if (supertype == "Energy")
                    {

                        string[] energyTypes = name.Split();
                        string energyType = energyTypes[^2];
                        PokemonType type = Type.GetType(energyType);
                        card = new EnergyCard(imageUrl, id, name, type);
                    }
                    else
                    {
                        throw new Exception();
                    }
                    _instances.Add(id, card);
                }
            }

        }

        /// <summary>
        /// Creates a <c>PokemonCard</c> given a <c>JsonObject</c> of the json file.
        /// </summary>
        /// <param name="jObject">The <c>JsonObject</c> of the json file containing the Pokemon card information</param>
        /// <param name="imageUrl">The url for the image of the Pokmeon card</param>
        /// <param name="id">The unique id of the card</param>
        /// <param name="name">The name of the card</param>
        /// <returns>A <c>PokemonCard</c> representing the data passed in</returns>
        private static PokemonCard LoadPokemonCard(
            JsonObject jObject,
            string imageUrl,
            string id,
            string name
            )
        {
            int hp = int.Parse(jObject.GetNamedString("hp"));

            // Get the types
            List<PokemonType> type = new();
            foreach (IJsonValue obj in jObject.GetNamedArray("types"))
            {
                type.Add(Type.GetType(obj.GetString()));
            }

            // Get the attacks
            List<Attack> attacks = new();
            JsonArray jsonAttackArray = jObject.GetNamedArray("attacks");
            for (int i = 0; i < jsonAttackArray.Count; i++)
            {
                // For whatever reason, a foreach loop did not work
                JsonObject jsonAttack = JsonObject.Parse(jsonAttackArray[i].Stringify());
                string attackName = jsonAttack.GetNamedString("name");

                // Get the energy cost of the attack
                Dictionary<PokemonType, int> attackCost = new();
                JsonArray jsonAttackCost = jsonAttack.GetNamedArray("cost");
                foreach (IJsonValue obj in jsonAttackCost)
                {
                    String typeString = obj.Stringify();
                    PokemonType t = Type.GetType(typeString[1..^1]);
                    if (!attackCost.ContainsKey(t))
                    {
                        attackCost[t] = 0;
                    }
                    attackCost[t] += 1;
                }
                attacks.Add(new Attack(attackName, attackCost));
            }

            // Get the abilities
            List<Ability> pokemonPowers = new();
            if (jObject.ContainsKey("abilities"))
            {
                JsonArray pokemonPowerArray = jObject.GetNamedArray("abilities");
                for (int i = 0; i < pokemonPowerArray.Count; i++)
                {
                    // Same as above
                    JsonObject jsonPokemonPower = JsonObject.Parse(pokemonPowerArray[i].Stringify());
                    string pokemonPowerName = jsonPokemonPower.GetNamedString("name");
                    string pokemonPowerText = jsonPokemonPower.GetNamedString("text");
                    string pokemonPowerType = jsonPokemonPower.GetNamedString("type");
                    pokemonPowers.Add(new Ability(pokemonPowerName, pokemonPowerText, pokemonPowerType));
                }
            }

            // Get the weaknesses
            Dictionary<PokemonType, string> weakness = new();
            if (jObject.ContainsKey("weaknesses"))
            {
                JsonArray weaknessArray = jObject.GetNamedArray("weaknesses");
                for (int i = 0; i < weaknessArray.Count; i++)
                {
                    // Same as above
                    var jsonWeaknesses = JsonObject.Parse(weaknessArray[i].Stringify());
                    string weaknessType = jsonWeaknesses.GetNamedString("type");
                    string weaknessValue = jsonWeaknesses.GetNamedString("value");
                    weakness[Type.GetType(weaknessType)] = weaknessValue;
                }
            }

            // Get the resisitances
            Dictionary<PokemonType, string> resistances = new();
            if (jObject.ContainsKey("resistances"))
            {
                JsonArray resistanceArray = jObject.GetNamedArray("resistances");
                for (int i = 0; i < resistanceArray.Count; i++)
                {
                    // Same as above
                    var jsonResistances = JsonObject.Parse(resistanceArray[i].Stringify());
                    string resistanceType = jsonResistances.GetNamedString("type");
                    string resistanceValue = jsonResistances.GetNamedString("value");
                    resistances[Type.GetType(resistanceType)] = resistanceValue;
                }
            }

            // Get the retreat cost
            Dictionary<PokemonType, int> retreatCost = new();
            if (jObject.ContainsKey("retreatCost"))
            {
                JsonArray jsonRetreatCost = jObject.GetNamedArray("retreatCost");
                foreach (IJsonValue obj in jsonRetreatCost)
                {
                    String retreatString = obj.Stringify();
                    PokemonType t = Type.GetType(retreatString[1..^1]);
                    if (!retreatCost.ContainsKey(t))
                    {
                        retreatCost[t] = 0;
                    }
                    retreatCost[t] += 1;

                }
            }

            string evolvesFrom = null;
            if (jObject.ContainsKey("evolvesFrom"))
            {
                evolvesFrom = jObject.GetNamedString("evolvesFrom");
            }

            return new PokemonCard(
                    imageUrl,
                    id,
                    name,
                    hp,
                    type,
                    attacks,
                    pokemonPowers,
                    weakness,
                    resistances,
                    retreatCost,
                    evolvesFrom
                 );
        }

    }

}
