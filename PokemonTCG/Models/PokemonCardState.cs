using System.Collections.Immutable;

namespace PokemonTCG.Models
{
    internal class PokemonCardState
    {
        internal readonly PokemonCard PokemonCard;
        internal readonly ImmutableList<PokemonCard> Energy;
        internal readonly ImmutableList<PokemonCard> EvolvedFrom;

        internal PokemonCardState(
            PokemonCard pokemonCard,
            ImmutableList<PokemonCard> energy,
            ImmutableList<PokemonCard> evolvedFrom
            )
        {
            PokemonCard = pokemonCard;
            Energy = energy;
            EvolvedFrom = evolvedFrom;
        }

        internal PokemonCardState(
            PokemonCard pokemonCard
            ) : this(
                pokemonCard, 
                ImmutableList<PokemonCard>.Empty, 
                ImmutableList<PokemonCard>.Empty
            )
        { }

        internal PokemonCardState AttachEnergy(PokemonCard card)
        {
            return new PokemonCardState(PokemonCard, Energy.Add(card), EvolvedFrom);
        }

        internal PokemonCardState EvolveTo(PokemonCard evolvedCard)
        {
            return new PokemonCardState(evolvedCard, Energy, EvolvedFrom.Add(PokemonCard));
        }

    }

}
