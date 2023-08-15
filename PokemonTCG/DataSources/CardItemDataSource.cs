using PokemonTCG.Enums;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Windows.Data.Json;
using Windows.Storage;

namespace PokemonTCG.DataSources
{

    internal class CardItemDataSource
    {

        internal static async IAsyncEnumerable<CardItem> GetCardItemsForSet(string setName)
        {
            IImmutableList<PokemonCard> cards = await SetDataSource.LoadSet(setName);

            foreach (PokemonCard card in cards)
            {
                CardItem cardItem = CreateCardItem(card);
                yield return cardItem;
            }
        }

        private static CardItem CreateCardItem(PokemonCard card)
        {
            string name = card.Name;
            int cardLimit = GetCardLimit(card);
            return new CardItem(
                id: card.Id, 
                number: card.Number, 
                name: name, 
                imagePath: card.ImagePaths[ImageSize.LARGE],
                limit: cardLimit, 
                count: 0
                );
        }

        private static int GetCardLimit(PokemonCard card)
        {
            int cardLimit = PokemonDeck.NON_ENERGY_CARD_LIMIT;

            // Energies do not have a limit; colorless energies do though
            if (card.Supertype == CardSupertype.ENERGY && card.Name != "Double Colorless Energy")
            {
                cardLimit = -1;
            }
            return cardLimit;
        }

    }

}
