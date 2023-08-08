using System.Collections.Generic;
using System.Collections.Immutable;

namespace PokemonTCG.Models
{

    /// <summary>
    /// Part of the <c>GameState</c>
    /// </summary>
    public class PlayerState
    {

        private ImmutableArray<PokemonCard> Bench;
        private ImmutableArray<PokemonCard> Prizes;
        private ImmutableArray<PokemonCard> Hand;
        private ImmutableArray<PokemonCard> Active;
        private DeckState DeckState;
        private ImmutableArray<PokemonCard> DiscardPile;


    }
}
