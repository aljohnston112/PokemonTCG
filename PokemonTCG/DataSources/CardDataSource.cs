using PokemonTCG.CardModels;
using PokemonTCG.Enums;
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
        private static readonly IDictionary<string, PokemonCard> idsToCards = new Dictionary<string, PokemonCard>();
        private static readonly Dictionary<string, IImmutableList<PokemonCard>> SetsToCards = new();

        internal static readonly string setFolder = "\\Assets\\sets\\";

        /// <summary>
        /// This method will load cards if it cannot find the specified card.
        /// </summary>
        /// <param name="id">The id of the card.</param>
        /// <returns>The <c>Card</c> instance that has the given id.</returns>
        internal static async Task<PokemonCard> GetCardById(string id)
        {
            if (!idsToCards.ContainsKey(id))
            {
                _ = await LoadSets();
            }
            return idsToCards[id];
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
        /// Loads all <c>Card</c> instances.
        /// </summary>
        /// <returns>A dictionary of set names to the list of cards in each set</returns>
        internal static async Task<IImmutableDictionary<string, IImmutableList<PokemonCard>>> LoadSets()
        {
            StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(FileUtil.GetFullPath(setFolder));
            StorageFileQueryResult query = storageFolder.CreateFileQueryWithOptions(GetQueryOptionsForSetFolder());
            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            // sets.json is the file containing information for all the sets
            foreach (StorageFile setFile in fileList.Where(name => name.Name != "sets.json"))
            {
                string setName = setFile.Name[..setFile.Name.LastIndexOf(".")];
                if (!SetsToCards.ContainsKey(setName))
                {
                    IImmutableList<PokemonCard> cards = await LoadCardsFromSet(setFile);
                    SetsToCards.Add(setName, cards);
                }
            }
            return SetsToCards.ToImmutableDictionary();
        }

        private static QueryOptions GetQueryOptionsForSetFolder()
        {
            List<string> fileTypeFilter = new() { ".json" };
            return new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
        }

        /// <summary>
        /// Gets all cards associated with a set. 
        /// This method takes a File because the <c>CardDataSource</c> iterates through a folder of set files.
        /// </summary>
        /// <param name="setFile">The json file containing the card information for the set.</param>
        /// <returns>A <c>Task<ICollection<Card>>/c> that returns the collection of cards in the set.</returns>
        /// <exception cref="Exception">Throws an exception if the supertype of the card is not "Pok\u00e9mon", "Trainer", or "Energy"</exception>
        internal async static Task<IImmutableList<PokemonCard>> LoadCardsFromSet(StorageFile setFile)
        {
            string jsonText = await FileIO.ReadTextAsync(setFile);
            JsonObject jObject = JsonObject.Parse(jsonText);
            JsonArray jArray = jObject.GetNamedArray("data");

            string baseImagePath = setFile.Path[(setFile.Path.LastIndexOf("Assets") - 1)..(setFile.Path.LastIndexOf("."))] + "\\";
            foreach (IJsonValue jsonCardValue in jArray)
            {
                JsonObject jsonCardObject = jsonCardValue.GetObject();
                string id = jsonCardObject.GetNamedString("id");

                if (!idsToCards.ContainsKey(id))
                {
                    JsonObject jsonSetObject = jsonCardObject.GetNamedObject("set");
                    PokemonCard card = new(
                        id: id,
                        name: jsonCardObject.GetNamedString("name"),
                        supertype: EnumUtil.Parse<CardSupertype>(jsonCardObject.GetNamedString("supertype")),
                        subtypes: GetJsonCardSubtypes(jsonCardObject),
                        level: GetJsonCardLevel(jsonCardObject),
                        hp: GetJsonCardHP(jsonCardObject),
                        types: GetJsonCardTypes(jsonCardObject),
                        evolvesFrom: GetJsonCardEvolvesFrom(jsonCardObject),
                        abilities: GetJsonCardAbilities(jsonCardObject),
                        attacks: GetJsonCardAttacks(jsonCardObject),
                        weaknesses: GetJsonCardWeaknesses(jsonCardObject),
                        resistances: GetJsonCardResistances(jsonCardObject),
                        retreatCost: GetJsonCardRetreatCost(jsonCardObject),
                        convertedRetreatCost: GetJsonCardConvertedRetreatCost(jsonCardObject),
                        imagePath: baseImagePath + id + "-large.png",
                        setId: jsonSetObject.GetNamedString("id"),
                        setName: jsonSetObject.GetNamedString("name"),
                        setSeries: jsonSetObject.GetNamedString("series"),
                        number: int.Parse(jsonCardObject.GetNamedString("number")),
                        artist: jsonCardObject.GetNamedString("artist"),
                        rarity: GetJsonCardRarities(jsonCardObject),
                        flavorText: GetJsonCardFlavorText(jsonCardObject),
                        legalities: GetJsonCardLegalities(jsonCardObject)
                        );
                    idsToCards.Add(id, card);
                }
            }
            return idsToCards.Values.ToImmutableList();
        }

        private static IImmutableList<CardSubtype> GetJsonCardSubtypes(JsonObject jsonCardObject)
        {
            List<CardSubtype> subtypes = new();
            if (jsonCardObject.ContainsKey("subtypes"))
            {
                JsonArray jsonSubtypeArray = jsonCardObject.GetNamedArray("subtypes");
                foreach (IJsonValue jsonSubtypeValue in jsonSubtypeArray)
                {
                    subtypes.Add(EnumUtil.Parse<CardSubtype>(jsonSubtypeValue.GetString()));
                }
            }
            return subtypes.ToImmutableList();
        }

        private static int GetJsonCardLevel(JsonObject jsonCardObject)
        {
            int level = 0;
            if (jsonCardObject.ContainsKey("level"))
            {
                level = int.Parse(jsonCardObject.GetNamedString("level"));
            }
            return level;
        }

        private static int GetJsonCardHP(JsonObject jsonCardObject)
        {
            int hp = 0;
            if (jsonCardObject.ContainsKey("hp"))
            {
                hp = int.Parse(jsonCardObject.GetNamedString("hp"));
            }
            return hp;
        }

        private static IImmutableList<PokemonType> GetJsonCardTypes(JsonObject jsonCardObject)
        {
            List<PokemonType> pokemonTypes = new();
            if (jsonCardObject.ContainsKey("types"))
            {
                JsonArray jsonTypeArray = jsonCardObject.GetNamedArray("types");
                foreach (IJsonValue jsonTypeValue in jsonTypeArray)
                {
                    pokemonTypes.Add(EnumUtil.Parse<PokemonType>(jsonTypeValue.GetString()));
                }
            }
            return pokemonTypes.ToImmutableList();
        }

        private static string GetJsonCardEvolvesFrom(JsonObject jsonCardObject)
        {
            string evolvesFrom = null;
            if (jsonCardObject.ContainsKey("evolvesFrom"))
            {
                evolvesFrom = jsonCardObject.GetNamedString("evolvesFrom");
            }
            return evolvesFrom;
        }

        private static IImmutableList<Ability> GetJsonCardAbilities(JsonObject jsonCardObject)
        {
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
            return abilities.ToImmutableList();
        }

        private static IImmutableList<Attack> GetJsonCardAttacks(JsonObject jsonCardObject)
        {
            List<Attack> attacks = new();
            if (jsonCardObject.ContainsKey("attacks"))
            {
                JsonArray jsonAttackArray = jsonCardObject.GetNamedArray("attacks");
                foreach (IJsonValue jsonAttackValue in jsonAttackArray)
                {
                    JsonObject jsonAttackObject = jsonAttackValue.GetObject();
                    attacks.Add(
                        new Attack(
                            name: jsonAttackObject.GetNamedString("name"),
                            energyCost: GetJsonAttackEnergyCost(jsonAttackObject),
                            convertedEnergyCost: (int)(jsonAttackObject.GetNamedNumber("convertedEnergyCost")),
                            damage: GetJsonAttackDamage(jsonAttackObject),
                            text: jsonAttackObject.GetNamedString("text")
                            )
                        );
                }
            }
            return attacks.ToImmutableList();
        }

        private static ImmutableDictionary<PokemonType, int> GetJsonAttackEnergyCost(JsonObject jsonAttackObject)
        {
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
            return attackCost.ToImmutableDictionary();
        }

        private static int GetJsonAttackDamage(JsonObject jsonAttackObject)
        {
            string attackDamageString = jsonAttackObject.GetNamedString("damage");
            if (attackDamageString.EndsWith("×") || attackDamageString.EndsWith("+") || attackDamageString.EndsWith("-"))
            {
                attackDamageString = attackDamageString[..^1];
            }
            else if (attackDamageString == "")
            {
                attackDamageString = "0";
            }
            return int.Parse(attackDamageString);
        }

        private static IImmutableDictionary<PokemonType, Modifier> GetJsonCardWeaknesses(JsonObject jsonCardObject)
        {
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
            return weaknesses.ToImmutableDictionary();
        }

        private static IImmutableDictionary<PokemonType, Modifier> GetJsonCardResistances(JsonObject jsonCardObject)
        {
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
            return resistances.ToImmutableDictionary();
        }

        private static IImmutableDictionary<PokemonType, int> GetJsonCardRetreatCost(JsonObject jsonCardObject)
        {
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
            return retreatCost.ToImmutableDictionary();
        }

        private static int GetJsonCardConvertedRetreatCost(JsonObject jsonCardObject)
        {
            int convertedRetreatCost = 0;
            if (jsonCardObject.ContainsKey("retreatCost"))
            {
                convertedRetreatCost = (int)(jsonCardObject.GetNamedNumber("convertedRetreatCost"));
            }
            return convertedRetreatCost;
        }

        private static Rarity GetJsonCardRarities(JsonObject jsonCardObject)
        {
            Rarity rarity = Rarity.NONE;
            if (jsonCardObject.ContainsKey("rarity"))
            {
                rarity = EnumUtil.Parse<Rarity>(jsonCardObject.GetNamedString("rarity"));
            }
            return rarity;
        }

        private static string GetJsonCardFlavorText(JsonObject jsonCardObject)
        {
            string flavorText = null;
            if (jsonCardObject.ContainsKey("flavorText"))
            {
                flavorText = jsonCardObject.GetNamedString("flavorText");
            }
            return flavorText;
        }

        private static IImmutableDictionary<LegalFormat, Legality> GetJsonCardLegalities(JsonObject jsonCardObject)
        {
            Dictionary<LegalFormat, Legality> legalities = new();
            foreach (KeyValuePair<string, IJsonValue> jsonLegalityPair in jsonCardObject.GetNamedObject("legalities"))
            {
                legalities[EnumUtil.Parse<LegalFormat>(jsonLegalityPair.Key)] = EnumUtil.Parse<Legality>(jsonLegalityPair.Value.GetString());
            }
            return legalities.ToImmutableDictionary();
        }

    }

}