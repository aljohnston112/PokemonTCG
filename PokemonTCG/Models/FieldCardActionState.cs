using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

using PokemonTCG.CardModels;
using PokemonTCG.States;
using PokemonTCG.Utilities;
using PokemonTCG.View;
using PokemonTCG.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PokemonTCG.Models
{
    internal class FieldCardActionState
    {

        internal const string EVOLVE_ACTION = "Evolve";


        private static ImmutableList<CardActionState<PokemonCardState>> GetFieldActions(
            GamePageViewModel gamePageViewModel
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            List<CardActionState<PokemonCardState>> fieldActions = new();
            foreach (PokemonCardState benchCard in gameState.PlayerState.Bench)
            {
                fieldActions.Add(GetFieldActionsForBenched(gamePageViewModel, benchCard));
            }
            fieldActions.Add(GetFieldActionsForActive(gamePageViewModel));
            return fieldActions.ToImmutableList();
        }

        private static CardActionState<PokemonCardState> GetFieldActionsForBenched(
            GamePageViewModel gamePageViewModel, 
            PokemonCardState benchCard
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            Dictionary<string, TappedEventHandler> cardActions = new();

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
            return new CardActionState<PokemonCardState>(benchCard, cardActions.ToImmutableDictionary());
        }

        private static CardActionState<PokemonCardState> GetFieldActionsForActive(
            GamePageViewModel gamePageViewModel
            )
        {
            GameState gameState = gamePageViewModel.GameState;
            Dictionary<string, TappedEventHandler> cardActionsForActive = new();
            PokemonCardState activeCard = gameState.PlayerState.Active;
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
            return new CardActionState<PokemonCardState>(activeCard, cardActionsForActive.ToImmutableDictionary());
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
                (object sender, TappedRoutedEventArgs e) =>
                {
                    CardPickerPage cardPickerPage = new();
                    Window window = null;
                    void onCardSelected(PokemonCard pickedCard)
                    {
                        PlayerState newPlayerState;
                        if(isBench)
                        {
                            newPlayerState = gameState.PlayerState.AfterEvolvingBenchedPokemon(benchCard, pickedCard);
                        } else
                        {
                            newPlayerState = gameState.PlayerState.AfterEvolvingActivePokemon(pickedCard);
                        }

                        window.Close();
                        gamePageViewModel.UpdateGameState(gameState.WithPlayerState(newPlayerState));
                    }

                    CardPickerPageArgs args = new(
                        evolutionCards,
                        onCardSelected
                        );

                    cardPickerPage.SetArgs(args);
                    window = WindowUtil.OpenPageInNewWindow(cardPickerPage);
                }
                );
        }

    }

}
