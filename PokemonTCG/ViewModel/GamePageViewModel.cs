using PokemonTCG.Models;
using PokemonTCG.Utilities;

namespace PokemonTCG.ViewModel
{
    internal class GameArguments
    {

        internal readonly string PlayerDeckName;
        internal readonly string OpponentDeckName;

        internal GameArguments(string playerDeck, string opponentDeck)
        {
            PlayerDeckName = playerDeck;
            OpponentDeckName = opponentDeck;
        }

    }

    internal class GamePageViewModel: BindableBase
    {

        private GameState _gameState = null;
        internal GameState GameState
        {
            get { return _gameState; }
            set { SetProperty(ref _gameState, value); }
        }

        internal void StartGame(GameArguments gameArguments)
        {
            UpdateGameState(new GameState(gameArguments));
        }

        internal void UpdateGameState(GameState gameState)
        {
            if (GameState != gameState)
            {
                GameState = gameState;
            }
        }

    }

}