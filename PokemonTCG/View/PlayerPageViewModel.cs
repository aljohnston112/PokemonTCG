using System;
using System.ComponentModel;
using Microsoft.UI.Xaml.Media;
using PokemonTCG.Models;
using PokemonTCG.Utilities;

namespace PokemonTCG.View
{

    public class PlayerPageViewModel: BindableBase
    {

        Action<PlayerState> OnPlayerStateChanged;

        public void SetOnPlayerStateChanged(Action<PlayerState> onPlayerStateChanged)
        {
            OnPlayerStateChanged = onPlayerStateChanged;
        }

        public void OnStateChange(PlayerState playerState)
        {
            OnPlayerStateChanged(playerState);
        }

    }

}
