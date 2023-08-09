using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PokemonTCG.Models
{

    public enum CardSupertype
    {
        Pokemon,
        Trainer,
        Energy
    }

    public enum CardSubtype
    {
        BASIC,
        STAGE_1,
        STAGE_2,
        SPECIAL,
    }

    public enum PokemonType
    {
        Colorless,
        Fighting,
        Fire,
        Grass,
        Lightning,
        Psychic,
        Water
    }

    public enum ImageSize
    {
        SMALL,
        LARGE
    }

    public enum Rarity
    {
        NONE,
        RARE_HOLO,
        RARE,
        UNCOMMON,
        COMMON
    }

    public enum Legality
    {
        UNLIMITED,
        EXPANDED,
        STANDARD
    }

    public enum LegalType
    {
        LEGAL,
        ILLEGAL
    }


    /// <summary>
    /// Contains card data.
    /// </summary>
    public class PokemonCard
    {
        public readonly string Id;
        public readonly string Name;
        public readonly CardSupertype Supertype;
        public readonly ImmutableList<CardSubtype> Subtypes;
        public readonly int Level;
        public readonly int Hp;
        public readonly ImmutableList<PokemonType> Types;
        public readonly string EvolvesFrom;
        public readonly ImmutableList<Ability> Abilities;
        public readonly ImmutableList<Attack> Attacks;
        public readonly ImmutableDictionary<PokemonType, string> Weaknesses;
        public readonly ImmutableDictionary<PokemonType, string> Resistances;
        public readonly ImmutableDictionary<PokemonType, int> RetreatCost;
        public readonly int ConvertedRetreatCost;
        public readonly ImmutableDictionary<ImageSize, string> ImageFileNames;

        public readonly string SetId;
        public readonly string SetName;
        public readonly string SetSeries;
        public readonly int Number;
        public readonly string Artist;
        public readonly Rarity Rarity;
        public readonly string FlavorText;
        public readonly IDictionary<Legality, LegalType> Legalities;

        public PokemonCard(
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
            this.Id = id;
            this.Name = name;
            this.Supertype = supertype;
            this.Subtypes = subtypes.ToImmutableList();
            this.Level = level;
            this.Hp = hp;
            this.Types = types.ToImmutableList();
            this.EvolvesFrom = evolvesFrom;
            this.Abilities = abilities.ToImmutableList();
            this.Attacks = attacks.ToImmutableList();
            this.Weaknesses = weaknesses.ToImmutableDictionary();
            this.Resistances = resistances.ToImmutableDictionary();
            this.RetreatCost = retreatCost.ToImmutableDictionary();
            this.ConvertedRetreatCost = convertedRetreatCost;
            this.ImageFileNames = imageFileNames.ToImmutableDictionary();
            this.SetId = setId;
            this.SetName = setName;
            this.SetSeries = setSeries;
            this.Number = number;
            this.Artist = artist;
            this.Rarity = rarity;
            this.FlavorText = flavorText;
            this.Legalities = legalities;
        }

        private static readonly ImmutableDictionary<string, CardSupertype> cardSupertypeMap =
            new Dictionary<string, CardSupertype>()
            {
                { "Pokémon", CardSupertype.Pokemon },
                { "Trainer", CardSupertype.Trainer },
                { "Energy", CardSupertype.Energy }
            }.ToImmutableDictionary();

        public static CardSupertype GetCardSuperType(string supertype)
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

        public static CardSubtype GetCardSubType(string subtype)
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
        public static PokemonType GetPokemonType(string pokemonType)
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

        public static Rarity GetRarity(string rarity)
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

        public static Legality GetLegality(string legality)
        {
            return legalityMap[legality];
        }

        private static readonly ImmutableDictionary<string, LegalType> legalTypeMap =
           new Dictionary<string, LegalType>()
           {
               { "Legal", LegalType.LEGAL },
               { "Illegal", LegalType.ILLEGAL }
           }.ToImmutableDictionary();

        public static LegalType GetLegalType(string legalType)
        {
            return legalTypeMap[legalType];
        }

        internal static PokemonType GetEnergyType(PokemonCard card)
        {
            string[] energyTypes = card.Name.Split();
            string energyType = energyTypes[^2];
            return GetPokemonType(energyType);
        }

    }

}
