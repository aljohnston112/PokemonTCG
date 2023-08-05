using System.Collections.Generic;

namespace PokemonTCG.Models
{
    public class Ability
    {
        public readonly string Name;
        public readonly string Text;
        public readonly string Type;

        /// <summary>
        /// Creates the Pokemon powers that cards have.
        /// </summary>
        /// <param name="name">The name of the Pokemon power</param>
        /// <param name="type">The type of the Pokemon power</param>
        public Ability(string name, string text, string type)
        {
            this.Name = name;
            this.Text = text;
            this.Type = type;
        }

    }
}