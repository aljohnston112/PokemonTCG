using PokemonTCG.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace PokemonTCG.DataSources
{

    internal class CardDataSource
    {

        private static readonly IDictionary<string, PokemonCard> idsToCards = new Dictionary<string, PokemonCard>();

        /// <summary>
        /// Gets all cards associated with a set. 
        /// </summary>
        /// <param name="file">The json file containing the card information for the set.</param>
        /// <returns>A <c>Task<ICollection<Card>>/c> that returns the collection of cards in the set.</returns>
        /// <exception cref="Exception">Throws an exception if the supertype of the card is not "Pok\u00e9mon", "Trainer", or "Energy"</exception>
        internal async static Task<ICollection<PokemonCard>> LoadCardsFromSet(StorageFile file)
        {
            // Read the file
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jObject = JsonObject.Parse(jsonText);
            JsonArray jArray = jObject.GetNamedArray("data");

            string baseImagePath = file.Path[..(file.Path.LastIndexOf(".") + 1)] + "\\";
            foreach (IJsonValue jsonCardValue in jArray)
            {
                JsonObject jsonCard = jsonCardValue.GetObject();
                string id = jsonCard.GetNamedString("id");

                if (!idsToCards.ContainsKey(id))
                {
                    string name = jsonCard.GetNamedString("name");
                    CardSupertype supertype = PokemonCard.GetCardSuperType(jsonCard.GetNamedString("supertype"));

                    // Subtypes
                    List<CardSubtype> cardSubTypes = new();
                    if (jsonCard.ContainsKey("evolvesFrom"))
                    {
                        JsonArray jsonSubtypes = jsonCard.GetNamedArray("subtypes");
                        foreach (IJsonValue jsonSubtype in jsonSubtypes)
                        {
                            cardSubTypes.Add(PokemonCard.GetCardSubType(jsonSubtype.GetString()));
                        }
                    }

                    int level = 0;
                    if (jsonCard.ContainsKey("level"))
                    {
                        level = int.Parse(jsonCard.GetNamedString("level"));
                    }
                    int hp = 0;
                    if (jsonCard.ContainsKey("level"))
                    {
                        hp = int.Parse(jsonCard.GetNamedString("hp"));
                    }

                    // Pokemon types
                    List<Models.PokemonType> pokemonTypes = new();
                    if (jsonCard.ContainsKey("types"))
                    {
                        JsonArray jsonTypeList = jsonCard.GetNamedArray("types");
                        foreach (IJsonValue obj in jsonTypeList)
                        {
                            pokemonTypes.Add(PokemonCard.GetPokemonType(obj.GetString()));
                        }
                    }

                    // Evolves from
                    string evolvesFrom = null;
                    if (jsonCard.ContainsKey("evolvesFrom"))
                    {
                        evolvesFrom = jsonCard.GetNamedString("evolvesFrom");
                    }

                    // Abilities
                    List<Ability> abilities = new();
                    if (jsonCard.ContainsKey("abilities"))
                    {
                        JsonArray jsonAbilityList = jsonCard.GetNamedArray("abilities");
                        foreach (IJsonValue jsonAbilityValue in jsonAbilityList)
                        {
                            JsonObject jsonAbility = jsonAbilityValue.GetObject();
                            string pokemonPowerName = jsonAbility.GetNamedString("name");
                            string pokemonPowerText = jsonAbility.GetNamedString("text");
                            string pokemonPowerType = jsonAbility.GetNamedString("type");
                            abilities.Add(new Ability(pokemonPowerName, pokemonPowerText, pokemonPowerType));
                        }
                    }

                    // Attacks
                    List<Attack> attacks = new();
                    if (jsonCard.ContainsKey("attacks"))
                    {
                        JsonArray jsonAttackArray = jsonCard.GetNamedArray("attacks");
                        foreach (IJsonValue jsonAttackValue in jsonAttackArray)
                        {
                            JsonObject jsonAttack = jsonAttackValue.GetObject();
                            string attackName = jsonAttack.GetNamedString("name");

                            // Get the energy cost of the attack
                            Dictionary<PokemonType, int> attackCost = new();
                            JsonArray jsonAttackCost = jsonAttack.GetNamedArray("cost");
                            foreach (IJsonValue pokemonTypeValue in jsonAttackCost)
                            {
                                PokemonType pokemonType = PokemonCard.GetPokemonType(pokemonTypeValue.GetString());
                                if (!attackCost.ContainsKey(pokemonType))
                                {
                                    attackCost[pokemonType] = 0;
                                }
                                attackCost[pokemonType] += 1;
                            }

                            int convertedEnergyCost = (int)(jsonAttack.GetNamedNumber("convertedEnergyCost"));
                            string damageString = jsonAttack.GetNamedString("damage");
                            if (damageString.EndsWith("×") || damageString.EndsWith("+") || damageString.EndsWith("-"))
                            {
                                damageString = damageString[..^1];
                            }
                            else if (damageString == "")
                            {
                                damageString = "0";
                            }
                            int damage = int.Parse(damageString);
                            string text = jsonAttack.GetNamedString("text");
                            attacks.Add(new Attack(attackName, attackCost, convertedEnergyCost, damage, text));
                        }
                    }

                    // Weaknesses
                    Dictionary<PokemonType, string> weakness = new();
                    if (jsonCard.ContainsKey("weaknesses"))
                    {
                        JsonArray weaknessArray = jsonCard.GetNamedArray("weaknesses");
                        foreach (IJsonValue weaknessValue in weaknessArray)
                        {
                            JsonObject jsonWeakness = weaknessValue.GetObject();

                            string weaknessType = jsonWeakness.GetNamedString("type");
                            string weaknessModifier = jsonWeakness.GetNamedString("value");
                            weakness[PokemonCard.GetPokemonType(weaknessType)] = weaknessModifier;
                        }
                    }

                    // Resistances
                    Dictionary<PokemonType, string> resistances = new();
                    if (jsonCard.ContainsKey("resistances"))
                    {
                        JsonArray resistanceArray = jsonCard.GetNamedArray("resistances");
                        foreach (IJsonValue resistanceValue in resistanceArray)
                        {
                            JsonObject jsonresistance = resistanceValue.GetObject();
                            string resistanceType = jsonresistance.GetNamedString("type");
                            string resistanceModifier = jsonresistance.GetNamedString("value");
                            resistances[PokemonCard.GetPokemonType(resistanceType)] = resistanceModifier;
                        }
                    }

                    // Retreat cost
                    Dictionary<PokemonType, int> retreatCost = new();
                    if (jsonCard.ContainsKey("retreatCost"))
                    {
                        JsonArray jsonRetreatCost = jsonCard.GetNamedArray("retreatCost");
                        foreach (IJsonValue retreatCostValue in jsonRetreatCost)
                        {
                            String retreatString = retreatCostValue.GetString();
                            PokemonType pokemontype = PokemonCard.GetPokemonType(retreatString);
                            if (!retreatCost.ContainsKey(pokemontype))
                            {
                                retreatCost[pokemontype] = 0;
                            }
                            retreatCost[pokemontype] += 1;

                        }
                    }

                    int convertedRetreatCost = 0;
                    if (jsonCard.ContainsKey("retreatCost"))
                    {
                        convertedRetreatCost = (int)(jsonCard.GetNamedNumber("convertedRetreatCost"));
                    }

                    // Image paths
                    Dictionary<ImageSize, String> imagePaths = new() {
                        { ImageSize.SMALL,  baseImagePath + id + "-small.png" },
                        { ImageSize.LARGE,  baseImagePath + id + "-large.png" }
                    };

                    PokemonCard card = new(
                        id,
                        name,
                        supertype,
                        level,
                        hp,
                        pokemonTypes,
                        evolvesFrom,
                        abilities,
                        attacks,
                        weakness,
                        resistances,
                        retreatCost,
                        convertedRetreatCost,
                        imagePaths
                        );

                    idsToCards.Add(id, card);
                }
            }
            return idsToCards.Values;
        }

        /// <summary>
        /// This method will throw an exception if cards are not loaded beforehand or the id is invalid.
        /// You can prevent this by calling <see cref="SetDataSource.LoadSets"/> to load the Card instances beforehand.
        /// </summary>
        /// <param name="id">The id of the card</param>
        /// <returns>The <c>Card</c> instance that having the given id.</returns>
        public static PokemonCard GetCardById(string id)
        {
            return idsToCards[id];
        }

    }

}
