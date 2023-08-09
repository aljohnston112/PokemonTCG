using PokemonTCG.DataSources;
using PokemonTCG.Models;
using System.Collections.Immutable;
using System.Linq;

namespace PokemonTCG.Utilities
{
    internal class DeckUtil
    {

        public static PlayerState DrawCards(ImmutableList<string> deck, int numberOfCards)
        {
            ImmutableList<PokemonCard> drawnCards = deck.Take(numberOfCards).Select(id => CardDataSource.GetCardById(id)).ToImmutableList();
            ImmutableList<string> newDeck = deck.Skip(numberOfCards).ToImmutableList();
            PlayerState playerState = new(deck: newDeck, hand: drawnCards);
            return playerState;
        }

    }
}
