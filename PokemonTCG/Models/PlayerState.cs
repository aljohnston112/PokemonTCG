using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using PokemonTCG.Utilities;

namespace PokemonTCG.Models
{

    /// <summary>
    /// Part of the <c>GameState</c>
    /// </summary>
    public class PlayerState
    {
        private static readonly int MAX_BENCH_SIZE = 5;

        public readonly ImmutableList<PokemonCard> Deck;
        public readonly ImmutableList<PokemonCard> Hand;
        public readonly PokemonCard Active;

        public readonly ImmutableList<PokemonCard> Bench;
        public readonly ImmutableList<PokemonCard> Prizes;
        public readonly ImmutableList<PokemonCard> DiscardPile;

        public PlayerState(
            ImmutableList<PokemonCard> deck,
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

        public bool HandHasBasicPokemon()
        {
            bool hasBasic = false;
            foreach (PokemonCard card in Hand)
            {
                if (PokemonCard.IsBasicPokemon(card))
                {
                    hasBasic = true;
                }
            }
            return hasBasic;
        }

        public PlayerState MoveFromHandToActive(PokemonCard active)
        {
            Debug.Assert(Hand.Contains(active));
            return new PlayerState(Deck, Hand.Remove(active), active, Bench, Prizes, DiscardPile);
        }

        public PlayerState MoveFromHandToBench(IList<PokemonCard> benchable)
        {
            Debug.Assert(benchable.Count < (MAX_BENCH_SIZE - Bench.Count));
            ImmutableList<PokemonCard> newHand = Hand;
            foreach (PokemonCard card in benchable)
            {
                Debug.Assert(Hand.Contains(card));
                newHand = newHand.Remove(card);
            }
            return new PlayerState(Deck, newHand, Active, Bench.AddRange(benchable), Prizes, DiscardPile);
        }

        public PlayerState SetUpPrizes()
        {
            (ImmutableList<PokemonCard> deck, ImmutableList<PokemonCard> prizes) = DeckUtil.DrawCards(Deck, 6);
            return new PlayerState(deck, Hand, Active, Bench, prizes, DiscardPile);
        }
    }

}
