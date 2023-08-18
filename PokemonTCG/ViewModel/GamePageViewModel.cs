using PokemonTCG.DataSources;
using System.Collections.Immutable;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using PokemonTCG.States;

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
            IImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
            PokemonDeck playerDeck = decks[gameArguments.PlayerDeckName];
            PokemonDeck opponentDeck = decks[gameArguments.OpponentDeckName];
            UpdateGameState(new GameState(playerDeck, opponentDeck));
        }

        internal void OnUsersFirstTurnSetUp()
        {
            // * 9. Reveal all Pokemon in play.
            UpdateGameState(GameState.OnUsersFirstTurnSetUp());
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