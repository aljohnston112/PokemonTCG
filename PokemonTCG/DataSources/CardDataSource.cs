using PokemonTCG.Models;
using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Search;
using Type = PokemonTCG.Models.Type;

namespace PokemonTCG.DataSources
{

    // TODO Create a Dictionary<string, CountdownEvent> so folder loading can be tracked

    /// <summary>
    /// For getting <c>Card</c> instances.0
    /// First the instances must be loaded from a given folder; <see cref="LoadCards"/>.
    /// Then instances are retrieved by the url of the corresponding png file with the card picture; <see cref="GetInstance(string)"/>.
    /// </summary>
    internal class CardDataSource
    {

        // Contains the card instances used for GamePage
        private static readonly Dictionary<string, Card> _instances = new();

        // The card instances will not be available till the count down reaches 0
        // TODO will become a Dictionary<string, CountdownEvent> to support mulitple folders
        private static readonly CountdownEvent _countDownEventForCardsLoaded = new CountdownEvent(1);

        /// <summary>
        /// Gets a <c>Card</c> instance based on the card's id.
        /// The <c>Card</c> class is used for the <c>GamePage</c>.
        /// This method will block until all Card instances are loaded from memory.
        /// You must call <see cref="LoadCards"/> to load the Card instances.
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
        public static async Task LoadCards(string folder)
        {
            // Get the json files from the folder
            StorageFolder storageFolder = await FileUtil.GetFolder(folder);
            List<string> fileTypeFilter = new() { ".json" };
            QueryOptions queryOptions = new(CommonFileQuery.OrderByName, fileTypeFilter);
            StorageFileQueryResult query = storageFolder.CreateFileQueryWithOptions(queryOptions);
            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            // Read the json files
            List<Card> pList = new();
            foreach (StorageFile file in fileList)
            {
                await LoadCard(file);
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
        private async static Task<Card> LoadCard(StorageFile file)
        {
            // Read the file
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jObject = JsonObject.Parse(jsonText);

            string path = file.Path.Substring(0, file.Path.LastIndexOf("\\") + 1);
            string imageUrl = path + jObject.GetNamedString("key");
            string id = jObject.GetNamedString("id");

            if (!_instances.ContainsKey(id))
            {
                // Create the card instance
                Card card;
                string supertype = jObject.GetNamedString("supertype");
                string name = jObject.GetNamedString("name");
                if (supertype == "Pok\u00e9mon")
                {
                    card = LoadPokemonCard(jObject, imageUrl, id, name);
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
            return _instances[id];

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
                JsonObject jsonAttackCost = jsonAttack.GetNamedObject("cost");
                foreach (KeyValuePair<string, IJsonValue> obj in jsonAttackCost)
                {
                    attackCost[Type.GetType(obj.Key)] = int.Parse(obj.Value.ToString());
                }
                attacks.Add(new Attack(attackName, attackCost));
            }

            // Get the abilities
            List<PokemonPower> pokemonPowers = new();
            JsonArray pokemonPowerArray = jObject.GetNamedArray("abilities");
            for (int i = 0; i < pokemonPowerArray.Count; i++)
            {
                // Same as above
                JsonObject jsonPokemonPower = JsonObject.Parse(pokemonPowerArray[i].Stringify());
                string pokemonPowerName = jsonPokemonPower.GetNamedString("name");
                string pokemonPowerType = jsonPokemonPower.GetNamedString("type");
                pokemonPowers.Add(new PokemonPower(pokemonPowerName, pokemonPowerType));
            }

            // Get the weaknesses
            Dictionary<PokemonType, string> weakness = new();
            JsonArray weaknessArray = jObject.GetNamedArray("weaknesses");
            for (int i = 0; i < weaknessArray.Count; i++)
            {
                // Same as above
                var jsonWeaknesses = JsonObject.Parse(weaknessArray[i].Stringify());
                string weaknessType = jsonWeaknesses.GetNamedString("type");
                string weaknessValue = jsonWeaknesses.GetNamedString("value");
                weakness[Type.GetType(weaknessType)] = weaknessValue;
            }

            // Get the resisitances
            Dictionary<PokemonType, string> resistances = new();
            JsonArray resistanceArray = jObject.GetNamedArray("resistances");
            for (int i = 0; i < resistanceArray.Count; i++)
            {
                // Same as above
                var jsonResistances = JsonObject.Parse(resistanceArray[i].Stringify());
                string resistanceType = jsonResistances.GetNamedString("type");
                string resistanceValue = jsonResistances.GetNamedString("value");
                weakness[Type.GetType(resistanceType)] = resistanceValue;
            }

            // Get the retreat cost
            Dictionary<PokemonType, int> retreatCost = new();
            JsonObject jsonRetreatCost = jObject.GetNamedObject("retreatcost");
            foreach (KeyValuePair<string, IJsonValue> obj in jsonRetreatCost)
            {
                retreatCost[Type.GetType(obj.Key)] = int.Parse(obj.Value.ToString());
            }

            string evolvesFrom = jObject.GetNamedString("evolvesfrom");

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
