using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using PokemonTCG.DataSources;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace PokemonTCG.ViewModel
{
    /// <summary>
    /// The view model for the <c>DeckEditorPage</c>.
    /// </summary>
    internal class DeckEditorViewModel : BindableBase
    {
        private readonly Collection<CardItem> _cards = new();
        private CardSifter _sifter = new();
        public readonly ObservableCollection<CardItem> Cards = new();

        private int _totalCount = 0;
        private int TotalCount
        {
            get { return _totalCount; }
            set 
            { 
                TotalCountText = value.ToString();
                SetProperty(ref _totalCount, value); 
            }
        }

        internal int GetTotalCount()
        {
            return _totalCount;
        }

        private string _totalCountText = "0";
        public string TotalCountText
        {
            get { return _totalCountText; }
            set { SetProperty(ref _totalCountText, value); }
        }

        public DeckEditorViewModel() { }

        public async void LoadCards(string deckFile)
        {
            // Get the json file containing all the cards for the base set
            StorageFile file = await FileUtil.getFile(deckFile);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonArray jsonArray = JsonArray.Parse(jsonText);

            // Load the cards
            int i = 0;
            DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            foreach (JsonValue element in jsonArray)
            {
                CardItem card = await CardItemDataSource.GetCard(dispatcherQueue, element, i);
                _cards.Add(card);
                Cards.Add(card);
                i++;
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
            int value = (int)args.NewValue;
            int diff = value - (int)args.OldValue;
            if ((diff + TotalCount) > 60)
            {

                value = (60 - TotalCount);
                diff = value - (int)args.OldValue;
            }
            item.SetCount(value);
            TotalCount += diff;
            Debug.Assert(TotalCount <= 60 && TotalCount >= 0);
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
        internal void PokemonCheckBox(bool isChecked)
        {
            _sifter = _sifter.PokemonUpdate(isChecked);
            SiftCards();
        }

        /// <summary>
        /// Called when there is a change to the trainer checkbox
        /// </summary>
        /// <param name="isChecked">Whether or not the checkbox is checked</param>
        internal void TrainerCheckBox(bool isChecked)
        {
            _sifter = _sifter.TrainerUpdate(isChecked);
            SiftCards();
        }

        /// <summary>
        /// Called when there is a change to the energy checkbox.
        /// </summary>
        /// <param name="isChecked">Whether or not the checkbox is checked</param>
        internal void EnergyCheckBox(bool isChecked)
        {
            _sifter = _sifter.EnergyUpdate(isChecked);
            SiftCards();
        }

        /// <summary>
        /// Called when there is a change to a PokemonType checkbox.
        /// </summary>
        /// <param name="isChecked">Whether or not the checkbox is checked</param>
        internal void TypeChange(PokemonType type, bool isChecked)
        {
            _sifter = _sifter.TypeUpdate(type, isChecked);
            SiftCards();
        }

        /// <summary>
        /// Called when there is a change to the in-deck checkbox.
        /// </summary>
        /// <param name="isChecked">Whether or not the checkbox is checked</param>
        internal void InDeckChanged(bool value)
        {
            _sifter = _sifter.InDeckUpdate(value);
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
            PokemonDeck deck = new PokemonDeck(name, cards);
            await PokemonDeck.SaveDeck(deck);
        }

    }

}
