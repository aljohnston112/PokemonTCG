using System;
using System.Collections.Immutable;
using PokemonTCG.DataSources;
using PokemonTCG.Models;
using PokemonTCG.View;

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

    internal class GamePageViewModel
    {
        private readonly GameTemplate GameTemplate = new();

        private readonly Action<GameState> OnGameStateChanged;
        private GameState GameState;

        internal GamePageViewModel(Action<GameState> onGameStateChanged)
        {
            OnGameStateChanged = onGameStateChanged;
        }

        internal void StartGame(GameArguments gameArguments)
        {
            IImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
            PokemonDeck playerDeck = decks[gameArguments.PlayerDeckName];
            PokemonDeck opponentDeck = decks[gameArguments.OpponentDeckName];
            UpdateGameState(GameTemplate.SetUpGame(playerDeck, opponentDeck));
        }

        private void UpdateGameState(GameState gameState)
        {
            GameState = gameState;
            OnGameStateChanged(GameState);
        }

        internal void OnUserSetUp()
        {
            GameState = GameTemplate.SetUpOpponent(GameState);
        }

    }

}