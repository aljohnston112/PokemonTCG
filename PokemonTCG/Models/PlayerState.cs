using System.Collections.Generic;

namespace PokemonTCG.Models
{

    /// <summary>
    /// Part of the <c>GameState</c>
    /// </summary>
    public class PlayerState
    {

        private readonly PokemonCard[] Bench = new PokemonCard[5];
        private readonly PokemonCard[] Prizes = new PokemonCard[6];
        private readonly PokemonCard[] Hand = new PokemonCard[7];
        private readonly PokemonCard[] Active;
        private readonly List<PokemonType> Deck = new();
        private readonly List<PokemonCard> DiscardPile = new();


    }
}
