using System.Collections.Generic;
using System.Collections.Immutable;

namespace PokemonTCG.Models { 
    /// <summary>
    /// The types that a Pokmeon card can have.
    /// </summary>
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

    public class Type
    {

        /// <summary>
        /// For converting strings to the <c>PokemonType</c> the correspond to.
        /// </summary>
        private static ImmutableDictionary<string, PokemonType> _typeMap =
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
        public static PokemonType GetType(string pokemonType)
        {
            PokemonType type = _typeMap[pokemonType];
            return type;
        }

    }

}
