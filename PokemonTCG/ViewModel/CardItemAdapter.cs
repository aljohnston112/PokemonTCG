using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PokemonTCG.Enums;
using PokemonTCG.Models;
using PokemonTCG.Utilities;

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
            AssertCount(
                count: count,
                limit: cardItem.Limit
                );
            _cardItems[id] = cardItem.WithCount(count);
            SiftCards();
        }

        private static void AssertCount(int count, int limit)
        {
            if (limit != -1 && (count > limit) || (count < 0 || (count > PokemonDeck.NUMBER_OF_CARDS_PER_DECK)))
            {
                throw new ArgumentException("Count was out of range: " + count);
            }
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

        internal void InludeType(string type, bool inludeType)
        {
            PokemonType pokemonType = EnumUtil.Parse<PokemonType>(type);
            CardSifter = CardSifter.WithTypeIncluded(pokemonType, inludeType);
            SiftCards();
        }

        internal void IncludeOnlyCardsInDeck(bool deckCardsOnly)
        {
            CardSifter = CardSifter.IncludeOnlyCardsFromDeck(deckCardsOnly);
            SiftCards();
        }

        internal void IncludeSupertype(string text, bool value)
        {
            CardSupertype supertype = EnumUtil.Parse<CardSupertype>(text);
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
