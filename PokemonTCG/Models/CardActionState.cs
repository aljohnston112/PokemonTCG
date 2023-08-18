using System.Collections.Immutable;
using Microsoft.UI.Xaml.Input;

namespace PokemonTCG.Models
{
    internal class CardActionState<T>
    {

        internal readonly T Card;
        internal readonly IImmutableDictionary<string, TappedEventHandler> Actions;

        internal CardActionState(T card, IImmutableDictionary<string, TappedEventHandler> actions)
        {
            Card = card;
            Actions = actions;
        }

    }

}
