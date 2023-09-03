using PokemonTCG.CardModels;
using PokemonTCG.DataSources;
using PokemonTCG.Enums;
using PokemonTCG.Utilities;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace PokemonTCG.States
{

    /// <summary>
    /// To be used for card context actions in the game page and the hand page.
    /// </summary>
    class HandCardActionState
    {
        internal const string MAKE_ACTIVE_ACTION = "Make active";
        internal const string PUT_ON_BENCH_ACTION = "Put on bench";
        internal const string USE_ACTION = "Use";
        internal const string ATTACH_TO_POKEMON_ACTION = "Attach to Pokemon";

        internal readonly ImmutableList<CardActionState<PokemonCard>> HandActions;

        internal HandCardActionState(GameState currentGameState, Action<GameState> onGameStateChanged)
        {
            HandActions = GetHandActions(currentGameState, onGameStateChanged);
        }

        private static ImmutableList<CardActionState<PokemonCard>> GetHandActions(
            GameState currentGameState, 
            Action<GameState> onGameStateChanged
            )
        {
            IImmutableList<PokemonCard> hand = currentGameState.PlayerState.Hand;
            List<CardActionState<PokemonCard>> handActions = new();
            if (currentGameState.IsPreGame || currentGameState.PlayersTurn)
            {
                foreach (PokemonCard handCard in hand)
                {
                    handActions.Add(GetCardActionsForHandCard(currentGameState, onGameStateChanged, handCard));
                }
            }
            return handActions.ToImmutableList();
        }

        private static CardActionState<PokemonCard> GetCardActionsForHandCard(
            GameState currentGameState, 
            Action<GameState> onGameStateChanged,
            PokemonCard handCard
            )
        {
            IImmutableList<PokemonCard> hand = currentGameState.PlayerState.Hand;
            Dictionary<string, Action> cardActions = new();

            if (handCard.Supertype == CardSupertype.POKéMON)
            {
                if (CardUtil.IsBasicPokemon(handCard))
                {
                    if (currentGameState.IsPreGame)
                    {
                        if (currentGameState.PlayerState.Active == null)
                        {
                            cardActions[MAKE_ACTIVE_ACTION] = GetMakeActiveAction(currentGameState, onGameStateChanged, handCard);
                        }
                        if (CardUtil.NumberOfBasicPokemon(hand) > 1 ||
                            CardUtil.NumberOfBasicPokemon(hand) > 0 && currentGameState.PlayerState.Active != null)
                        {
                            cardActions[PUT_ON_BENCH_ACTION] = GetPutOnBenchAction(currentGameState, onGameStateChanged, handCard);
                        }
                    }
                    else
                    {
                        if (currentGameState.PlayerState.Bench.Count < 5)
                        {
                            cardActions[PUT_ON_BENCH_ACTION] = GetPutOnBenchAction(
                                currentGameState, 
                                onGameStateChanged, 
                                handCard
                                );
                        }
                    }
                }
            }
            else if (handCard.Supertype == CardSupertype.TRAINER)
            {
                if (!currentGameState.IsPreGame)
                {
                    // TODO only one Supporter and one Stadium per turn. 
                    // TODO Supporters cannot be played on the first player's first turn.
                    // TODO You can't play a stadium that is already active.
                    if (CanUseCard(currentGameState, handCard))
                    {
                        cardActions[USE_ACTION] = GetUseAction(currentGameState, onGameStateChanged, handCard);
                    }
                }
            }
            else if (handCard.Supertype == CardSupertype.ENERGY)
            {
                if (!currentGameState.IsPreGame && !currentGameState.PlayerState.OncePerTurnActionsState.HasAttachedEnergy)
                {
                    cardActions[ATTACH_TO_POKEMON_ACTION] = GetAttachEnergyToPokemonAction(
                        currentGameState, 
                        onGameStateChanged, 
                        handCard
                        );
                }
            }
            return new CardActionState<PokemonCard>(handCard, cardActions.ToImmutableDictionary());
        }

        private static Action GetMakeActiveAction(
            GameState currentGameState,
            Action<GameState> onGameStateChanged,
            PokemonCard handCard
            )
        {
            return new Action(
                () =>
                {
                    onGameStateChanged(
                        currentGameState.WithPlayerState(
                            currentGameState.PlayerState.AfterMovingFromHandToActive(handCard)
                            )
                        );
                }
                );
        }

        private static Action GetPutOnBenchAction(
            GameState currentGameState,
            Action<GameState> onGameStateChanged,
            PokemonCard handCard
            )
        {
            return new Action(
                () =>
                {
                    onGameStateChanged(
                        currentGameState.WithPlayerState(
                            currentGameState.PlayerState.AfterMovingFromHandToBench(handCard)
                            )
                        );
                }
                );
        }

        private static Action GetUseAction(
            GameState currentGameState,
            Action<GameState> onGameStateChanged,
            PokemonCard handCard
            )
        {
            return new Action(
                () =>
                {
                    MethodInfo method = GetUseFunction(handCard, handCard.Name);
                    onGameStateChanged(
                        (GameState)method.Invoke(null, new object[] { currentGameState })
                    );
                }
                );
        }

        private static Action GetAttachEnergyToPokemonAction(
            GameState currentGameState,
            Action<GameState> onGameStateChanged,
            PokemonCard handCard
            )
        {
            GameState gameState = currentGameState;
            return new Action(
                () =>
                {
                    void OnCardSelected(IImmutableList<PokemonCardState> cardStates)
                    {
                        PlayerState newPlayerState;
                        PokemonCardState cardState = cardStates[0];
                        if (gameState.PlayerState.Active == cardState)
                        {
                            newPlayerState = gameState.PlayerState.AfterAttachingEnergyToActiveFromHand(CardUtil.GetEnergyType(handCard));
                        }
                        else
                        {
                            newPlayerState = gameState.PlayerState.AfterAttachingEnergyToBenchedFromHand(
                                cardState,
                                CardUtil.GetEnergyType(handCard)
                                );
                        }
                        onGameStateChanged(
                        gameState.WithPlayerState(newPlayerState)
                        );
                    }
                    WindowUtil.OpenCardPickerPageAndGetSelectedCards(
                        cards: gameState.PlayerState.Bench.Add(gameState.PlayerState.Active),
                        onCardsSelected: OnCardSelected,
                        numberOfCardsToPick: 1
                        );
                }
                );
        }

        private static MethodInfo GetUseFunction(PokemonCard card, string toUse)
        {
            MethodInfo methodInfo = CardFunctionDataSource.GetPlayerUseFunction(card.Id, toUse);
            return methodInfo;
        }

        private static bool CanUseCard(GameState gameState, PokemonCard card)
        {
            MethodInfo methodInfo = CardFunctionDataSource.GetCanUseFunction(card.Id, card.Name);
            return (bool)methodInfo.Invoke(null, new object[] { gameState });
        }

    }

}

/*
 * Hand Pokemon
 *      Put on bench
 *      Make Active
 * 
 * Active Pokemon
 *      Use ability
 *      Attack
 *      Evolve
 *      Retreat
 *      Attach energy
 * 
 * Bench Pokemon
 *      Use ability
 *      Evolve
 *      Make active
 *      Attach energy
 *      
 * Trainer cards
 *      Use
 */


/* 
 * Pokemon
 *      Use ability - If ability condition is met
 *          Bench
 *          Active
 *      Attack - If energy condition is met and attack condition is met
 *          Active
 *      Put on bench - "If room on bench and is a basic Pokemon" or "All 4 cards of a V-UNION (from the discard pile)".
 *          Hand
 *      Evolve - If evolution is available in hand and evolves from the Pokemon to be evolved
 *          Active
 *          Bench
 *      Retreat - If retreat cost is met
 *          Active
 *      Make active - If start of game or active was knocked out
 *          Hand
 *          Bench
 *      Attach energy - If not done
 *          Active
 *          Bench
 *          
 * Trainer - If trainer condition is met
 *      Use
 */