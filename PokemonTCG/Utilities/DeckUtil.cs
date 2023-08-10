using PokemonTCG.DataSources;
using PokemonTCG.Models;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace PokemonTCG.Utilities
{
    internal class DeckUtil
    {

        public static (ImmutableList<PokemonCard>, ImmutableList<PokemonCard>) DrawCards(ImmutableList<PokemonCard> deck, int numberOfCards)
        {
            ImmutableList<PokemonCard> drawnCards = deck.Take(numberOfCards).ToImmutableList();
            ImmutableList<PokemonCard> newDeck = deck.Skip(numberOfCards).ToImmutableList();
            return (newDeck, drawnCards);
        }

    }

}
