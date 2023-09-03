using PokemonTCG.Enums;
using PokemonTCG.States;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PokemonTCG.ViewModel
{
    internal class CardItemAdapter
    {
        private CardSifter CardSifter = new();
        private readonly IDictionary<string, CardItem> _cardItems = new Dictionary<string, CardItem>();

        internal readonly ObservableCollection<CardItem> CardItems = new();

        internal ICollection<CardItem> GetAllCardItems()
        {
            return _cardItems.Values;
        }

        internal void AddCardItem(CardItem cardItem)
        {
            _cardItems.Add(cardItem.Id, cardItem);
            CardItems.Add(cardItem);
        }

        internal void IncrementCardCountForCardWithId(string id)
        {
            CardItem cardItem = _cardItems[id];
            int count = cardItem.Count + 1;
            SetCardCountForCardWithId(cardItem.Id, count);
        }

        internal void SetCardCountForCardWithId(string id, int count)
        {
            CardItem cardItem = _cardItems[id];
            Debug.Assert(
                cardItem.Limit != -1 && (count > cardItem.Limit) || (count < 0 || (count > PokemonDeck.NUMBER_OF_CARDS_PER_DECK)),
                "Count was out of range: " + count
                );
            _cardItems[id] = cardItem.WithCount(count);
            SiftCards();
        }

        private void SiftCards()
        {
            ICollection<CardItem> cards = CardSifter.Sift(_cardItems.Values);
            CardItems.Clear();
            foreach (CardItem card in cards)
            {
                CardItems.Add(card);
            }
        }

        internal void UpdateSearchString(string text)
        {
            CardSifter = CardSifter.WithNewSearchString(text);
            SiftCards();
        }

        internal void InludeType(PokemonType type, bool inludeType)
        {
            CardSifter = CardSifter.WithTypeIncluded(type, inludeType);
            SiftCards();
        }

        internal void IncludeOnlyCardsInDeck(bool deckCardsOnly)
        {
            CardSifter = CardSifter.IncludeOnlyCardsFromDeck(deckCardsOnly);
            SiftCards();
        }

        internal void IncludeSupertype(CardSupertype type, bool value)
        {
            CardSupertype supertype = type;
            if (supertype == CardSupertype.POKéMON)
            {
                IncludePokemon(value);
            }
            else if (supertype == CardSupertype.TRAINER)
            {
                IncludeTrainer(value);
            }
            else if (supertype == CardSupertype.ENERGY)
            {
                IncludeEnergy(value);
            }
        }

        private void IncludePokemon(bool includePokemon)
        {
            CardSifter = CardSifter.WithPokemonIncluded(includePokemon);
            SiftCards();
        }

        private void IncludeTrainer(bool includeTrainer)
        {
            CardSifter = CardSifter.WithTrainersIncluded(includeTrainer);
            SiftCards();
        }

        private void IncludeEnergy(bool includeEnergy)
        {
            CardSifter = CardSifter.WithEnergiesIncluded(includeEnergy);
            SiftCards();
        }

    }

}