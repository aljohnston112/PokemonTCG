using PokemonTCG.DataSources;
using PokemonTCG.Utilities;
using PokemonTCG.ViewModel;
using System;
using System.Collections.Immutable;

namespace PokemonTCG.Models
{
    internal class GameState
    {
        internal readonly bool IsPreGame;
        private readonly PreGameState PreGameState;

        internal readonly bool PlayersTurn;
        internal readonly PlayerState PlayerState;
        internal readonly PlayerState OpponentState;
        internal readonly PokemonCard StadiumCard;

        internal GameState(GameArguments gameArguments)
        {
            IImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
            PokemonDeck playerDeck = decks[gameArguments.PlayerDeckName];
            PokemonDeck opponentDeck = decks[gameArguments.OpponentDeckName];
            PreGameState = new PreGameState(playerDeck, opponentDeck);
            IsPreGame = true;
            PlayerState = PreGameState.GameState.PlayerState;
            OpponentState = PreGameState.GameState.OpponentState;
        }

        internal GameState(
            bool playersTurn,
            PlayerState playerState,
            PlayerState opponentState,
            PokemonCard stadiumCard
        )
        {
            IsPreGame = false;
            PlayersTurn = playersTurn;
            PlayerState = playerState;
            OpponentState = opponentState;
            StadiumCard = stadiumCard;
        }

        internal GameState WithStadiumCard(PokemonCard card)
        {
            // TODO Stadium cards replace any stadium cards in play.You can't play a stadium that is already active. 
            return new GameState(PlayersTurn, PlayerState, OpponentState, card);
        }

        internal GameState WithPlayerState(PlayerState playerState)
        {
            return new GameState(PlayersTurn, playerState, OpponentState, StadiumCard);
        }

        internal GameState WithOpponentState(PlayerState opponentState)
        {
            return new GameState(PlayersTurn, PlayerState, opponentState, StadiumCard);
        }

        // TODO I need to figure out how to design the cards and their effects.
        /*
        internal GameState PlayTrainerCard(PokemonCard pokemonCard)
        {
            // TODO Supporters cannot be played on the first player's first turn.
            
        }
        */

        internal GameState OnUsersFirstTurnSetUp()
        {
            return PreGameState.SetUpOpponent();
        }

        internal bool CurrentPlayersActiveHasEnoughEnergyForAttack(Attack attack)
        {
            bool canUse = true;
            if (PlayersTurn)
            {
                if (!CardUtil.IsEnoughEnergyForAttack(PlayerState.Active.Energy, attack))
                {
                    canUse = false;
                }
                else if (!CardUtil.IsEnoughEnergyForAttack(OpponentState.Active.Energy, attack))
                {
                    canUse = false;
                }
            }
            return canUse;
        }

        internal static GameState AfterMovingFromPlayersHandToActive(GameState gamestate, object[] newActive)
        {
            return gamestate.WithPlayerState(
                gamestate.PlayerState.AfterMovingFromHandToActive(newActive[0] as PokemonCard)
                );
        }

        internal static GameState AfterMovingFromPlayersHandToBench(GameState gameState, object[] benchable)
        {
            return gameState.WithPlayerState(
                gameState.PlayerState.AfterMovingFromHandToBench(benchable[0] as PokemonCard)
                );
        }

        internal GameState WithPlayersTurn(bool playersTurn)
        {
            return new GameState(playersTurn, PlayerState, OpponentState, StadiumCard);
        }

        internal bool CurrentPlayersActiveCanUseAttack(Attack attack)
        {
            bool canUse = CurrentPlayersActiveHasEnoughEnergyForAttack(attack);
            return canUse;
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
