using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.Models;
using PokemonTCG.Utilities;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Search;

namespace PokemonTCG.DataSources
{

    internal class CardDataSource
    {
        private static readonly Dictionary<string, IImmutableList<PokemonCard>> SetsToCards = new();
        private static readonly IDictionary<string, PokemonCard> idsToCards = new Dictionary<string, PokemonCard>();

        internal static readonly string setFolder = "\\Assets\\sets\\";

        /// <summary>
        /// Loads all <c>Card</c> instances.
        /// </summary>
        internal static async Task<IImmutableDictionary<string, IImmutableList<PokemonCard>>> LoadSets()
        {
            StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(FileUtil.GetFullPath(setFolder));
            List<string> fileTypeFilter = new() { ".json" };
            QueryOptions queryOptions = new(CommonFileQuery.OrderByName, fileTypeFilter);
            StorageFileQueryResult query = storageFolder.CreateFileQueryWithOptions(queryOptions);
            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            foreach (StorageFile file in fileList.Where(name => name.Name != "sets.json"))
            {
                string setName = file.Name[..file.Name.IndexOf(".")];
                if (!SetsToCards.ContainsKey(setName))
                {
                    IImmutableList<PokemonCard> cards = await CardDataSource.LoadCardsFromSet(file);
                    SetsToCards.Add(setName, cards);
                }
            }
            return SetsToCards.ToImmutableDictionary();
        }

        internal static async Task<IImmutableList<PokemonCard>> LoadSet(string setName)
        {
            if (!SetsToCards.ContainsKey(setName))
            {
                await LoadSets();
            }
            return SetsToCards[setName];
        }

        /// <summary>
        /// Gets all cards associated with a set. 
        /// This method takes a File because the <c>CardDataSource</c> iterates through a folder of set files.
        /// </summary>
        /// <param name="file">The json file containing the card information for the set.</param>
        /// <returns>A <c>Task<ICollection<Card>>/c> that returns the collection of cards in the set.</returns>
        /// <exception cref="Exception">Throws an exception if the supertype of the card is not "Pok\u00e9mon", "Trainer", or "Energy"</exception>
        internal async static Task<IImmutableList<PokemonCard>> LoadCardsFromSet(StorageFile file)
        {
            // Read the file
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jObject = JsonObject.Parse(jsonText);
            JsonArray jArray = jObject.GetNamedArray("data");

            string baseImagePath = file.Path[(file.Path.IndexOf("Assets") - 1)..(file.Path.LastIndexOf("."))] + "\\";
            foreach (IJsonValue jsonCardValue in jArray)
            {
                JsonObject jsonCardObject = jsonCardValue.GetObject();
                string id = jsonCardObject.GetNamedString("id");

                if (!idsToCards.ContainsKey(id))
                {
                    string name = jsonCardObject.GetNamedString("name");
                    CardSupertype supertype = EnumUtil.Parse<CardSupertype>(jsonCardObject.GetNamedString("supertype"));

                    // Subtypes
                    List<CardSubtype> subtypes = new();
                    if (jsonCardObject.ContainsKey("subtypes"))
                    {
                        JsonArray jsonSubtypeArray = jsonCardObject.GetNamedArray("subtypes");
                        foreach (IJsonValue jsonSubtypeValue in jsonSubtypeArray)
                        {
                            subtypes.Add(EnumUtil.Parse<CardSubtype>(jsonSubtypeValue.GetString()));
                        }
                    }

                    int level = 0;
                    if (jsonCardObject.ContainsKey("level"))
                    {
                        level = int.Parse(jsonCardObject.GetNamedString("level"));
                    }

                    int hp = 0;
                    if (jsonCardObject.ContainsKey("hp"))
                    {
                        hp = int.Parse(jsonCardObject.GetNamedString("hp"));
                    }

                    // Pokemon types
                    List<PokemonType> pokemonTypes = new();
                    if (jsonCardObject.ContainsKey("types"))
                    {
                        JsonArray jsonTypeArray = jsonCardObject.GetNamedArray("types");
                        foreach (IJsonValue jsonTypeValue in jsonTypeArray)
                        {
                            pokemonTypes.Add(EnumUtil.Parse<PokemonType>(jsonTypeValue.GetString()));
                        }
                    }

                    // Evolves from
                    string evolvesFrom = null;
                    if (jsonCardObject.ContainsKey("evolvesFrom"))
                    {
                        evolvesFrom = jsonCardObject.GetNamedString("evolvesFrom");
                    }

                    // Abilities
                    List<Ability> abilities = new();
                    if (jsonCardObject.ContainsKey("abilities"))
                    {
                        JsonArray jsonAbilityList = jsonCardObject.GetNamedArray("abilities");
                        foreach (IJsonValue jsonAbilityValue in jsonAbilityList)
                        {
                            JsonObject jsonAbility = jsonAbilityValue.GetObject();
                            string abilityName = jsonAbility.GetNamedString("name");
                            string abilityText = jsonAbility.GetNamedString("text");
                            AbilityType abilityType = EnumUtil.Parse<AbilityType>(jsonAbility.GetNamedString("type"));
                            abilities.Add(
                                new Ability(
                                    name: abilityName,
                                    text: abilityText,
                                    abilityType
                                    )
                                );
                        }
                    }

                    // Attacks
                    List<Attack> attacks = new();
                    if (jsonCardObject.ContainsKey("attacks"))
                    {
                        JsonArray jsonAttackArray = jsonCardObject.GetNamedArray("attacks");
                        foreach (IJsonValue jsonAttackValue in jsonAttackArray)
                        {
                            JsonObject jsonAttackObject = jsonAttackValue.GetObject();
                            string attackName = jsonAttackObject.GetNamedString("name");

                            // Energy cost
                            Dictionary<PokemonType, int> attackCost = new();
                            JsonArray jsonAttackCostArray = jsonAttackObject.GetNamedArray("cost");
                            foreach (IJsonValue jsonPokemonTypeValue in jsonAttackCostArray)
                            {
                                PokemonType pokemonType = EnumUtil.Parse<PokemonType>(jsonPokemonTypeValue.GetString());
                                if (!attackCost.ContainsKey(pokemonType))
                                {
                                    attackCost[pokemonType] = 0;
                                }
                                attackCost[pokemonType] += 1;
                            }
                            int convertedEnergyCostForAttack = (int)(jsonAttackObject.GetNamedNumber("convertedEnergyCost"));

                            string attackDamageString = jsonAttackObject.GetNamedString("damage");
                            if (attackDamageString.EndsWith("×") || attackDamageString.EndsWith("+") || attackDamageString.EndsWith("-"))
                            {
                                attackDamageString = attackDamageString[..^1];
                            }
                            else if (attackDamageString == "")
                            {
                                attackDamageString = "0";
                            }
                            int attackDamage = int.Parse(attackDamageString);

                            string attackText = jsonAttackObject.GetNamedString("text");
                            attacks.Add(
                                new Attack(
                                    name: attackName, 
                                    energyCost : attackCost, 
                                    convertedEnergyCost: convertedEnergyCostForAttack, 
                                    damage: attackDamage,
                                    text: attackText
                                    )
                                );
                        }
                    }

                    // Weaknesses
                    Dictionary<PokemonType, Modifier> weaknesses = new();
                    if (jsonCardObject.ContainsKey("weaknesses"))
                    {
                        JsonArray jsonWeaknessArray = jsonCardObject.GetNamedArray("weaknesses");
                        foreach (IJsonValue weaknessValue in jsonWeaknessArray)
                        {
                            JsonObject jsonWeaknessObject = weaknessValue.GetObject();

                            PokemonType weaknessType = EnumUtil.Parse<PokemonType>(jsonWeaknessObject.GetNamedString("type"));
                            string weaknessModifierString = jsonWeaknessObject.GetNamedString("value");
                            Modifier weaknessModifier = new(
                                EnumUtil.Parse<ModifierType>(
                                    weaknessModifierString[..1]
                                    .Replace("×", "times")
                                    .Replace("-", "minus")
                                    ),
                                int.Parse(weaknessModifierString[1..])
                                );
                            weaknesses[weaknessType] = weaknessModifier;
                        }
                    }

                    // Resistances
                    Dictionary<PokemonType, Modifier> resistances = new();
                    if (jsonCardObject.ContainsKey("resistances"))
                    {
                        JsonArray resistanceArray = jsonCardObject.GetNamedArray("resistances");
                        foreach (IJsonValue resistanceValue in resistanceArray)
                        {
                            JsonObject jsonresistance = resistanceValue.GetObject();
                            string resistanceType = jsonresistance.GetNamedString("type");
                            string resistanceModifierString = jsonresistance.GetNamedString("value");
                            Modifier resistanceModifier = new(
                                EnumUtil.Parse<ModifierType>(
                                    resistanceModifierString[..1]
                                    .Replace("×", "times")
                                    .Replace("-", "minus")
                                    ),
                                int.Parse(resistanceModifierString[1..])
                                );
                            resistances[EnumUtil.Parse<PokemonType>(resistanceType)] = resistanceModifier;
                        }
                    }

                    // Retreat cost
                    Dictionary<PokemonType, int> retreatCost = new();
                    if (jsonCardObject.ContainsKey("retreatCost"))
                    {
                        JsonArray jsonRetreatCostArray = jsonCardObject.GetNamedArray("retreatCost");
                        foreach (IJsonValue jsonRetreatCostValue in jsonRetreatCostArray)
                        {
                            string retreatCostString = jsonRetreatCostValue.GetString();
                            PokemonType pokemontype = EnumUtil.Parse<PokemonType>(retreatCostString);
                            if (!retreatCost.ContainsKey(pokemontype))
                            {
                                retreatCost[pokemontype] = 0;
                            }
                            retreatCost[pokemontype] += 1;
                        }
                    }

                    int convertedRetreatCost = 0;
                    if (jsonCardObject.ContainsKey("retreatCost"))
                    {
                        convertedRetreatCost = (int)(jsonCardObject.GetNamedNumber("convertedRetreatCost"));
                    }

                    // Image path
                    string imagePath = baseImagePath + id + "-large.png";

                    JsonObject jsonSetObject = jsonCardObject.GetNamedObject("set");
                    string setId = jsonSetObject.GetNamedString("id");
                    string setName = jsonSetObject.GetNamedString("name");
                    string setSeries = jsonSetObject.GetNamedString("series");
                    int cardNumber = int.Parse(jsonCardObject.GetNamedString("number"));
                    string artist = jsonCardObject.GetNamedString("artist");

                    Rarity rarity = Rarity.NONE;
                    if (jsonCardObject.ContainsKey("rarity"))
                    {
                        rarity = EnumUtil.Parse<Rarity>(jsonCardObject.GetNamedString("rarity"));
                    }
                    string flavorText = null;
                    if (jsonCardObject.ContainsKey("flavorText"))
                    {
                        flavorText = jsonCardObject.GetNamedString("flavorText");
                    }

                    // Legalities
                    Dictionary<LegalFormat, Legality> legalities = new();
                    foreach (KeyValuePair<string, IJsonValue> jsonLegalityPair in jsonCardObject.GetNamedObject("legalities"))
                    {
                        legalities[EnumUtil.Parse<LegalFormat>(jsonLegalityPair.Key)] = EnumUtil.Parse<Legality>(jsonLegalityPair.Value.GetString());
                    }

                    PokemonCard card = new(
                        id: id,
                        name: name,
                        supertype: supertype,
                        subtypes: subtypes.ToImmutableList(),
                        level: level,
                        hp: hp,
                        types: pokemonTypes.ToImmutableList(),
                        evolvesFrom: evolvesFrom,
                        abilities: abilities.ToImmutableList(),
                        attacks: attacks.ToImmutableList(),
                        weaknesses: weaknesses.ToImmutableDictionary(),
                        resistances: resistances.ToImmutableDictionary(),
                        retreatCost: retreatCost.ToImmutableDictionary(),
                        convertedRetreatCost: convertedRetreatCost,
                        imagePath: imagePath,
                        setId: setId,
                        setName: setName,
                        setSeries: setSeries,
                        number: cardNumber,
                        artist: artist,
                        rarity: rarity,
                        flavorText: flavorText,
                        legalities: legalities.ToImmutableDictionary()
                        );
                    idsToCards.Add(id, card);
                }
            }
            return idsToCards.Values.ToImmutableList();
        }

        /// <summary>
        /// This method will load cards if it cannot find the specified card.
        /// </summary>
        /// <param name="id">The id of the card.</param>
        /// <returns>The <c>Card</c> instance that havs the given id.</returns>
        internal static async Task<PokemonCard> GetCardById(string id)
        {
            if(!idsToCards.ContainsKey(id))
            {
                _ = await LoadSets();
            }
            return idsToCards[id];
        }

    }

}
