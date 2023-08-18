using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using PokemonTCG.Models;

namespace PokemonTCG.ViewModel
{

    internal class CardStatePickerPageArgs
    {

        internal IImmutableList<PokemonCardState> Cards;
        internal Action<PokemonCardState> OnCardSelected;

        internal CardStatePickerPageArgs(
            IImmutableList<PokemonCardState> cards,
            Action<PokemonCardState> onCardSelected
            )
        {
            Cards = cards;
            OnCardSelected = onCardSelected;
        }

    }
    internal class CardStatePickerViewModel
    {
        private PokemonCardState SelectedCard;
        private Action<PokemonCardState> OnCardSelected;

        internal ObservableCollection<PokemonCardState> Cards = new();

        internal void NewCardSelected(object pokemonCard)
        {
            SelectedCard = pokemonCard as PokemonCardState;
        }

        internal void SetArgs(CardStatePickerPageArgs cardPickerPageArgs)
        {
            OnCardSelected = cardPickerPageArgs.OnCardSelected;
            foreach (PokemonCardState card in cardPickerPageArgs.Cards)
            {
                Cards.Add(card);
            }
        }

        internal void SubmitSelected()
        {
            OnCardSelected?.Invoke(SelectedCard);
        }

    }

}
