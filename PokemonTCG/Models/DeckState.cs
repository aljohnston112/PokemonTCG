using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTCG.Models
{
    internal class DeckState
    {

        private readonly ImmutableArray<string> CardIds;

        public DeckState(ImmutableArray<string> ids) {
            CardIds = ids;
        }

        public (DeckState, IList<string>) DrawCards(int numberOfCards)
        {
            List<string> drawnCards = CardIds.Take(numberOfCards).ToList();
            DeckState state = new(CardIds.Skip(numberOfCards).ToImmutableArray());
            return (state, drawnCards);
        }

    }

}
