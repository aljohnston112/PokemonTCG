using PokemonTCG.Enums;

using System.Collections.Immutable;

namespace PokemonTCG.CardModels
{

    internal class Attack
    {

        internal readonly string Name;
        internal readonly IImmutableDictionary<PokemonType, int> EnergyCost;
        internal readonly int ConvertedEnergyCost;
        internal readonly int Damage;
        internal readonly string Text;

        internal Attack(
            string name,
            IImmutableDictionary<PokemonType, int> energyCost,
            int convertedEnergyCost,
            int damage,
            string text
            )
        {
            EnergyCost = energyCost;
            Name = name;
            ConvertedEnergyCost = convertedEnergyCost;
            Damage = damage;
            Text = text;
        }

    }

}
