using System;
using PokemonTCG.States;

namespace PokemonTCG.View
{

    internal class PlayerPageViewModel
    {

        Action<PlayerState> OnPlayerStateChanged;

        internal void SetOnPlayerStateChanged(Action<PlayerState> onPlayerStateChanged)
        {
            OnPlayerStateChanged = onPlayerStateChanged;
        }

        internal void OnStateChange(PlayerState playerState)
        {
            OnPlayerStateChanged(playerState);
        }

    }

}
