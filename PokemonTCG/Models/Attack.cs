using PokemonTCG.Enums;
using System.Collections.Generic;

namespace PokemonTCG.Models
{

    /// <summary>
    /// To be used for the <c>PokemonCard</c>s.
    /// </summary>
    internal class Attack
    {

        internal readonly string Name;
        internal readonly Dictionary<PokemonType, int> EnergyCost;
        internal readonly int ConvertedEnergyCost;
        internal readonly int Damage;
        internal readonly string Text;

        /// <summary>
        /// Creates an <c>Attack</c> to be used for the <c>PokemonCard</c>s.
        /// </summary>
        /// <param name="name">The name of the attack</param>
        /// <param name="energyCost">The energy cost of the attack</param>
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
