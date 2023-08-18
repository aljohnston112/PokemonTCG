using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.Models;

using System.Collections.Generic;
using System;
using System.Collections.Immutable;
using System.Linq;


namespace PokemonTCG.Utilities
{
    internal class DeckUtil
    {

        /// <summary>
        /// Fisher-Yates shuffle algorithm on a deck.
        /// </summary>
        /// <param name="deck"></param>
        /// <returns></returns>
        internal static IImmutableList<string> ShuffleDeck(PokemonDeck deck)
        {
            IList<string> CardIds = deck.CardIds;
            Random random = new();
            List<string> shuffledIds = new(CardIds);
            for (int i = CardIds.Count -1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (shuffledIds[i], shuffledIds[j]) = (shuffledIds[j], shuffledIds[i]);
            }
            return shuffledIds.ToImmutableList();
        }

        internal static (IImmutableList<PokemonCard>, IImmutableList<PokemonCard>) DrawCards(
            IImmutableList<PokemonCard> deck, 
            int numberOfCards
            )
        {
            IImmutableList<PokemonCard> drawnCards = deck.Take(numberOfCards).ToImmutableList();
            IImmutableList<PokemonCard> newDeck = deck.Skip(numberOfCards).ToImmutableList();
            return (newDeck, drawnCards);
        }

        internal static int GetCardLimit(PokemonCard card)
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
