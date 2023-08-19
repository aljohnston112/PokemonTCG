using PokemonTCG.States;
using PokemonTCG.Utilities;
using PokemonTCG.ViewModel;

namespace PokemonTCG.View
{

    internal class PlayerPageViewModel: BindableBase
    {

        private PlayerState _playerState = null;
        internal PlayerState PlayerState
        {
            get { return _playerState; }
            set { SetProperty(ref _playerState, value); }
        }

        private FieldCardActionState _fieldCardActionState;
        internal FieldCardActionState FieldCardActionState
        {
            get { return _fieldCardActionState; }
            set { SetProperty(ref _fieldCardActionState, value); }
        }

        internal void OnStateChange(GamePageViewModel gamePageViewModel, bool isPlayerState)
        {
            if (isPlayerState)
            {
                FieldCardActionState = new FieldCardActionState(gamePageViewModel);
                PlayerState = gamePageViewModel.GameState.PlayerState;
            }
            else
            {
                PlayerState = gamePageViewModel.GameState.OpponentState;
            }
        }

    }

}
