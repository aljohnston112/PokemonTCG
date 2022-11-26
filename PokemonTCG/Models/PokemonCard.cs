using PokemonTCG.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace PokemonTCG.Models
{
    /// <summary>
    /// To be used in the <c>GamePage</c>
    /// </summary>
    public class PokemonCard : Card
    {
        public readonly int Hp;
        public readonly ImmutableList<PokemonType> Types;
        public readonly ImmutableList<Attack> Attacks;
        public readonly ImmutableList<PokemonPower> PokemonPowers;
        public readonly ImmutableDictionary<PokemonType, string> Weaknesses;
        public readonly ImmutableDictionary<PokemonType, string> Resistances;
        public readonly ImmutableDictionary<PokemonType, int> RetreatCost;
        public readonly string EvolvesFrom;

        /// <summary>
        /// Creates a <c>PokemonCard</c>. 
        /// Should be supplied with card data from the json files.
        /// </summary>
        /// <param name="url">The url for the image of the Pokmeon card</param>
        /// <param name="id">The unique id of the card</param>
        /// <param name="name">The name of the card</param>
        /// <param name="hp">The hp of the card</param>
        /// <param name="types">The <c>PokemonType</c>s of the card</param>
        /// <param name="attacks">The <c>Attack</c>s of the card</param>
        /// <param name="pokemonPowers">The <c>PokemonPower</c>s of the card</param>
        /// <param name="weaknesses">The weaknesses of the card. 
        ///                          A Dictionary of <c>PokemonType</c> to the modifier string.</param>
        /// <param name="resistances">The resistances of the card.
        ///                           A Dictionary of <c>PokemonType</c> to the modifier string.</param>
        /// <param name="retreatCost">The retreat cost of the card.
        ///                           A Dictionary of <c>PokemonType</c> to number of energies.</param>
        /// <param name="evolvesFrom">The name of the card that this card evolves from</param>
        public PokemonCard(
             string url,
             string id,
             string name,
             int hp,
             List<PokemonType> types,
             List<Attack> attacks,
             List<PokemonPower> pokemonPowers,
             Dictionary<PokemonType, string> weaknesses,
             Dictionary<PokemonType, string> resistances,
             Dictionary<PokemonType, int> retreatCost,
             string evolvesFrom
        ) : base(url, id, name)
        {
            this.Hp = hp;
            this.Types = types.ToImmutableList();
            this.Attacks = attacks.ToImmutableList();
            this.PokemonPowers = pokemonPowers.ToImmutableList();
            this.Weaknesses = weaknesses.ToImmutableDictionary();
            this.Resistances = resistances.ToImmutableDictionary();
            this.RetreatCost = retreatCost.ToImmutableDictionary();
            this.EvolvesFrom = evolvesFrom;
        }

    }
}
