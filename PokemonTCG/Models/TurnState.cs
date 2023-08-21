using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.States;
using PokemonTCG.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Windows.ApplicationModel.Store.Preview.InstallControl;

namespace PokemonTCG.Models
{
    internal class TurnState
    {

        internal static GameState NextTurn(GameState gameState)
        {
            GameState newGameState;
            if (gameState.PlayersTurn)
            {
                newGameState = PlayersNextTurn(gameState);
            }
            else
            {
                newGameState = OpponentsNextTurn(gameState);
            }

            return newGameState;
        }

        private static GameState OpponentsNextTurn(GameState gameState)
        {
            PlayerState opponentState = gameState.OpponentState;
            opponentState = opponentState.AfterDrawingCards(1);
            opponentState = AfterPotentialMoveBenchAction(gameState);
            return gameState.WithOpponentState(opponentState);
        }

        private static PlayerState AfterPotentialMoveBenchAction(GameState gameState)
        {
            PlayerState opponentState = gameState.OpponentState;
            PokemonCard bestCardToAddToBench = BestCardIfShouldAddToBench(gameState);
            if (bestCardToAddToBench != null)
            {
                opponentState = MoveBestCardToBench(opponentState);
            }
            return opponentState;
        }

        private static PokemonCard BestCardIfShouldAddToBench(GameState gameState)
        {
            PokemonCard bestHandCard = null;
            if (gameState.OpponentState.HandHasBasicPokemon())
            {
                IList<PokemonCardState> playerCards = gameState.PlayerState.Bench.ToList();
                playerCards.Add(gameState.PlayerState.Active);


                // Check how much many turns it will take for each bench Pokemon to KO
                //     all player cards on field
                int bestNumberOfKOs = -1;
                foreach (PokemonCardState benchCard in gameState.OpponentState.Bench)
                {
                    int numberOfKOs = GetNumberOfCardsPokemonCanKO(
                        benchCard, 
                        CardUtil.GetNumberOfEachEnergy(gameState.OpponentState.Hand), 
                        playerCards
                        );
                    bestNumberOfKOs = Math.Max(bestNumberOfKOs, numberOfKOs);
                }

                // If a hand card can KO more cards than those in the bench, add to bench
                foreach (PokemonCard handCard in gameState.OpponentState.Hand)
                {
                    int numberOfKOs = GetNumberOfCardsPokemonCanKO(
                        handCard,
                        CardUtil.GetNumberOfEachEnergy(gameState.OpponentState.Hand),
                        playerCards
                        );
                    if (numberOfKOs > bestNumberOfKOs)
                    {
                        bestNumberOfKOs = numberOfKOs;
                        bestHandCard = handCard;
                    }
                }

            }
            throw new NotImplementedException();
            return bestHandCard;
        }

        private static int GetNumberOfCardsPokemonCanKO(
            PokemonCard handCard, 
            IImmutableDictionary<PokemonType, int> energy, 
            IList<PokemonCardState> playerCards
            )
        {
            throw new NotImplementedException();
        }

        private static int GetNumberOfCardsPokemonCanKO(
            PokemonCardState benchCard,
            IImmutableDictionary<PokemonType, int> energy,
            IList<PokemonCardState> playerCards
            )
        {
            throw new NotImplementedException();
        }

        private static PlayerState MoveBestCardToBench(PlayerState opponentState)
        {
            throw new NotImplementedException();
        }

        private static GameState PlayersNextTurn(GameState gameState)
        {
            PlayerState playerState = gameState.PlayerState;
            playerState = playerState.AfterDrawingCards(1);
            return gameState
                .WithPlayerState(playerState)
                .WithPlayersTurn(true);
        }

    }

}

/*
 * 1. Draw a card. If you have no cards to draw you lose.
 * 2. Do any of the following in any order.
 *      a. Put any number of basic Pokemon on the bench. 
 *              All 4 cards of a V-UNION can be played only from the discard pile and take up one bench spot.
 *      b. Evolve a Pokemon who has a name that matches the "evolves from" of another card. 
 *         Cannot be done on a Pokemon's first turn in play.
 *                  Level is not part of a card name. 
 *                  Symbols are except δ (Delta Species). 
 *                  Owner is part of the name. Form is part of the name.
 *                  "-Ex" is not the same as "ex".
 *                  Turn ends if the evolution is a mega evolution or a primal reversion Pokemon.
 *              i. Remove attack effects and status condition.
 *              ii. Evolve
 *      c. Attach one energy to any of your in play Pokemon.
 *      d. Play any number of trainers, but only one Supporter and one Stadium per turn. 
 *              Supporters cannot be played on the first player's first turn.
 *              Stadium cards replace any stadium cards in play. You can't play a stadium that is already active.
 *      e. Retreat the active Pokemon once. Remove energy for retreat cost. Asleep or paralyzed Pokemon cannot retreat.
 *              Attack effects and status conditions are cleared from the retreating Pokemon.
 *      f. Use any amount of abilities. 
 *              The active and all benched Pokemon abilities are fair game. 
 *              A player can only use one VSTAR per game.
 *      g. Use "any amount"? of ancient traits. Ancient traits do not count as abilities.
 * 
 * 3. Attack; Attacks that say to put damage tokens on another Pokemon do not have any damage calulations. 
 *         "Up to" mean between 1 and N. 
 *         "Any" mean 0 or more.
 *         The first player cannot attack on the first turn.
 *         Makes sure energy requirments are met.
 *         Sleep, confusion and paralysis replace one another.
 *         A player can only use one GX attack per game
 *      a. Apply your effects and any trainer cards that alter the attack; 
 *         Attack effects go away when any active Pokemon goes to the bench.
 *      b. Check if confusion inflicted. Flip a coin to see if you take 30 damage. 
 *         Card is upside down.
 *      c. Take weakness into account, except for benched Pokemon.
 *      d. Take resistance into account, except for benched Pokemon.
 *      e. Perform attack choices.
 *      f. Do what the attack says.
 *      g. Apply opponent effects and any trainer that alters damage.
 *      h. Apply all other effects.
 *      i. Do damage
 * 
 * 4. If a opponent's Pokemon is knocked out, take a prize. 
 *          Take another if the Pokemon was 
 *              a Pokemon ex,
 *              a tera Pokemon ex,
 *              a Pokemon VSTAR,
 *              a Pokemon V,
 *              a Pokemon GX,
 *              a Pokemon-EX,
 *              a Mega evolution Pokemon,
 *              a primal reversion Pokemon
 *          or Take 2 more if the Pokemon was 
 *              a Pokemon V-UNION,
 *              a Pokemon VMAX,
 *              a TAG TEAM
 * 5. Pokemon checkup
 *      a. Poison. 10 damage.
 *      b. Burn. 20 damage. Flip a coin to see if healed.
 *      c. Sleep. Flip a coin to see if awakened. Card top facing left.
 *      d. Paralysis. Heals after owner's next turn. Card top facing right.
 *      e. Abilitiy and trainer effects.
 *      f. Other Pokemon checkup and between turn effects.
 *      a-d and e-f can be switched, so e-f then a-d.
 *      g. Take prizes for any knocked out Pokemon.
 *      
 * Special rules.
 *      Tera Pokemon ex do not take any damamge while benched.
 *      VSTAR are considered V Pokemon.
 *      V-UNION cards are not basic, stage 1, stage 2, or evolution Pokemon
 *      Individual V-UNION cards do not any attributes other than types, catergory, and name.
 *      V-UNION cards count as Pokemon V.
 *      Prism cards go to the lost zone instead of the dicard pile.
 *      Restored Pokemon are not basic Pokemon, but are considered unevolved.
 */
