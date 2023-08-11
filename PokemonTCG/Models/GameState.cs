using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using PokemonTCG.DataSources;
using PokemonTCG.Utilities;

namespace PokemonTCG.Models
{
    /// <summary>
    /// The state of the game. To be used for the <c>GamePage</c>
    /// </summary>
    public class GameState
    {
        internal readonly PlayerState PlayerState;
        internal readonly PlayerState OpponentState;

        internal GameState(
            PlayerState playerState, 
            PlayerState newOpponentState
            )
        {
            PlayerState = playerState;
            OpponentState = newOpponentState;
        }

    }

}
