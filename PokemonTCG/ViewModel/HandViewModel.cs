using PokemonTCG.Models;
using PokemonTCG.Utilities;

namespace PokemonTCG.ViewModel
{

    internal class HandViewModel: BindableBase
    {

        private HandCardActionState _handCardActionState;
        internal HandCardActionState HandCardActionState
        {
            get { return _handCardActionState; }
            set { SetProperty(ref _handCardActionState, value); }
        }

        internal void GameStateChanged(GameState gameState)
        {
            HandCardActionState = new HandCardActionState(gameState);
        }

    }

}
