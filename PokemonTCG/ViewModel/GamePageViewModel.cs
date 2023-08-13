using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using PokemonTCG.DataSources;
using PokemonTCG.Models;
using PokemonTCG.View;

namespace PokemonTCG.ViewModel
{
    internal class GamePageViewModel
    {
        private GameState GameState;
        private readonly Action<GameState> OnGameStateChanged;
        private readonly GameTemplate GameTemplate = new();

        internal GamePageViewModel(Action<GameState> onGameStateChanged)
        {
            OnGameStateChanged = onGameStateChanged;
        }

        internal void StartGame(GameArguments gameArguments)
        {
            IImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
            PokemonDeck playerDeck = decks[gameArguments.PlayerDeckName];
            PokemonDeck opponentDeck = decks[gameArguments.OpponentDeckName];
            GameState = GameTemplate.SetUpGame(playerDeck, opponentDeck);
            OnGameStateChanged(GameState);
        }

        internal void OnUserSetUp()
        {
            GameState = GameTemplate.SetUpOpponent(GameState);
        }

    }

}
