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

        internal void OnStateChange(GamePageViewModel gamePageViewModel)
        {
            HandCardActionState = new HandCardActionState(gamePageViewModel);
            if(gamePageViewModel.GameState.IsPreGame && gamePageViewModel.GameState.PlayerState.Active != null)
            {
                ShowStartGameButton = true;
            } else
            {
                ShowStartGameButton = false;
            }
        }

    }

}
