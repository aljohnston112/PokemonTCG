namespace PokemonTCG.Models
{
    internal class Ability
    {
        internal readonly string Name;
        internal readonly string Text;
        internal readonly string Type;

        internal Ability(string name, string text, string type)
        {
            Name = name;
            Text = text;
            Type = type;
        }

    }
}