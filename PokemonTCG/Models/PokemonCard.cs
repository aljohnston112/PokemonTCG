using System.Collections.Generic;
using System.Collections.Immutable;

namespace PokemonTCG.Models
{

    internal enum CardSupertype
    {
        Pokemon,
        Trainer,
        Energy
    }

    internal enum CardSubtype
    {
        BASIC,
        STAGE_1,
        STAGE_2,
        SPECIAL,
    }

    internal enum PokemonType
    {
        Grass,
        Fire,
        Water,
        Lightning,
        Psychic,
        Fighting,
        Darkness,
        Metal,
        Fairy,
        Dragon,
        Colorless,
    }

    internal enum ImageSize
    {
        SMALL,
        LARGE
    }

    internal enum Rarity
    {
        NONE,
        RARE_HOLO,
        RARE,
        UNCOMMON,
        COMMON
    }

    internal enum Legality
    {
        UNLIMITED,
        EXPANDED,
        STANDARD
    }

    internal enum LegalType
    {
        LEGAL,
        ILLEGAL
    }


    /// <summary>
    /// Contains card data.
    /// </summary>
    internal class PokemonCard
    {
        internal readonly string Id;
        internal readonly string Name;
        internal readonly CardSupertype Supertype;
        internal readonly ImmutableList<CardSubtype> Subtypes;
        internal readonly int Level;
        internal readonly int Hp;
        internal readonly ImmutableList<PokemonType> Types;
        internal readonly string EvolvesFrom;
        internal readonly ImmutableList<Ability> Abilities;
        internal readonly ImmutableList<Attack> Attacks;
        internal readonly ImmutableDictionary<PokemonType, string> Weaknesses;
        internal readonly ImmutableDictionary<PokemonType, string> Resistances;
        internal readonly ImmutableDictionary<PokemonType, int> RetreatCost;
        internal readonly int ConvertedRetreatCost;
        internal readonly ImmutableDictionary<ImageSize, string> ImageFileNames;
        internal readonly string SetId;
        internal readonly string SetName;
        internal readonly string SetSeries;
        internal readonly int Number;
        internal readonly string Artist;
        internal readonly Rarity Rarity;
        internal readonly string FlavorText;
        internal readonly IDictionary<Legality, LegalType> Legalities;

        internal PokemonCard(
            string id,
            string name,
            CardSupertype supertype,
            List<CardSubtype> subtypes,
            int level,
            int hp,
            List<PokemonType> types,
            string evolvesFrom,
            List<Ability> abilities,
            List<Attack> attacks,
            Dictionary<PokemonType, string> weaknesses,
            Dictionary<PokemonType, string> resistances,
            Dictionary<PokemonType, int> retreatCost,
            int convertedRetreatCost,
            Dictionary<ImageSize, string> imageFileNames,
            string setId,
            string setName,
            string setSeries,
            int number,
            string artist,
            Rarity rarity,
            string flavorText,
            IDictionary<Legality, LegalType> legalities
        )
        {
            Id = id;
            Name = name;
            Supertype = supertype;
            Subtypes = subtypes.ToImmutableList();
            Level = level;
            Hp = hp;
            Types = types.ToImmutableList();
            EvolvesFrom = evolvesFrom;
            Abilities = abilities.ToImmutableList();
            Attacks = attacks.ToImmutableList();
            Weaknesses = weaknesses.ToImmutableDictionary();
            Resistances = resistances.ToImmutableDictionary();
            RetreatCost = retreatCost.ToImmutableDictionary();
            ConvertedRetreatCost = convertedRetreatCost;
            ImageFileNames = imageFileNames.ToImmutableDictionary();
            SetId = setId;
            SetName = setName;
            SetSeries = setSeries;
            Number = number;
            Artist = artist;
            Rarity = rarity;
            FlavorText = flavorText;
            Legalities = legalities;
        }

        private static readonly ImmutableDictionary<string, CardSupertype> cardSupertypeMap =
            new Dictionary<string, CardSupertype>()
            {
                { "Pokémon", CardSupertype.Pokemon },
                { "Trainer", CardSupertype.Trainer },
                { "Energy", CardSupertype.Energy }
            }.ToImmutableDictionary();

        internal static CardSupertype GetCardSuperType(string supertype)
        {
            CardSupertype cardSupertype = cardSupertypeMap[supertype];
            return cardSupertype;
        }

        private static readonly ImmutableDictionary<string, CardSubtype> cardSubtypeMap =
            new Dictionary<string, CardSubtype>()
            {
                { "Basic",  CardSubtype.BASIC },
                { "Stage 1",  CardSubtype.STAGE_1 },
                { "Stage 2",  CardSubtype.STAGE_2 },
                { "Special", CardSubtype.SPECIAL }
            }.ToImmutableDictionary();

        internal static CardSubtype GetCardSubType(string subtype)
        {
            CardSubtype cardSubtype = cardSubtypeMap[subtype];
            return cardSubtype;
        }

        /// <summary>
        /// For converting strings to the <c>PokemonType</c> the correspond to.
        /// </summary>
        private static readonly ImmutableDictionary<string, PokemonType> pokemonTypeMap =
            new Dictionary<string, PokemonType>(){
                { "Colorless", PokemonType.Colorless },
                { "Fighting", PokemonType.Fighting },
                { "Fire", PokemonType.Fire },
                { "Grass", PokemonType.Grass },
                { "Lightning", PokemonType.Lightning },
                { "Psychic", PokemonType.Psychic },
                { "Water", PokemonType.Water }
            }.ToImmutableDictionary();

        /// <summary>
        /// Takes a string representing a <c>PokemonType</c> and returns the corresponding <c>PokemonType</c>.
        /// </summary>
        /// <param name="pokemonType">The string that represents a <c>PokemonType</c></param>
        /// <returns>The <c>PokemonType</c> that the string corresponds to</returns>
        internal static PokemonType GetPokemonType(string pokemonType)
        {
            PokemonType type = pokemonTypeMap[pokemonType];
            return type;
        }

        private static readonly ImmutableDictionary<string, Rarity> rarityMap =
            new Dictionary<string, Rarity>()
            {
                { "", Models.Rarity.NONE },
                { "Rare Holo", Models.Rarity.RARE_HOLO },
                { "Rare", Models.Rarity.RARE },
                { "Uncommon", Models.Rarity.UNCOMMON },
                { "Common", Models.Rarity.COMMON}
            }.ToImmutableDictionary();

        internal static Rarity GetRarity(string rarity)
        {
            return rarityMap[rarity];
        }

        private static readonly ImmutableDictionary<string, Legality> legalityMap =
           new Dictionary<string, Legality>()
           {
               { "unlimited", Legality.UNLIMITED },
               { "expanded", Legality.EXPANDED },
               { "standard", Legality.STANDARD }
           }.ToImmutableDictionary();

        internal static Legality GetLegality(string legality)
        {
            return legalityMap[legality];
        }

        private static readonly ImmutableDictionary<string, LegalType> legalTypeMap =
           new Dictionary<string, LegalType>()
           {
               { "Legal", LegalType.LEGAL },
               { "Illegal", LegalType.ILLEGAL }
           }.ToImmutableDictionary();

        internal static LegalType GetLegalType(string legalType)
        {
            return legalTypeMap[legalType];
        }

        internal static PokemonType GetEnergyType(PokemonCard card)
        {
            string[] energyTypes = card.Name.Split();
            string energyType = energyTypes[^2];
            return GetPokemonType(energyType);
        }

        internal static bool IsBasicPokemon(PokemonCard card)
        {
            return card.Supertype == CardSupertype.Pokemon && card.Subtypes.Contains(CardSubtype.BASIC);
        }
    }

}
