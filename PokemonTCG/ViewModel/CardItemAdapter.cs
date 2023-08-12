using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PokemonTCG.DataSources;
using PokemonTCG.Enums;
using PokemonTCG.Models;
using PokemonTCG.View;

namespace PokemonTCG.ViewModel
{
    internal class CardItemAdapter
    {
        private CardSifter _sifter = new();
        private readonly IDictionary<string, CardItemView> _cardItemViews = new Dictionary<string, CardItemView>();
        private readonly IDictionary<string, CardItem> CardItems = new Dictionary<string, CardItem>();
        internal IEnumerable<CardItem> GetAllCardItems()
        {
            return CardItems.Values;
        }
        internal readonly ObservableCollection<CardItemView> CardItemViews = new();

        internal void AddCardItems(ISet<CardItem> cardItems)
        {
            foreach (var cardItem in cardItems)
            {
                string id = cardItem.Id;
                CardItems.Add(id, cardItem);

                CardItemView view = new(cardItem);
                _cardItemViews.Add(id, view);
                CardItemViews.Add(view);
            }
            SiftCards();
        }

        internal void IncrementCardCountForCardWithId(string id)
        {
            CardItem cardItem = CardItems[id];
            int count = cardItem.Count + 1;
            AssertCount(count, cardItem.Limit);
            SetCardCountForCardWithId(cardItem.Id, count);
        }

        private static void AssertCount(int count, int limit)
        {
            if (limit != -1 && (count > limit) || (count < 0 || (count > DeckDataSource.NUMBER_OF_CARDS_PER_DECK)))
            {
                throw new ArgumentException("Count was out of range: " + count);
            }
        }

        internal void SetCardCountForCardWithId(string id, int count)
        {
            CardItems[id] = CardItems[id].WithCount(count);
            _cardItemViews[id].SetCount(count);
        }

        private void SiftCards()
        {
            Collection<CardItem> cards = _sifter.Sift(CardItems.Values);
            CardItemViews.Clear();
            foreach (CardItem card in cards)
            {
                CardItemViews.Add(_cardItemViews[card.Id]);
            }
        }

        internal void UpdateSearchString(string text)
        {
            _sifter = _sifter.NewString(text);
            SiftCards();
        }

        internal void IncludePokemon(bool includePokemon)
        {
            _sifter = _sifter.IncludePokemon(includePokemon);
            SiftCards();
        }


        internal void IncludeTrainer(bool includeTrainer)
        {
            _sifter = _sifter.IncludeTrainer(includeTrainer);
            SiftCards();
        }

        internal void IncludeEnergy(bool includeEnergy)
        {
            _sifter = _sifter.IncludeEnergy(includeEnergy);
            SiftCards();
        }
        internal void InludeType(PokemonType type, bool inludeType)
        {
            _sifter = _sifter.InludeType(type, inludeType);
            SiftCards();
        }

        internal void IncludeOnlyThoseFromDeck(bool deckCardsOnly)
        {
            _sifter = _sifter.IncludeOnlyThoseFromDeck(deckCardsOnly);
            SiftCards();
        }

    }

}
