using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTCG.Models
{
    /// <summary>
    /// The state of the game. To be used for the <c>GamePage</c>
    /// </summary>
    public class GameState
    {
        private PlayerState _playerState1 = new();
        private PlayerState _playerState2 = new();

        /// <summary>
        /// Creates a new <c>GameState</c>
        /// </summary>
        private GameState()
        {

        }

    }
}
