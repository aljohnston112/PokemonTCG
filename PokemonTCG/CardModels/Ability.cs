using PokemonTCG.Enums;

namespace PokemonTCG.CardModels
{
    internal class Ability
    {
        internal readonly string Name;
        internal readonly string Text;
        internal readonly AbilityType Type;

        internal Ability(string name, string text, AbilityType type)
        {
            Name = name;
            Text = text;
            Type = type;
        }

    }

}