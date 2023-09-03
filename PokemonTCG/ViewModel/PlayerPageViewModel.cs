using PokemonTCG.States;
using PokemonTCG.Utilities;

using System;

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

        internal void OnStateChange(GameState gameState, Action<GameState> onGameStateChanged, bool isPlayerState)
        {
            if (isPlayerState)
            {
                FieldCardActionState = new FieldCardActionState(
                    gameState, 
                    onGameStateChanged
                    );
                PlayerState = gameState.PlayerState;
            }
            else
            {
                PlayerState = gameState.OpponentState;
            }
        }

    }

}