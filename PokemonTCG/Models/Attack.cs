using PokemonTCG.Enums;
using System.Collections.Generic;

namespace PokemonTCG.Models
{

    internal class Attack
    {

        internal readonly string Name;
        internal readonly Dictionary<PokemonType, int> EnergyCost;
        internal readonly int ConvertedEnergyCost;
        internal readonly int Damage;
        internal readonly string Text;

        internal Attack(
            string name, 
            Dictionary<PokemonType, int> energyCost, 
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
