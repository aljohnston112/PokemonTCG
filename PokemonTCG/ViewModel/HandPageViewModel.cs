using System;
using PokemonTCG.States;
using PokemonTCG.Utilities;

namespace PokemonTCG.ViewModel
{

    internal class HandPageViewModel: BindableBase
    {

        private HandCardActionState _handCardActionState;
        internal HandCardActionState HandCardActionState
        {
            get { return _handCardActionState; }
            set { SetProperty(ref _handCardActionState, value); }
        }

        private bool _showStartGameButton;
        internal bool ShowStartGameButton
        {
            get { return _showStartGameButton; }
            set { SetProperty(ref _showStartGameButton, value); }
        }

        internal void OnStateChange(GameState gameState, Action<GameState> onGameStateChanged)
        {
            HandCardActionState = new HandCardActionState(gameState, onGameStateChanged);
            if(gameState.IsPreGame && gameState.PlayerState.Active != null)
            {
                ShowStartGameButton = true;
            } else
            {
                ShowStartGameButton = false;
            }
        }

    }

}
