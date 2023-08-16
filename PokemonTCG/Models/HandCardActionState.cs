using PokemonTCG.Enums;
using PokemonTCG.Utilities;
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

        internal delegate GameState CardFunction(GameState gameState, params object[] arguments);

        internal readonly ImmutableList<CardActionState> HandActions;

        internal HandCardActionState(GameState gameState)
        {
            HandActions = GetHandActions(gameState, gameState.PlayerState.Hand);
        }

        private static ImmutableList<CardActionState> GetHandActions(
            GameState gameState,
            IImmutableList<PokemonCard> hand
            )
        {
            List<CardActionState> handActions = new();
            foreach (PokemonCard card in hand)
            {
                Dictionary<string, CardFunction> cardActions = new();
                if (card.Supertype == CardSupertype.POKéMON)
                {
                    if (CardUtil.IsBasicPokemon(card))
                    {
                        if (gameState.IsPreGame)
                        {
                            cardActions[MAKE_ACTIVE_ACTION] = new(GameState.AfterMovingFromPlayersHandToActive);
                            if (CardUtil.NumberOfBasicPokemon(hand) > 1)
                            {
                                cardActions[PUT_ON_BENCH_ACTION] = new(GameState.AfterMovingFromPlayersHandToBench);
                            }
                        }
                        else
                        {
                            cardActions[PUT_ON_BENCH_ACTION] = new(GameState.AfterMovingFromPlayersHandToBench);
                        }

                    }
                }
                else if (card.Supertype == CardSupertype.TRAINER)
                {
                    if (!gameState.IsPreGame)
                    {
                        if (CanUse(gameState, card))
                        {
                            cardActions[USE_ACTION] = GetUseFunction(card, card.Name);
                        }
                    }
                }
                handActions.Add(new CardActionState(card, cardActions.ToImmutableDictionary()));
            }
            return handActions.ToImmutableList();
        }

        private static CardFunction GetUseFunction(PokemonCard card, string toUse)
        {

            string namespaceName = "PokemonTCG.Generated";
            string className = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(card.Id).Replace(" ", "_").Replace("-", "_");
            string methodName = toUse.Replace(" ", "_").Replace("-", "_") + "_Use";

            Type type = Type.GetType($"{namespaceName}.{className}");
            MethodInfo methodInfo = type?.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            return (CardFunction)Delegate.CreateDelegate(typeof(CardFunction), methodInfo);
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
