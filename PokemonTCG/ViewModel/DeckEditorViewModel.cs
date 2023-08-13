using Microsoft.UI.Xaml.Controls;
using PokemonTCG.DataSources;
using PokemonTCG.Enums;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonTCG.ViewModel
{
    /// <summary>
    /// The view model for the <c>DeckEditorPage</c>.
    /// </summary>
    internal class DeckEditorViewModel : BindableBase
    {

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

        internal void OnNavigatedTo(object deckName, CardItemAdapter cardItemAdapter)
        {
            ISet<string> sets = GetSetsForDeck(deckName);

            foreach (string set in sets)
            {
                _ = LoadCardsItemsForSet(set, cardItemAdapter);
            }

            if(deckName != null)
            {
                AddDeckCards(deckName as string, cardItemAdapter);
            }

        }

        private void AddDeckCards(string deckName, CardItemAdapter cardItemAdapter)
        {
            IImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
            foreach (string id in decks[deckName].CardIds)
            {
                cardItemAdapter.IncrementCardCountForCardWithId(id);
                NumberOfCardsInDeck++;
                Debug.Assert(NumberOfCardsInDeck <= 60 && NumberOfCardsInDeck >= 0);
            }
        }

        private static ISet<string> GetSetsForDeck(object deckName)
        {
            ISet<string> sets = new HashSet<string>();
            if (deckName != null)
            {
                IImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
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

        private static async Task LoadCardsItemsForSet(string setName, CardItemAdapter cardItemAdapter)
        {
            await foreach (CardItem item in CardItemDataSource.GetCardItemsForSet(setName))
            {
                cardItemAdapter.AddCardItem(item);
            }
        }

        internal void ChangeCardItemCount(
            CardItemAdapter cardItemAdapter,
            string cardId,
            int newValue
            )
        {
            int oldValue = cardItemAdapter.GetAllCardItems().Where(item => item.Id == cardId).First().Count;
            int value = newValue;
            int diff = value - oldValue;
            if ((diff + NumberOfCardsInDeck) > PokemonDeck.NUMBER_OF_CARDS_PER_DECK)
            {
                value = (PokemonDeck.NUMBER_OF_CARDS_PER_DECK - NumberOfCardsInDeck);
                diff = value - oldValue;
            }
            
            NumberOfCardsInDeck += diff;
            cardItemAdapter.SetCardCountForCardWithId(cardId, value);
            Debug.Assert(NumberOfCardsInDeck <= PokemonDeck.NUMBER_OF_CARDS_PER_DECK && NumberOfCardsInDeck >= 0);
        }

        internal static bool HasBasicPokemon(CardItemAdapter cardItemAdapter)
        {
            // TODO Fossils count as basic Pokemon
            ImmutableArray<CardItem> cardITems = cardItemAdapter.GetAllCardItems().ToImmutableArray();
            bool hasBasicPokemon = false;
            int i = 0;
            while (i < cardITems.Length && !hasBasicPokemon)
            {
                if(CardDataSource.GetCardById(cardITems[i].Id).Subtypes.Contains(CardSubtype.BASIC))
                {
                    hasBasicPokemon = true;
                };
                i++;
            }
            return hasBasicPokemon;
        }

        /// <summary>
        /// Creates a PokemonDeck from cards that have been selected by the user and are considered in-deck.
        /// </summary>
        /// <returns>Task representing the saving of the deck.</returns>
        internal static async Task SaveDeck(string name, CardItemAdapter cardItemAdapter)
        {
            string[] cards = new string[PokemonDeck.NUMBER_OF_CARDS_PER_DECK];
            foreach (CardItem card in cardItemAdapter.GetAllCardItems())
            {
                for (int i = 0; i < card.Count; i++)
                {
                    cards[i] = (card.Id);
                }
            }
            PokemonDeck deck = new(name, cards.ToImmutableArray());
            await DeckDataSource.SaveDeck(deck);
        }

        internal static double GetCardLimit(CardItem cardItem)
        {
            int limit = cardItem.Limit;
            if (limit == -1)
            {
                limit = 60;
            }
            return limit;
        }

    }

}