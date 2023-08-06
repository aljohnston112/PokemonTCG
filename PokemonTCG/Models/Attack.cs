using System.Collections.Generic;

namespace PokemonTCG.Models
{

    /// <summary>
    /// To be used for the <c>PokemonCard</c>s.
    /// </summary>
    public class Attack
    {

        public readonly string Name;
        public readonly Dictionary<PokemonType, int> EnergyCost;
        public readonly int ConvertedEnergyCost;
        public readonly int Damage;
        public readonly string Text;

        /// <summary>
        /// Creates an <c>Attack</c> to be used for the <c>PokemonCard</c>s.
        /// </summary>
        /// <param name="name">The name of the attack</param>
        /// <param name="energyCost">The energy cost of the attack</param>
        public Attack(
            string name, 
            Dictionary<PokemonType, int> energyCost, 
            int convertedEnergyCost,
            int damage, 
            string text
            )
        {
            this.EnergyCost = energyCost;
            this.Name = name;
            this.ConvertedEnergyCost = convertedEnergyCost;
            this.Damage = damage;
            this.Text = text;
        }

    }

}
