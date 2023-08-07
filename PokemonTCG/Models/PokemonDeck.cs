using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace PokemonTCG.Models
{
    public class PokemonDeck
    {

        public readonly ImmutableArray<string> CardIds;
        public readonly string Name;

        /// <summary>
        /// Creates a Deck given the ids of Pokemon cards.
        /// </summary>
        /// <param name="ids">The ids of Pokemon cards</param>
        public PokemonDeck(string name, ICollection<string> ids)
        {
            Debug.Assert(ids.Count == 60);
            CardIds = ids.ToImmutableArray();
            Name = name;
        }

        public static readonly PokemonDeck BLACKOUT_DECK = new(
            "Blackout",
            ImmutableList.Create(
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
