using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PokemonTCG.DataSources;
using PokemonTCG.Models;
using PokemonTCG.View;

namespace PokemonTCG.ViewModel
{
    internal class GamePageViewModel
    {

        private GameState GameState;

        private Action<GameState> OnGameStateChanged;

        public GamePageViewModel(Action<GameState> onGameStateChanged)
        {
            OnGameStateChanged = onGameStateChanged;
        }

        internal async Task StartGame(GameArguments gameArguments)
        { 
            string playerDeckName = gameArguments.PlayerDeck;
            string opponentDeckName = gameArguments.OpponentDeck;

            await DeckDataSource.LoadDecks();
            IDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();

            PokemonDeck playerDeck = decks[playerDeckName];
            PokemonDeck opponentDeck = decks[opponentDeckName];
            GameState = new(playerDeck, opponentDeck);
            OnGameStateChanged(GameState);
        }

        internal void OnUserSetUp()
        {
            GameState = GameState.SetUpOpponent();
        }

    }

}
