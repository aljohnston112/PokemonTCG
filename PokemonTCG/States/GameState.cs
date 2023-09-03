using PokemonTCG.CardModels;
using PokemonTCG.Utilities;

using System.Diagnostics;
using System.Linq;

namespace PokemonTCG.States
{
    internal class GameState
    {
        internal readonly bool IsPreGame;
        internal readonly bool PlayersTurn;
        internal readonly PlayerState PlayerState;
        internal readonly PlayerState OpponentState;
        internal readonly PokemonCard StadiumCard;

        internal GameState(PokemonDeck playerDeck, PokemonDeck opponentDeck)
        {
            IsPreGame = true;
            GameState gameState = PreGameBuilder.StartGame(playerDeck, opponentDeck);
            PlayersTurn = gameState.PlayersTurn;
            PlayerState = gameState.PlayerState;
            OpponentState = gameState.OpponentState;
        }

        internal GameState(
            bool isPreGame,
            bool playersTurn,
            PlayerState playerState,
            PlayerState opponentState,
            PokemonCard stadiumCard
        )
        {
            IsPreGame = isPreGame;
            PlayersTurn = playersTurn;
            PlayerState = playerState;
            OpponentState = opponentState;
            StadiumCard = stadiumCard;
        }

        internal GameState OnUsersFirstTurnSetUp()
        {
            return PreGameBuilder.SetUpOpponent(this);
        }

        internal GameState WithPlayersTurn(bool playersTurn)
        {
            return new GameState(
                isPreGame: IsPreGame,
                playersTurn: playersTurn,
                playerState: PlayerState,
                opponentState: OpponentState,
                stadiumCard: StadiumCard
                );
        }

        internal GameState WithPlayerState(PlayerState playerState)
        {
            return new GameState(
                isPreGame: IsPreGame,
                playersTurn: PlayersTurn,
                playerState: playerState,
                opponentState: OpponentState,
                stadiumCard: StadiumCard
                );
        }

        internal GameState WithOpponentState(PlayerState opponentState)
        {
            return new GameState(
                isPreGame: IsPreGame,
                playersTurn: PlayersTurn,
                playerState: PlayerState,
                opponentState: opponentState,
                stadiumCard: StadiumCard
                );
        }

        internal GameState WithStadiumCard(PokemonCard stadiumCard)
        {
            // TODO Stadium cards replace any stadium cards in play.You can't play a stadium that is already active. 
            return new GameState(
                isPreGame: IsPreGame,
                playersTurn: PlayersTurn,
                playerState: PlayerState,
                opponentState: OpponentState,
                stadiumCard: stadiumCard
                );
        }

        internal bool CurrentPlayersActiveCanUseAttack(Attack attack)
        {
            Debug.Assert(CurrentPlayerState().Active.PokemonCard.Attacks.Contains(attack));
            bool canUse = AttackUtil.IsEnoughEnergyForAttack(CurrentPlayerState().Active.Energy, attack);
            return canUse;
        }

        internal PlayerState CurrentPlayerState()
        {
            PlayerState currentPlayerState;
            if (PlayersTurn)
            {
                currentPlayerState = PlayerState;
            }
            else
            {
                currentPlayerState = OpponentState;
            }
            return currentPlayerState;
        }

        internal PlayerState CurrentNonPlayerState()
        {
            PlayerState CurrentNonPlayerState;
            if (PlayersTurn)
            {
                CurrentNonPlayerState = OpponentState;
            }
            else
            {
                CurrentNonPlayerState = PlayerState;
            }
            return CurrentNonPlayerState;
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