using PokemonTCG.CardModels;
using PokemonTCG.DataSources;
using PokemonTCG.Utilities;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        internal FieldCardActionState(GameState currentGameState, Action<GameState> onGameStateChanged)
        {
            if (currentGameState.PlayersTurn)
            {
                BenchCardActions = GetBenchCardActions(currentGameState, onGameStateChanged);
                ActiveCardActions = GetActiveCardActions(currentGameState, onGameStateChanged);
            }
        }

        private static ImmutableList<CardActionState<PokemonCardState>> GetActiveCardActions(
            GameState currentGameState, 
            Action<GameState> onGameStateChanged
            )
        {
            List<CardActionState<PokemonCardState>> fieldActions = new()
            {
                GetFieldActionsForActive(currentGameState, onGameStateChanged)
            };
            return fieldActions.ToImmutableList();
        }

        private static ImmutableList<CardActionState<PokemonCardState>> GetBenchCardActions(
            GameState currentGameState, 
            Action<GameState> onGameStateChanged
            )
        {
            List<CardActionState<PokemonCardState>> fieldActions = new();
            foreach (PokemonCardState benchCard in currentGameState.PlayerState.Bench)
            {
                fieldActions.Add(GetFieldActionsForBenched(currentGameState, onGameStateChanged, benchCard));
            }
            return fieldActions.ToImmutableList();
        }

        private static CardActionState<PokemonCardState> GetFieldActionsForBenched(
            GameState currentGameState,
            Action<GameState> onGameStateChanged,
            PokemonCardState benchCard
            )
        {
            Dictionary<string, Action> cardActions = new();
            if (!benchCard.FirstTurnInPlay)
            {
                IImmutableList<PokemonCard> evolutionCards = currentGameState.PlayerState.GetEvolutionCardsForCard(benchCard);
                if (evolutionCards.Count > 0)
                {
                    cardActions[EVOLVE_ACTION] = GetEvolveAction(
                        currentGameState,
                        onGameStateChanged,
                        benchCard,
                        evolutionCards,
                        isBench: true
                        );
                }
            }

            foreach (Ability ability in benchCard.PokemonCard.Abilities)
            {
                if (CanUseAbility(currentGameState, benchCard, ability.Name))
                {
                    cardActions[$"{USE_ACTION}{ability.Name}"] = GetAbilityAction(
                        currentGameState,
                        onGameStateChanged,
                        benchCard,
                        ability
                        );
                }
            }

            return new CardActionState<PokemonCardState>(benchCard, cardActions.ToImmutableDictionary());
        }

        private static CardActionState<PokemonCardState> GetFieldActionsForActive(
            GameState currentGameState,
            Action<GameState> onGameStateChanged
            )
        {
            PlayerState playerState = currentGameState.PlayerState;
            Dictionary<string, Action> cardActionsForActive = new();
            PokemonCardState activeCard = currentGameState.PlayerState.Active;

            if (activeCard != null)
            {
                if (!activeCard.FirstTurnInPlay)
                {
                    IImmutableList<PokemonCard> evolutionCardsForActive = playerState.GetEvolutionCardsForCard(activeCard);
                    if (evolutionCardsForActive.Count > 0)
                    {
                        cardActionsForActive[EVOLVE_ACTION] = GetEvolveAction(
                            currentGameState,
                            onGameStateChanged,
                            activeCard,
                            evolutionCardsForActive,
                            isBench: false
                            );
                    }
                }
                if (!currentGameState.IsPreGame)
                {
                    if (activeCard.CanRetreat() && playerState.Bench.Count > 0)
                    {
                        cardActionsForActive[RETREAT_ACTION] = GetRetreatAction(
                            currentGameState,
                            onGameStateChanged,
                            activeCard
                            );
                    }
                    foreach (Ability ability in activeCard.PokemonCard.Abilities)
                    {
                        if (CanUseAbility(currentGameState, activeCard, ability.Name))
                        {
                            cardActionsForActive[$"{USE_ACTION}{ability.Name}"] = GetAbilityAction(
                                currentGameState,
                                onGameStateChanged,
                                activeCard,
                                ability
                                );
                        }
                    }
                    if (!activeCard.FirstTurnInPlay)
                    {
                        foreach (Attack attack in activeCard.PokemonCard.Attacks)
                        {
                            if (CanUseAttack(currentGameState, activeCard, attack))
                            {
                                cardActionsForActive[$"{USE_ACTION}{attack.Name}"] = GetAttackAction(
                                    currentGameState,
                                    onGameStateChanged,
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

        private static Action GetAttackAction(
            GameState currentGameState,
            Action<GameState> onGameStateChanged,
            PokemonCardState activeCard,
            Attack attack
            )
        {
            return new Action(
                () =>
                {
                    MethodInfo method = CardFunctionDataSource.GetPlayerUseFunction(activeCard.PokemonCard.Id, attack.Name);
                    GameState newGameState = (GameState)method.Invoke(null, new object[] { currentGameState, attack });
                    onGameStateChanged(newGameState);
                }
                );
        }

        private static Action GetAbilityAction(
            GameState currentGameState,
            Action<GameState> onGameStateChanged,
            PokemonCardState activeCard,
            Ability ability
            )
        {
            return new Action(
                () =>
                {
                    MethodInfo method = CardFunctionDataSource.GetPlayerUseFunction(activeCard.PokemonCard.Id, ability.Name);
                    GameState newGameState = (GameState)method.Invoke(null, new object[] { currentGameState, activeCard });
                    onGameStateChanged(newGameState);
                }
                );
        }

        private static Action GetRetreatAction(
            GameState currentGameState,
            Action<GameState> onGameStateChanged,
            PokemonCardState activeCard
            )
        {
            return new Action(
                () =>
                {
                    void OnEnergySelected(IImmutableList<PokemonCard> pickedEnergy)
                    {
                        void onCardSelected(IImmutableList<PokemonCardState> pickedBenchCards)
                        {
                            PlayerState newPlayerState = currentGameState.PlayerState.AfterRetreatingActivePokemon(
                                pickedBenchCards[0],
                                pickedEnergy
                                );
                            onGameStateChanged(currentGameState.WithPlayerState(newPlayerState));
                        }
                        WindowUtil.OpenCardPickerPageAndGetSelectedCards(
                            cards: currentGameState.PlayerState.Bench,
                            onCardsSelected: onCardSelected,
                            numberOfCardsToPick: 1
                            );
                    }
                    WindowUtil.OpenCardPickerPageAndGetSelectedCards(
                        cards: activeCard.Energy,
                        onCardsSelected: OnEnergySelected,
                        numberOfCardsToPick: activeCard.PokemonCard.ConvertedRetreatCost);
                }
                );
        }

        private static Action GetEvolveAction(
            GameState currentGameState,
            Action<GameState> onGameStateChanged,
            PokemonCardState benchCard,
            IImmutableList<PokemonCard> evolutionCards,
            bool isBench
            )
        {
            GameState gameState = currentGameState;
            return new Action(
                () =>
                {
                    PlayerState playerState = gameState.PlayerState;
                    void OnCardSelected(IImmutableList<PokemonCard> pickedCards)
                    {
                        PlayerState newPlayerState;
                        PokemonCard pickedCard = pickedCards[0];
                        if (isBench)
                        {
                            newPlayerState = playerState.AfterEvolvingBenchedPokemon(benchCard, pickedCard);
                        }
                        else
                        {
                            newPlayerState = playerState.AfterEvolvingActivePokemon(pickedCard);
                        }
                        onGameStateChanged(gameState.WithPlayerState(newPlayerState));
                    }
                    WindowUtil.OpenCardPickerPageAndGetSelectedCards(
                        cards: evolutionCards,
                        onCardsSelected: OnCardSelected,
                        numberOfCardsToPick: 1
                        );
                }
                );
        }

        private static bool CanUseAbility(
            GameState gameState,
            PokemonCardState card,
            string abilityName
            )
        {
            MethodInfo methodInfo =  CardFunctionDataSource.GetCanUseFunction(card.PokemonCard.Id, abilityName);
            return (bool)methodInfo.Invoke(null, new object[] { gameState, card });
        }


        private static bool CanUseAttack(
            GameState gameState,
            PokemonCardState card,
            Attack attack
            )
        {
            MethodInfo methodInfo = CardFunctionDataSource.GetCanUseFunction(card.PokemonCard.Id, attack.Name);
            return (bool)methodInfo.Invoke(null, new object[] { gameState, attack });
        }

    }

}