using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using PokemonTCG.DataSources;
using PokemonTCG.Enums;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using PokemonTCG.View;
using static System.Net.Mime.MediaTypeNames;

namespace PokemonTCG.ViewModel
{
    internal class CardItemAdapter
    {
        private CardSifter _sifter = new();
        private readonly IDictionary<string, CardItem> _cardItems = new Dictionary<string, CardItem>();

        internal IEnumerable<CardItem> GetAllCardItems()
        {
            return _cardItems.Values;
        }
        internal readonly ObservableCollection<CardItem> CardItems = new();

        internal void AddCardItem(CardItem cardItem)
        {
            string id = cardItem.Id;
            _cardItems.Add(id, cardItem);
            CardItems.Add(cardItem);
            SiftCards();
        }

        internal void IncrementCardCountForCardWithId(string id)
        {
            CardItem cardItem = _cardItems[id];
            int count = cardItem.Count + 1;
            AssertCount(count, cardItem.Limit);
            SetCardCountForCardWithId(cardItem.Id, count);
        }

        private static void AssertCount(int count, int limit)
        {
            if (limit != -1 && (count > limit) || (count < 0 || (count > PokemonDeck.NUMBER_OF_CARDS_PER_DECK)))
            {
                throw new ArgumentException("Count was out of range: " + count);
            }
        }

        internal void SetCardCountForCardWithId(string id, int count)
        {
            _cardItems[id] = _cardItems[id].WithCount(count);
        }

        private void SiftCards()
        {
            ICollection<CardItem> cards = _sifter.Sift(_cardItems.Values);
            CardItems.Clear();
            foreach (CardItem card in cards)
            {
                CardItems.Add(_cardItems[card.Id]);
            }
        }

        internal void InludeType(string type, bool inludeType)
        {
            PokemonType pokemonType = EnumUtil.Parse<PokemonType>(type);
            _sifter = _sifter.WithTypeIncluded(pokemonType, inludeType);
            SiftCards();
        }

        internal void UpdateSearchString(string text)
        {
            _sifter = _sifter.WithNewSearchString(text);
            SiftCards();
        }

        internal void IncludeOnlyThoseInDeck(bool deckCardsOnly)
        {
            _sifter = _sifter.IncludeOnlyThoseFromDeck(deckCardsOnly);
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
            _sifter = _sifter.WithPokemonIncluded(includePokemon);
            SiftCards();
        }


        private void IncludeTrainer(bool includeTrainer)
        {
            _sifter = _sifter.WithTrainersIncluded(includeTrainer);
            SiftCards();
        }

        private void IncludeEnergy(bool includeEnergy)
        {
            _sifter = _sifter.WithEnergiesIncluded(includeEnergy);
            SiftCards();
        }

    }

}
