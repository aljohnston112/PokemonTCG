using PokemonTCG.DataSources;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using PokemonTCG.View;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        private readonly CardItemAdapter CardItemAdapter = new();
        public readonly ObservableCollection<CardItemView> CardItemViews;

        private int _numberOfCardsInDeck = 0;
        private string _numberOfCardsInDeckText = "0";

        internal int NumberOfCardsInDeck
        {
            get { return _numberOfCardsInDeck; }
            private set
            {
                NumberOfCardsInDeckText = value.ToString();
                SetProperty(ref _numberOfCardsInDeck, value);
            }
        }

        internal string NumberOfCardsInDeckText
        {
            get { return _numberOfCardsInDeckText; }
            private set { SetProperty(ref _numberOfCardsInDeckText, value); }
        }

        public DeckEditorViewModel()
        {
            CardItemViews = CardItemAdapter.CardItemViews;
        }

        internal async Task OnNavigatedTo(object deckName)
        {
            ISet<string> sets = GetSetsForDeck(deckName);

            foreach (string set in sets)
            {
                ISet<CardItem> cardItems = await LoadCardsItemsForSet(set);
                CardItemAdapter.AddCardItems(cardItems);
            }

            if(deckName != null)
            {
                CountDeckCards(deckName as string);
            }

        }

        private void CountDeckCards(string deckName)
        {
            ImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
            foreach (string id in decks[deckName].CardIds)
            {
                CardItemAdapter.IncrementCardCountForCardWithId(id);
                NumberOfCardsInDeck++;
                Debug.Assert(NumberOfCardsInDeck <= 60 && NumberOfCardsInDeck >= 0);
            }
        }

        private static ISet<string> GetSetsForDeck(object deckName)
        {
            ISet<string> sets = new HashSet<string>();
            if (deckName != null)
            {
                ImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
                foreach (string id in decks[deckName as string].CardIds)
                {
                    PokemonCard card = CardDataSource.GetCardById(id);
                    sets.Add(card.SetId);
                }
            }
            else
            {
                sets.Add("base1");
            }
            return sets;
        }

        private async Task<ISet<CardItem>> LoadCardsItemsForSet(string setName)
        {
            HashSet<CardItem> cardItems = new();
            await foreach (CardItem item in CardItemDataSource.GetCardItemsForSet(setName))
            {
                cardItems.Add(item);
            }
            return cardItems;
        }

        public void ChangeCardItemCount(int oldValue, int newValue, CardItemView cardItemView)
        {
            int value = newValue;
            int diff = value - oldValue;
            if ((diff + NumberOfCardsInDeck) > DeckDataSource.NUMBER_OF_CARDS_PER_DECK)
            {
                value = (DeckDataSource.NUMBER_OF_CARDS_PER_DECK - NumberOfCardsInDeck);
                diff = value - oldValue;
            }
            
            NumberOfCardsInDeck += diff;
            CardItemAdapter.SetCardCountForCardWithId(cardItemView.Id, value);
            Debug.Assert(NumberOfCardsInDeck <= DeckDataSource.NUMBER_OF_CARDS_PER_DECK && NumberOfCardsInDeck >= 0);
        }

        /// <summary>
        /// Creates a PokemonDeck from cards that have been selected by the user and are considered in-deck.
        /// </summary>
        /// <returns>Task representing the saving of the deck.</returns>
        internal async Task CreateDeck(string name)
        {
            Collection<string> cards = new();
            foreach (CardItem card in CardItemAdapter.GetAllCardItems())
            {
                for (int i = 0; i < card.Count; i++)
                {
                    cards.Add(card.Id);
                }
            }
            PokemonDeck deck = new(name, cards);
            await DeckDataSource.SaveDeck(deck);
        }

        public void SearchStringUpdated(string text)
        {
            CardItemAdapter.UpdateSearchString(text);
        }

        internal void OnPokemonCheckBox(bool isChecked)
        {
            CardItemAdapter.IncludePokemon(isChecked);
        }

        internal void OnTrainerCheckBox(bool isChecked)
        {
            CardItemAdapter.IncludeTrainer(isChecked);
        }

        internal void OnEnergyCheckBox(bool isChecked)
        {
            CardItemAdapter.IncludeEnergy(isChecked);
        }

        internal void OnTypeCheckBox(PokemonType type, bool isChecked)
        {
            CardItemAdapter.InludeType(type, isChecked);
        }

        internal void InDeckCheckbox(bool isChecked)
        {
            CardItemAdapter.IncludeOnlyThoseFromDeck(isChecked);
        }

    }

}