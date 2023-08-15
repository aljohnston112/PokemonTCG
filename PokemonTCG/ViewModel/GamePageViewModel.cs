using System;
using PokemonTCG.Models;

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
        private readonly Action<GameState> OnGameStateChanged;

        internal GamePageViewModel(Action<GameState> onGameStateChanged)
        {
            OnGameStateChanged = onGameStateChanged;
        }

        internal void StartGame(GameArguments gameArguments)
        {
            OnGameStateChanged(new GameState(gameArguments));
        }

        private void UpdateGameState(GameState gameState)
        {
            OnGameStateChanged(gameState);
        }

    }

}