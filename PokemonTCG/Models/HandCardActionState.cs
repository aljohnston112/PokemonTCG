using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.States;
using PokemonTCG.Utilities;
using PokemonTCG.View;
using PokemonTCG.ViewModel;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;

namespace PokemonTCG.Models
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

        internal HandCardActionState(GamePageViewModel gamePageViewModel)
        {
            HandActions = GetHandActions(gamePageViewModel);
        }

        private static ImmutableList<CardActionState<PokemonCard>> GetHandActions(
            GamePageViewModel gamePageViewModel
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            IImmutableList<PokemonCard> hand = gameState.PlayerState.Hand;
            List<CardActionState<PokemonCard>> handActions = new();
            if (gameState.IsPreGame || gameState.PlayersTurn)
            {
                foreach (PokemonCard handCard in hand)
                {
                    handActions.Add(GetCardActionsForHandCard(gamePageViewModel, handCard));
                }
            }
            return handActions.ToImmutableList();
        }

        private static CardActionState<PokemonCard> GetCardActionsForHandCard(
            GamePageViewModel gamePageViewModel,
            PokemonCard handCard
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            IImmutableList<PokemonCard> hand = gameState.PlayerState.Hand;
            Dictionary<string, TappedEventHandler> cardActions = new();

            if (handCard.Supertype == CardSupertype.POKéMON)
            {
                if (CardUtil.IsBasicPokemon(handCard))
                {
                    if (gameState.IsPreGame)
                    {
                        if (gameState.PlayerState.Active == null)
                        {
                            cardActions[MAKE_ACTIVE_ACTION] = GetMakeActiveAction(gamePageViewModel, handCard);
                        }
                        if (CardUtil.NumberOfBasicPokemon(hand) > 1 ||
                            (CardUtil.NumberOfBasicPokemon(hand) > 0) && gameState.PlayerState.Active != null)
                        {
                            cardActions[PUT_ON_BENCH_ACTION] = GetPutOnBenchAction(gamePageViewModel, handCard);
                        }
                    }
                    else
                    {
                        if (gameState.PlayerState.Bench.Count < 5)
                        {
                            cardActions[PUT_ON_BENCH_ACTION] = GetPutOnBenchAction(gamePageViewModel, handCard);
                        }
                    }
                }
            }
            else if (handCard.Supertype == CardSupertype.TRAINER)
            {
                if (!gameState.IsPreGame)
                {
                    // TODO only one Supporter and one Stadium per turn. 
                    // TODO Supporters cannot be played on the first player's first turn.
                    // TODO You can't play a stadium that is already active.
                    if (CanUse(gameState, handCard))
                    {
                        cardActions[USE_ACTION] = GetUseAction(gamePageViewModel, handCard);
                    }
                }
            }
            else if (handCard.Supertype == CardSupertype.ENERGY)
            {
                if (!gameState.IsPreGame && !gameState.PlayerState.OncePerTurnActionsState.HasAttachedEnergy)
                {
                    cardActions[ATTACH_TO_POKEMON_ACTION] = GetAttachEnergyToPokemonAction(gamePageViewModel, handCard);
                }
            }
            return new CardActionState<PokemonCard>(handCard, cardActions.ToImmutableDictionary());
        }

        private static TappedEventHandler GetMakeActiveAction(
            GamePageViewModel gamePageViewModel,
            PokemonCard handCard
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            return new TappedEventHandler(
                (object sender, TappedRoutedEventArgs e) =>
                {
                    gamePageViewModel.UpdateGameState(
                        gameState.WithPlayerState(
                            gameState.PlayerState.AfterMovingFromHandToActive(handCard)
                            )
                        );
                }
                );
        }

        private static TappedEventHandler GetPutOnBenchAction(
            GamePageViewModel gamePageViewModel,
            PokemonCard handCard
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            return new TappedEventHandler(
                (object sender, TappedRoutedEventArgs e) =>
                {
                    gamePageViewModel.UpdateGameState(
                        gameState.WithPlayerState(
                            gameState.PlayerState.AfterMovingFromHandToBench(handCard)
                            )
                        );
                }
                );
        }

        private static TappedEventHandler GetUseAction(
            GamePageViewModel gamePageViewModel,
            PokemonCard handCard
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            return new TappedEventHandler(
                (object sender, TappedRoutedEventArgs e) =>
                {
                    MethodInfo method = GetUseFunction(handCard, handCard.Name);
                    gamePageViewModel.UpdateGameState(
                        (GameState)method.Invoke(null, new object[] { gameState })
                    );
                }
                );
        }

        private static TappedEventHandler GetAttachEnergyToPokemonAction(
            GamePageViewModel gamePageViewModel,
            PokemonCard handCard
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            return new TappedEventHandler(
                (object sender, TappedRoutedEventArgs e) =>
                {
                    CardPickerPage cardPickerPage = new();
                    Window window = null;
                    void OnCardSelected(IImmutableList<PokemonCardState> cardStates)
                    {
                        window.Close();
                        PlayerState newPlayerState;
                        PokemonCardState cardState = cardStates[0];
                        if (gameState.PlayerState.Active == cardState)
                        {
                            newPlayerState = gameState.PlayerState.AfterAttachingEnergyToActiveFromHand(handCard.Id);
                        }
                        else
                        {
                            newPlayerState = gameState.PlayerState.AfterAttachingEnergyToBenchedFromHand(
                                cardState,
                                handCard.Id
                                );
                        }
                        gamePageViewModel.UpdateGameState(
                        gameState.WithPlayerState(newPlayerState)
                        );
                    }

                    CardPickerPageArgs<PokemonCardState> args = new(
                        gameState.PlayerState.Bench.Add(gameState.PlayerState.Active),
                        OnCardSelected,
                        1
                        );

                    cardPickerPage.SetArgs(args);
                    window = WindowUtil.OpenPageInNewWindow(cardPickerPage);
                }
                );
        }

        private static MethodInfo GetUseFunction(PokemonCard card, string toUse)
        {

            string namespaceName = "PokemonTCG.Generated";
            string className = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(card.Id).Replace(" ", "_").Replace("-", "_");
            string methodName = toUse.Replace(" ", "_").Replace("-", "_") + "_Use";

            Type type = Type.GetType($"{namespaceName}.{className}");
            MethodInfo methodInfo = type?.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            return methodInfo;
        }

        private static bool CanUse(GameState gameState, PokemonCard card)
        {
            string namespaceName = "PokemonTCG.Generated";
            string className = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(card.Id.Replace(" ", "_").Replace("-", "_"));
            string methodName = card.Name.Replace(" ", "_").Replace("-", "_") + "_CanUse";

            Type type = Type.GetType($"{namespaceName}.{className}");
            MethodInfo methodInfo = type?.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
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