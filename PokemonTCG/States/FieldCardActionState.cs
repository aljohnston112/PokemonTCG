using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using PokemonTCG.View;
using PokemonTCG.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;

namespace PokemonTCG.States
{
    internal class FieldCardActionState
    {

        internal const string EVOLVE_ACTION = "Evolve";
        internal const string RETREAT_ACTION = "Retreat";
        internal const string USE_ACTION = "Use ";

        internal readonly ImmutableList<CardActionState<PokemonCardState>> BenchCardActions;
        internal readonly ImmutableList<CardActionState<PokemonCardState>> ActiveCardActions;

        internal FieldCardActionState(GamePageViewModel gamePageViewModel)
        {
            if (gamePageViewModel.GameState.PlayersTurn)
            {
                BenchCardActions = GetBenchCardActions(gamePageViewModel);
                ActiveCardActions = GetActiveCardActions(gamePageViewModel);
            }
        }

        private static ImmutableList<CardActionState<PokemonCardState>> GetActiveCardActions(
            GamePageViewModel gamePageViewModel
            )
        {
            List<CardActionState<PokemonCardState>> fieldActions = new()
            {
                GetFieldActionsForActive(gamePageViewModel)
            };
            return fieldActions.ToImmutableList();
        }

        private static ImmutableList<CardActionState<PokemonCardState>> GetBenchCardActions(
            GamePageViewModel gamePageViewModel
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            List<CardActionState<PokemonCardState>> fieldActions = new();
            foreach (PokemonCardState benchCard in gameState.PlayerState.Bench)
            {
                fieldActions.Add(GetFieldActionsForBenched(gamePageViewModel, benchCard));
            }
            return fieldActions.ToImmutableList();
        }

        private static CardActionState<PokemonCardState> GetFieldActionsForBenched(
            GamePageViewModel gamePageViewModel,
            PokemonCardState benchCard
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            Dictionary<string, TappedEventHandler> cardActions = new();

            if (!benchCard.FirstTurnInPlay)
            {
                IImmutableList<PokemonCard> evolutionCards = gameState.PlayerState.GetEvolutionCardsForCard(benchCard);
                if (evolutionCards.Count > 0)
                {
                    cardActions[EVOLVE_ACTION] = GetEvolveAction(
                        gamePageViewModel,
                        gameState,
                        benchCard,
                        evolutionCards,
                        isBench: true
                        );
                }
            }

            foreach (Ability ability in benchCard.PokemonCard.Abilities)
            {
                if (CanUseAbility(gameState, benchCard, ability.Name))
                {
                    cardActions[$"{USE_ACTION}{ability.Name}"] = GetAbilityAction(
                        gamePageViewModel,
                        gameState,
                        benchCard,
                        ability
                        );
                }
            }

            return new CardActionState<PokemonCardState>(benchCard, cardActions.ToImmutableDictionary());
        }

        private static CardActionState<PokemonCardState> GetFieldActionsForActive(
            GamePageViewModel gamePageViewModel
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            Dictionary<string, TappedEventHandler> cardActionsForActive = new();
            PokemonCardState activeCard = gameState.PlayerState.Active;

            if (activeCard != null)
            {
                if (!activeCard.FirstTurnInPlay)
                {
                    IImmutableList<PokemonCard> evolutionCardsForActive = gameState.PlayerState.GetEvolutionCardsForCard(activeCard);
                    if (evolutionCardsForActive.Count > 0)
                    {
                        cardActionsForActive[EVOLVE_ACTION] = GetEvolveAction(
                            gamePageViewModel,
                            gameState,
                            activeCard,
                            evolutionCardsForActive,
                            isBench: false
                            );
                    }
                }
                if (!gameState.IsPreGame )
                {
                    if (activeCard.CanRetreat() && gameState.PlayerState.Bench.Count > 0)
                    {
                        cardActionsForActive[RETREAT_ACTION] = GetRetreatAction(
                            gamePageViewModel,
                            gameState,
                            activeCard
                            );
                    }
                    foreach(Ability ability in activeCard.PokemonCard.Abilities)
                    {
                        if (CanUseAbility(gameState, activeCard, ability.Name))
                        {
                            cardActionsForActive[$"{USE_ACTION}{ability.Name}"] = GetAbilityAction(
                                gamePageViewModel,
                                gameState,
                                activeCard,
                                ability
                                );
                        }
                    }
                    if (!activeCard.FirstTurnInPlay)
                    {
                        foreach (Attack attack in activeCard.PokemonCard.Attacks)
                        {
                            if (CanUseAttack(gameState, activeCard, attack))
                            {
                                cardActionsForActive[$"{USE_ACTION}{attack.Name}"] = GetAttackAction(
                                    gamePageViewModel,
                                    gameState,
                                    activeCard,
                                    attack
                                    );
                            }
                        }
                    }
                }
                

            }

            return new CardActionState<PokemonCardState>(activeCard, cardActionsForActive.ToImmutableDictionary());
        }

        private static TappedEventHandler GetAttackAction(
            GamePageViewModel gamePageViewModel, 
            GameState gameState, 
            PokemonCardState activeCard, 
            Attack attack
            )
        {
            return new TappedEventHandler(
                (sender, e) =>
                {
                    MethodInfo method = GetUseFunction(activeCard.PokemonCard, attack.Name);
                    gamePageViewModel.UpdateGameState(
                        (GameState)method.Invoke(null, new object[] { gameState, attack })
                    );
                }
                );
        }

        private static TappedEventHandler GetAbilityAction(
            GamePageViewModel gamePageViewModel,
            GameState gameState,
            PokemonCardState activeCard,
            Ability ability
            )
        {
            return new TappedEventHandler(
                (sender, e) =>
                {
                    MethodInfo method = GetUseFunction(activeCard.PokemonCard, ability.Name);
                    gamePageViewModel.UpdateGameState(
                        (GameState)method.Invoke(null, new object[] { gameState, activeCard })
                    );
                }
                );
        }

        private static TappedEventHandler GetRetreatAction(
            GamePageViewModel gamePageViewModel,
            GameState gameState,
            PokemonCardState activeCard
            )
        {
            return new TappedEventHandler(
                (sender, e) =>
                {
                    CardPickerPage energyCardPickerPage = new();
                    Window energyPickerWindow = null;
                    void OnEnergySelected(IImmutableList<PokemonCard> pickedEnergy)
                    {

                        energyPickerWindow.Close();

                        CardPickerPage benchCardPickerPage = new();
                        Window benchPickerWindow = null;
                        void onCardSelected(IImmutableList<PokemonCardState> pickedBenchCards)
                        {
                            PlayerState newPlayerState;
                            newPlayerState = gameState.PlayerState.AfterRetreatingActivePokemon(
                                pickedBenchCards[0],
                                pickedEnergy
                                );
                            benchPickerWindow.Close();
                            gamePageViewModel.UpdateGameState(gameState.WithPlayerState(newPlayerState));
                        }

                        CardPickerPageArgs<PokemonCardState> args = new(
                            gameState.PlayerState.Bench,
                            onCardSelected,
                            1
                            );

                        benchCardPickerPage.SetArgs(args);
                        benchPickerWindow = WindowUtil.OpenPageInNewWindow(benchCardPickerPage);
                    }
                    CardPickerPageArgs<PokemonCard> energyArgs = new(
                            activeCard.Energy,
                            OnEnergySelected,
                            activeCard.PokemonCard.ConvertedRetreatCost
                            );

                    energyCardPickerPage.SetArgs(energyArgs);
                    energyPickerWindow = WindowUtil.OpenPageInNewWindow(energyCardPickerPage);
                }
                );
        }

        private static TappedEventHandler GetEvolveAction(
            GamePageViewModel gamePageViewModel,
            GameState gameState,
            PokemonCardState benchCard,
            IImmutableList<PokemonCard> evolutionCards,
            bool isBench
            )
        {
            return new TappedEventHandler(
                (sender, e) =>
                {
                    CardPickerPage cardPickerPage = new();
                    Window window = null;
                    void OnCardSelected(IImmutableList<PokemonCard> pickedCards)
                    {
                        PlayerState newPlayerState;
                        PokemonCard pickedCard = pickedCards[0];
                        if (isBench)
                        {
                            newPlayerState = gameState.PlayerState.AfterEvolvingBenchedPokemon(benchCard, pickedCard);
                        }
                        else
                        {
                            newPlayerState = gameState.PlayerState.AfterEvolvingActivePokemon(pickedCard);
                        }

                        window.Close();
                        gamePageViewModel.UpdateGameState(gameState.WithPlayerState(newPlayerState));
                    }

                    CardPickerPageArgs<PokemonCard> args = new(
                        evolutionCards,
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

        private static bool CanUseAbility(
            GameState gameState, 
            PokemonCardState card, 
            string abilityName
            )
        {
            string namespaceName = "PokemonTCG.Generated";
            string className = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(card.PokemonCard.Id.Replace(" ", "_").Replace("-", "_"));
            string methodName = abilityName.Replace(" ", "_").Replace("-", "_") + "_CanUse";

            Type type = Type.GetType($"{namespaceName}.{className}");
            MethodInfo methodInfo = type?.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            return (bool)methodInfo.Invoke(null, new object[] { gameState, card });
        }


        private static bool CanUseAttack(
            GameState gameState,
            PokemonCardState card,
            Attack attack
            )
        {
            string namespaceName = "PokemonTCG.Generated";
            string className = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(card.PokemonCard.Id.Replace(" ", "_").Replace("-", "_"));
            string methodName = attack.Name.Replace(" ", "_").Replace("-", "_") + "_CanUse";

            Type type = Type.GetType($"{namespaceName}.{className}");
            MethodInfo methodInfo = type?.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            return (bool)methodInfo.Invoke(null, new object[] { gameState, attack });
        }

    }

}
