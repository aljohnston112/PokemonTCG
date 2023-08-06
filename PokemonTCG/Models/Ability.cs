using System.Collections.Generic;

namespace PokemonTCG.Models
{
    public class Ability
    {
        public readonly string Name;
        public readonly string Text;
        public readonly string Type;

        public Ability(string name, string text, string type)
        {
            this.Name = name;
            this.Text = text;
            this.Type = type;
        }

    }
}