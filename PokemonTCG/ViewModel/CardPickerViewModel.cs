using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.Utilities;

namespace PokemonTCG.ViewModel
{

    internal class CardPickerPageArgs<U>
    {

        internal IImmutableList<U> Cards;
        internal Action<IImmutableList<U>> OnCardSelected;
        internal int NumberToPick;

        internal CardPickerPageArgs(
            IImmutableList<U> cards,
            Action<IImmutableList<U>> onCardSelected,
            int numberToPick
            )
        {
            Cards = cards;
            OnCardSelected = onCardSelected;
            NumberToPick = numberToPick;
        }

    }

    internal class CardPickerViewModel<T> : BindableBase
    {
        private Action<IImmutableList<T>> OnCardSelected;
        internal int NumberOfCards;

        private List<T> SelectedCards = new();
        private List<T> _cards = new();
        internal ObservableCollection<PokemonCard> Cards = new();

        private bool _canSubmit;
        internal bool CanSubmit
        {
            get { return _canSubmit; }
            set { SetProperty(ref _canSubmit, value); }
        }

        internal void NewCardSelected(int index)
        {
            SelectedCards.Clear();
            SelectedCards.Add((T)_cards[index]);
            if (SelectedCards.Count == NumberOfCards)
            {
                CanSubmit = true;
            }
        }

        internal void SetArgs<V>(CardPickerPageArgs<V> cardPickerPageArgs)
        {
            NumberOfCards = cardPickerPageArgs.NumberToPick;
            Action<IImmutableList<T>> wrappedAction = list =>
            {
                var convertedList = list.Cast<V>().ToList();
                cardPickerPageArgs.OnCardSelected(convertedList.ToImmutableList());
            };
            OnCardSelected = wrappedAction;
            foreach (V card in cardPickerPageArgs.Cards)
            {
                _cards.Add((T)(object)card);
                if(typeof(V) == typeof(PokemonCardState))
                {
                    Cards.Add((card as PokemonCardState).PokemonCard);
                } else
                {
                    Debug.Assert(typeof(V) == typeof(PokemonCard));
                    Cards.Add(card as PokemonCard);
                }
            }
        }

        internal void SubmitSelection()
        {
            OnCardSelected?.Invoke(SelectedCards.ToImmutableList());
        }

        internal void NewCardsSelected(IList<object> selectedItems)
        {
            SelectedCards.Clear();
            foreach (PokemonCard selectedCard in selectedItems.Cast<PokemonCard>())
            {
                // TODO may select the wrong card if there are two states with the same card.
                SelectedCards.Add(_cards[Cards.IndexOf(selectedCard)]);
            }
            if (SelectedCards.Count == NumberOfCards)
            {
                CanSubmit = true;
            }
        }
    }

}
