using System.Collections.Immutable;
using static PokemonTCG.Models.HandCardActionState;

namespace PokemonTCG.Models
{
    internal class CardActionState
    {

        internal readonly PokemonCard Card;
        internal readonly IImmutableDictionary<string, CardFunction> Actions;

        internal CardActionState(PokemonCard card, IImmutableDictionary<string, CardFunction> actions)
        {
            Card = card;
            Actions = actions;
        }

    }

}
