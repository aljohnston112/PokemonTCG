using Microsoft.UI.Xaml.Controls;
using PokemonTCG.DataSources;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PokemonTCG.ViewModel
{
    /// <summary>
    /// The view model for the <c>DeckEditorPage</c>.
    /// </summary>
    internal class DeckEditorViewModel : BindableBase
    {
        private readonly List<CardItem> _cards = new();
        private CardSifter _sifter = new();
        public readonly ObservableCollection<CardItem> Cards = new();

        private int _numberOfCardsInDeck = 0;
        private string _NumberOfCardsInDeckText = "0";

        internal int NumberOfCardsInDeck
        {
            get { return _numberOfCardsInDeck; }
            private set
            {
                TotalCountText = value.ToString();
                SetProperty(ref _numberOfCardsInDeck, value);
            }
        }

        public string TotalCountText
        {
            get { return _NumberOfCardsInDeckText; }
            set { SetProperty(ref _NumberOfCardsInDeckText, value); }
        }

        public DeckEditorViewModel() { }

        public async void LoadCardsItemsForSet(string deckFile)
        {
            await foreach (CardItem item in CardItemDataSource.GetCardItemsForSet(deckFile))
            {
                _cards.Add(item);
                Cards.Insert(Math.Min(item.Number - 1, Cards.Count), item);
            }
        }

        /// <summary>
        /// Increments item's Count if the total count is going to be 60 or less.
        /// </summary>
        /// <param name="args">The NumberBoxValueChangedEventArgs from the event handler.</param>
        /// <param name="item">The item that corresponds to the NumberBoxValueChangedEvent.
        ///                    It's Count will be updated by the args unless it will cause the total count to be over 60</param>
        /// <returns>true if the item's Count was updated, or the total count would have gone over 60, else false</returns>
        internal void ChangeCount(NumberBoxValueChangedEventArgs args, CardItem item)
        {
            int value = Math.Max((int)args.NewValue, 0);

            int diff = value - (int)args.OldValue;
            if ((diff + NumberOfCardsInDeck) > 60)
            {
                value = (60 - NumberOfCardsInDeck);
                diff = value - (int)args.OldValue;
            }
            item.SetCount(value);
            NumberOfCardsInDeck += diff;
            Debug.Assert(NumberOfCardsInDeck <= 60 && NumberOfCardsInDeck >= 0);
        }

        /// <summary>
        /// Updates the cards to match the sifting criteria.
        /// </summary>
        private void SiftCards()
        {
            Collection<CardItem> cards = _sifter.Sift(_cards);
            Cards.Clear();
            foreach (CardItem card in cards)
            {
                Cards.Add(card);
            }
        }

        /// <summary>
        /// Sifts cards by checking if some text is in the name of the card.
        /// The search is not case sensitive.
        /// </summary>
        /// <param name="text">The text to search the name of the card for</param>
        public void SearchStringUpdated(string text)
        {
            _sifter = _sifter.NewString(text);
            SiftCards();
        }

        /// <summary>
        /// Called when there is a change to the pokmeon checkbox
        /// </summary>
        /// <param name="isChecked">Whether or not the checkbox is checked</param>
        internal void OnPokemonCheckBox(bool isChecked)
        {
            _sifter = _sifter.IncludePokemon(isChecked);
            SiftCards();
        }

        /// <summary>
        /// Called when there is a change to the trainer checkbox
        /// </summary>
        /// <param name="isChecked">Whether or not the checkbox is checked</param>
        internal void OnTrainerCheckBox(bool isChecked)
        {
            _sifter = _sifter.IncludeTrainer(isChecked);
            SiftCards();
        }

        /// <summary>
        /// Called when there is a change to the energy checkbox.
        /// </summary>
        /// <param name="isChecked">Whether or not the checkbox is checked</param>
        internal void OnEnergyCheckBox(bool isChecked)
        {
            _sifter = _sifter.IncludeEnergy(isChecked);
            SiftCards();
        }

        /// <summary>
        /// Called when there is a change to a PokemonType checkbox.
        /// </summary>
        /// <param name="isChecked">Whether or not the checkbox is checked</param>
        internal void TypeChange(PokemonType type, bool isChecked)
        {
            _sifter = _sifter.InludeType(type, isChecked);
            SiftCards();
        }

        /// <summary>
        /// Called when there is a change to the in-deck checkbox.
        /// </summary>
        /// <param name="isChecked">Whether or not the checkbox is checked</param>
        internal void InDeckChanged(bool value)
        {
            _sifter = _sifter.IncludeOnlyThoseFromDeck(value);
            SiftCards();
        }

        /// <summary>
        /// Creates a PokemonDeck from cards that have been selected by the user and are considered in-deck.
        /// </summary>
        /// <returns></returns>
        internal async Task CreateDeck(string name)
        {
            Collection<string> cards = new();
            foreach (CardItem card in _cards)
            {
                if (card.GetCount() > 0)
                {
                    for (int i = 0; i < card.GetCount(); i++)
                    {
                        cards.Add(card.Id);
                    }
                }
            }
            PokemonDeck deck = new(name, cards);
            await DeckDataSource.SaveDeck(deck);
        }

    }

}
