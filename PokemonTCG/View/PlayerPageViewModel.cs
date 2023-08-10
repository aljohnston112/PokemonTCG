using System.ComponentModel;
using Microsoft.UI.Xaml.Media;
using PokemonTCG.Models;
using PokemonTCG.Utilities;

namespace PokemonTCG.View
{
    public class PlayerPageViewModel: BindableBase
    {

        private PlayerState _playerState;
        public PlayerState PlayerState
        {
            get { return _playerState; }
            set { SetProperty(ref _playerState, value); }
        }

        public void OnStateChange(PlayerState playerState)
        {
            PlayerState = playerState;
        }

    }

}
