using System.Collections.Generic;
using System.Collections.Immutable;

namespace PokemonTCG.Models
{

    /// <summary>
    /// Part of the <c>GameState</c>
    /// </summary>
    internal class PlayerState
    {
        internal ImmutableList<string> Deck;
        internal ImmutableList<PokemonCard> Hand;
        internal PokemonCard Active;

        internal ImmutableList<PokemonCard> Bench;
        internal ImmutableList<PokemonCard> Prizes;
        internal ImmutableList<PokemonCard> DiscardPile;

        internal PlayerState(
            ImmutableList<string> deck,
            ImmutableList<PokemonCard> hand
        )
        {
            Deck = deck;
            Hand = hand;
            Active = null;
            Bench = ImmutableList.Create<PokemonCard>();
            Prizes = ImmutableList.Create<PokemonCard>();
            DiscardPile = ImmutableList.Create<PokemonCard>();
        }

        internal PlayerState(
            ImmutableList<string> deck,
            ImmutableList<PokemonCard> hand,
            PokemonCard active,

            ImmutableList<PokemonCard> bench,
            ImmutableList<PokemonCard> prizes,
            ImmutableList<PokemonCard> discardPile
        )
        {
            Deck = deck;
            Hand = hand;
            Active = active;

            Bench = bench;
            Prizes = prizes;
            DiscardPile = discardPile;
        }

        internal bool HandHasBasicPokemon()
        {
            bool hasBasic = false;
            foreach (PokemonCard card in Hand)
            {
                if (card.Subtypes.Contains(CardSubtype.BASIC))
                {
                    hasBasic = true;
                }
            }
            return hasBasic;
        }

    }

}
