using System;
using System.Collections.Immutable;

namespace PokemonTCG.States
{
    internal class CardActionState<T>
    {

        internal readonly T Card;
        internal readonly IImmutableDictionary<string, Action> Actions;

        internal CardActionState(T card, IImmutableDictionary<string, Action> actions)
        {
            Card = card;
            Actions = actions;
        }

    }

}
