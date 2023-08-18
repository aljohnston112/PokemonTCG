using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using PokemonTCG.CardModels;

namespace PokemonTCG.ViewModel
{

    internal class CardPickerPageArgs
    {

        internal IImmutableList<PokemonCard> Cards;
        internal Action<PokemonCard> OnCardSelected;

        internal CardPickerPageArgs(
            IImmutableList<PokemonCard> cards,
            Action<PokemonCard> onCardSelected
            )
        {
            Cards = cards;
            OnCardSelected = onCardSelected;
        }

    }

    internal class CardPickerViewModel
    {

        private PokemonCard SelectedCard;
        private Action<PokemonCard> OnCardSelected;

        internal ObservableCollection<PokemonCard> Cards = new();

        internal void NewCardSelected(object pokemonCard)
        {
            SelectedCard = pokemonCard as PokemonCard;
        }

        internal void SetArgs(CardPickerPageArgs cardPickerPageArgs)
        {
            OnCardSelected = cardPickerPageArgs.OnCardSelected;
            foreach (PokemonCard card in cardPickerPageArgs.Cards)
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
