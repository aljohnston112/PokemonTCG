using PokemonTCG.CardModels;
using PokemonTCG.DataSources;
using PokemonTCG.Enums;
using PokemonTCG.States;
using PokemonTCG.Utilities;

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

        internal async Task OnNavigatedTo(object deckName, CardItemAdapter cardItemAdapter)
        {
            ISet<string> deckSets = await GetSetsForDeck(deckName);

            foreach (string deckSet in deckSets)
            {
                await LoadCardsItemsForSet(deckSet, cardItemAdapter);
            }

            if(deckName != null)
            {
                AddDeckCards(deckName as string, cardItemAdapter);
            }

        }

        private static async Task<ISet<string>> GetSetsForDeck(object deckName)
        {
            ISet<string> sets = new HashSet<string>();
            if (deckName != null)
            {
                IImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
                foreach (string id in decks[deckName as string].CardIds)
                {
                    PokemonCard card = await CardDataSource.GetCardById(id);
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
            IImmutableList<CardItem> cardItems = await CardItemDataSource.GetCardItemsForSet(setName);
            foreach (CardItem item in cardItems)
            {
                cardItemAdapter.AddCardItem(item);
            }
        }

        private void AddDeckCards(string deckName, CardItemAdapter cardItemAdapter)
        {
            IImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
            foreach (string cardId in decks[deckName].CardIds)
            {
                cardItemAdapter.IncrementCardCountForCardWithId(cardId);
                NumberOfCardsInDeck++;
                Debug.Assert(NumberOfCardsInDeck <= 60 && NumberOfCardsInDeck >= 0);
            }
        }

        internal void ChangeCardItemCount(
            CardItemAdapter cardItemAdapter,
            string cardId,
            int newValue
            )
        {
            IImmutableList<CardItem> matchingCardItems = 
                cardItemAdapter
                .GetAllCardItems()
                .Where(item => item.Id == cardId)
                .ToImmutableList();

            Debug.Assert(matchingCardItems.Count == 1);
            int oldValue = matchingCardItems[0].Count;
            int difference = newValue - oldValue;

            int value = newValue;
            if ((difference + NumberOfCardsInDeck) > PokemonDeck.NUMBER_OF_CARDS_PER_DECK)
            {
                value = (PokemonDeck.NUMBER_OF_CARDS_PER_DECK - NumberOfCardsInDeck);
                difference = value - oldValue;
            }
            
            NumberOfCardsInDeck += difference;
            cardItemAdapter.SetCardCountForCardWithId(cardId, value);
            Debug.Assert(NumberOfCardsInDeck <= PokemonDeck.NUMBER_OF_CARDS_PER_DECK && NumberOfCardsInDeck >= 0);
        }

        internal async Task<string> CheckUserInput(string name, CardItemAdapter cardItemAdapter)
        {
            string errorMessage = null;
            bool hasBasicPokemon = await HasBasicPokemon(cardItemAdapter);
            // TODO Warn that duplicate name overwrites
            if (name.Length == 0)
            {
                errorMessage = "A deck name is needed to make a deck.";
            }
            else if (NumberOfCardsInDeck != PokemonDeck.NUMBER_OF_CARDS_PER_DECK)
            {
                errorMessage = $"{PokemonDeck.NUMBER_OF_CARDS_PER_DECK} cards are needed to make a deck.";
            }
            else if (!hasBasicPokemon)
            {
                errorMessage = "At least one basic Pokemon is needed to make a deck.";
            }
            // TODO Only one Radiant Pokemon
            // TODO Only one of each Prism star Pokemon
            // TODO Only one ACE SPEC trainer card
            return errorMessage;
        }

        private static async Task<bool> HasBasicPokemon(CardItemAdapter cardItemAdapter)
        {
            // TODO Fossils count as basic Pokemon
            ImmutableArray<CardItem> cardItems = cardItemAdapter.GetAllCardItems().ToImmutableArray();
            bool hasBasicPokemon = false;
            int i = 0;
            while (i < cardItems.Length && !hasBasicPokemon)
            {
                PokemonCard card = await CardDataSource.GetCardById(cardItems[i].Id);
                if (card.Supertype == CardSupertype.POKéMON && card.Subtypes.Contains(CardSubtype.BASIC))
                {
                    hasBasicPokemon = true;
                };
                i++;
            }
            return hasBasicPokemon;
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

        /// <summary>
        /// Creates and saves a PokemonDeck from cards that have been selected by the user and are considered in-deck.
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
            Debug.Assert(cards[PokemonDeck.NUMBER_OF_CARDS_PER_DECK - 1] != null);
            PokemonDeck deck = new(name, cards.ToImmutableArray());
            await DeckDataSource.SaveDeck(deck);
        }

    }

}