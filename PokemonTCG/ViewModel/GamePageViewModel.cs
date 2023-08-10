using System.Collections.Generic;
using System.Threading.Tasks;
using PokemonTCG.DataSources;
using PokemonTCG.Models;
using PokemonTCG.View;

namespace PokemonTCG.ViewModel
{
    internal class GamePageViewModel
    {

        internal interface IGameCallbacks
        {
            void OnReadyForUSerToSetUp();
            void OnGameStateChanged(GameState gameState);
        }

        private GameState GameState;
        private IGameCallbacks Callbacks;

        internal async Task StartGame(GameArguments gameArguments, IGameCallbacks callbacks)
        { 
            string playerDeckName = gameArguments.PlayerDeck;
            string opponentDeckName = gameArguments.OpponentDeck;

            await DeckDataSource.LoadDecks();
            IDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();

            PokemonDeck playerDeck = decks[playerDeckName];
            PokemonDeck opponentDeck = decks[opponentDeckName];
            GameState = new(playerDeck, opponentDeck);
            GameState = GameState.SetUpOpponent();
            Callbacks = callbacks;
            Callbacks.OnGameStateChanged(GameState);
            Callbacks.OnReadyForUSerToSetUp();
        }

    }

}
