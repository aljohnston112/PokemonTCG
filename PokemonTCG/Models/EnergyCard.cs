using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTCG.Models
{
    /// <summary>
    /// To be used with the <c>GamePage</c>
    /// </summary>
    class EnergyCard : Card
    {

        public readonly PokemonType Type;

        /// <summary>
        /// Creates an <c>EnergyCard</c> to be used in the <c>GamePage</c>.
        /// </summary>
        /// <param name="imageFileName">The card image's url</param>
        /// <param name="id">The unique id of the card</param>
        /// <param name="name">The name of the card</param>
        /// <param name="type">The type of the energy</param>
        public EnergyCard(
            string imageFileName,
            string id,
            string name,
            PokemonType type
        ) : base(imageFileName, id, name)
        {
            Type = type;
        }

    }
}
