using System.Collections.Generic;

namespace PokemonTCG.Models
{

    /// <summary>
    /// Part of the <c>GameState</c>
    /// </summary>
    public class PlayerState
    {

        private PokemonCard[] _bench = new PokemonCard[5];
        private PokemonCard[] _prizes = new PokemonCard[6];
        private PokemonCard[] _hand = new PokemonCard[7];
        private PokemonCard[] _active;
        private List<PokemonType> _deck = new();
        private List<PokemonCard> _discardPile = new();


    }
}
