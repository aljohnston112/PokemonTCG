using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace PokemonTCG.Models
{
    internal class PokemonDeck
    {
        internal const int NUMBER_OF_CARDS_PER_DECK = 60;
        internal const int NON_ENERGY_CARD_LIMIT = 4;

        internal readonly string Name;
        internal readonly ImmutableArray<string> CardIds;

        /// <summary>
        /// Creates a Deck given the ids of Pokemon cards.
        /// </summary>
        /// <param name="name">The name of the deck.</param>
        /// <param name="ids">The ids of Pokemon cards</param>
        internal PokemonDeck(string name, ImmutableArray<string> ids)
        {
            Debug.Assert(ids.Length == 60);
            CardIds = ids;
            Name = name;
        }

        internal static readonly PokemonDeck BLACKOUT_DECK = new(
            "Blackout",
            ImmutableArray.Create(
                "base1-7", "base1-79", "base1-93", "base1-84", "base1-88", "base1-34",
                "base1-34", "base1-42", "base1-42", "base1-27", "base1-27", "base1-62",
                "base1-62", "base1-62", "base1-56", "base1-56", "base1-56", "base1-65",
                "base1-65", "base1-65", "base1-52", "base1-52", "base1-52", "base1-52",
                "base1-63", "base1-63", "base1-63", "base1-63", "base1-92", "base1-92",
                "base1-92", "base1-92", "base1-102", "base1-102", "base1-102", "base1-102",
                "base1-102", "base1-102", "base1-102", "base1-102", "base1-102", "base1-102",
                "base1-102", "base1-102", "base1-97", "base1-97", "base1-97", "base1-97",
                "base1-97", "base1-97", "base1-97", "base1-97", "base1-97", "base1-97",
                "base1-97", "base1-97", "base1-97", "base1-97", "base1-97", "base1-97"
                )
            );

    }

}
