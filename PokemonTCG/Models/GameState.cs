using PokemonTCG.DataSources;
using PokemonTCG.ViewModel;
using System.Collections.Immutable;

namespace PokemonTCG.Models
{
    internal class GameState
    {

        private readonly PreGameState PreGameState;

        internal readonly bool IsPreGame;
        internal readonly bool PlayersTurn;
        internal readonly GameFieldState GameFieldState;

        internal GameState(GameArguments gameArguments)
        {
            IImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
            PokemonDeck playerDeck = decks[gameArguments.PlayerDeckName];
            PokemonDeck opponentDeck = decks[gameArguments.OpponentDeckName];
            PreGameState = PreGameState.StartPreGame(playerDeck, opponentDeck);
            IsPreGame = true;
            GameFieldState = PreGameState.GameFieldState;
        }

        internal GameState(bool playersTurn, GameFieldState gameFieldState)
        {
            IsPreGame = false;
            PlayersTurn = playersTurn;
            GameFieldState = gameFieldState;
        }


        internal GameState OnUsersFirstTurnSetUp()
        {
            return PreGameState.SetUpOpponent();
        }

        internal GameState WithPlayerState(PlayerState playerState)
        {
            return new GameState(PlayersTurn, GameFieldState.WithPlayerState(playerState));
        }

        internal static GameState NextTurn(GameState gameState)
        {
            return TurnState.NextTurn(gameState);
        }

    }

}

/*
 * 1. Pick who goes first randomly.
 * 2. Shuffle deck.
 * 3. Draw 7 cards.
 * 4. Check for basic Pokemon. Fossils count as basic Pokemon.
 * 5. If no basic Pokemon, go to 2 after opponent reaches step 6.
 * 6. Select an active basic Pokemon.
 * 7. For each time step 2 was repeated after 5, opponent draws 1 card minus any times they had to repeat step 2 after 5.
 * 8. Put up to 5 Pokemon on the bench.
 * 9. Reveal all Pokemon in play.
 * 10. First turn.
 * 11. Second player's turn.
 * 12. First player's turn. Go to 11 unless the game ends.
 *          The game ends when one player takes all their prizes, 
 *          does not have any Pokemon left in play, 
 *          or has no cards left to draw at the beginning of their turn.
 * Sudden death happens if both players win at the same time, unless one meets two of the conditions and the other does not.
 */
