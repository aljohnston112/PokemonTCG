using System.Collections.Immutable;

namespace PokemonTCG.Models
{
    internal class PokemonCardState
    {
        internal readonly PokemonCard PokemonCard;
        internal readonly IImmutableList<PokemonCard> Energy;
        internal readonly IImmutableList<PokemonCard> EvolvedFrom;

        internal PokemonCardState(
            PokemonCard pokemonCard,
            IImmutableList<PokemonCard> energy,
            IImmutableList<PokemonCard> evolvedFrom
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
            return new PokemonCardState(
                pokemonCard: PokemonCard, 
                energy: Energy.Add(card),
                evolvedFrom: EvolvedFrom
                );
        }

        internal PokemonCardState EvolveTo(PokemonCard evolvedCard)
        {
            return new PokemonCardState(
                pokemonCard: evolvedCard,
                energy: Energy,
                evolvedFrom: EvolvedFrom.Add(PokemonCard)
                );
        }

    }

}
