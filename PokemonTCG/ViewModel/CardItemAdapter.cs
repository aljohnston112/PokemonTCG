using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PokemonTCG.Enums;
using PokemonTCG.Models;
using PokemonTCG.States;
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

        internal async void IncrementCardCountForCardWithId(string id)
        {
            CardItem cardItem = _cardItems[id];
            int count = cardItem.Count + 1;
            await SetCardCountForCardWithId(cardItem.Id, count);
        }

        internal async Task SetCardCountForCardWithId(string id, int count)
        {
            CardItem cardItem = _cardItems[id];
            AssertCount(
                count: count,
                limit: cardItem.Limit
                );
            _cardItems[id] = cardItem.WithCount(count);
            await SiftCards();
        }

        private static void AssertCount(int count, int limit)
        {
            if (limit != -1 && (count > limit) || (count < 0 || (count > PokemonDeck.NUMBER_OF_CARDS_PER_DECK)))
            {
                throw new ArgumentException("Count was out of range: " + count);
            }
        }

        private async Task SiftCards()
        {
            ICollection<CardItem> cards = await CardSifter.Sift(_cardItems.Values);
            CardItems.Clear();
            foreach (CardItem card in cards)
            {
                CardItems.Add(card);
            }
        }

        internal async Task UpdateSearchString(string text)
        {
            CardSifter = CardSifter.WithNewSearchString(text);
            await SiftCards();
        }

        internal async Task InludeType(string type, bool inludeType)
        {
            PokemonType pokemonType = EnumUtil.Parse<PokemonType>(type);
            CardSifter = CardSifter.WithTypeIncluded(pokemonType, inludeType);
            await SiftCards();
        }

        internal async Task IncludeOnlyCardsInDeck(bool deckCardsOnly)
        {
            CardSifter = CardSifter.IncludeOnlyCardsFromDeck(deckCardsOnly);
            await SiftCards();
        }

        internal async Task IncludeSupertype(string text, bool value)
        {
            CardSupertype supertype = EnumUtil.Parse<CardSupertype>(text);
            if (supertype == CardSupertype.POKéMON)
            {
                await IncludePokemon(value);
            }
            else if (supertype == CardSupertype.TRAINER)
            {
                await IncludeTrainer(value);
            }
            else if (supertype == CardSupertype.ENERGY)
            {
                await IncludeEnergy(value);
            }
        }

        private async Task IncludePokemon(bool includePokemon)
        {
            CardSifter = CardSifter.WithPokemonIncluded(includePokemon);
            await SiftCards();
        }


        private async Task IncludeTrainer(bool includeTrainer)
        {
            CardSifter = CardSifter.WithTrainersIncluded(includeTrainer);
            await SiftCards();
        }

        private async Task IncludeEnergy(bool includeEnergy)
        {
            CardSifter = CardSifter.WithEnergiesIncluded(includeEnergy);
            await SiftCards();
        }

    }

}
