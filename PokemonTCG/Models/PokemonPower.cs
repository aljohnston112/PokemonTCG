using System.Collections.Generic;

namespace PokemonTCG.Models
{
    public class PokemonPower
    {
        public readonly string Name;
        public readonly string Type;

        /// <summary>
        /// Creates the Pokemon powers that cards have.
        /// </summary>
        /// <param name="name">The name of the Pokemon power</param>
        /// <param name="type">The type of the Pokemon power</param>
        public PokemonPower(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }

    }
}