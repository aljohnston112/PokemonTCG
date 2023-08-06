using PokemonTCG.Models;
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

    /// <summary>
    /// Contains card data.
    /// </summary>
    public class PokemonCard
    {
        public readonly string Id;
        public readonly string Name;
        public readonly CardSupertype Supertype;
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

        public PokemonCard(
            string id,
            string name,
            CardSupertype supertype,
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
            Dictionary<ImageSize, string> imageFileNames
        )
        {
            this.Id = id;
            this.Name = name;
            this.Supertype = supertype;
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
                { "Stage 2",  CardSubtype.STAGE_2 }
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

    }

}
