using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using PokemonTCG.Enums;
using PokemonTCG.Utilities;

namespace PokemonTCG.Models
{
    internal class PokemonCardState
    {
        internal readonly PokemonCard PokemonCard;
        internal readonly IImmutableList<PokemonCard> Energy;
        internal readonly IImmutableList<PokemonCard> EvolvedFrom;
        internal readonly bool FirstTurnInPlay;

        internal PokemonCardState(
            PokemonCard pokemonCard,
            IImmutableList<PokemonCard> energy,
            IImmutableList<PokemonCard> evolvedFrom,
            bool firstTurnInPlay
            )
        {
            PokemonCard = pokemonCard;
            Energy = energy;
            EvolvedFrom = evolvedFrom;
            FirstTurnInPlay = firstTurnInPlay;
        }

        internal PokemonCardState(
            PokemonCard pokemonCard
            ) : this(
                pokemonCard, 
                ImmutableList<PokemonCard>.Empty, 
                ImmutableList<PokemonCard>.Empty,
                firstTurnInPlay: true
            )
        { }

        internal PokemonCardState WithEnergyAttached(PokemonCard card)
        {
            return new PokemonCardState(
                pokemonCard: PokemonCard, 
                energy: Energy.Add(card),
                evolvedFrom: EvolvedFrom,
                firstTurnInPlay: FirstTurnInPlay
                );
        }

        internal PokemonCardState AfterEvolvingTo(PokemonCard evolvedCard)
        {
            Debug.Assert(!FirstTurnInPlay);
            // TODO * i. Remove attack effects and status condition.
            // TODO Turn ends if the evolution is a mega evolution or a primal reversion Pokemon.
            return new PokemonCardState(
                pokemonCard: evolvedCard,
                energy: Energy,
                evolvedFrom: EvolvedFrom.Add(PokemonCard),
                firstTurnInPlay: FirstTurnInPlay
                );
        }

        internal PokemonCardState WithoutAttack(Attack attack)
        {
            return new PokemonCardState(
                pokemonCard: PokemonCard.WithoutAttack(attack),
                energy: Energy,
                evolvedFrom: EvolvedFrom,
                firstTurnInPlay: FirstTurnInPlay
                );
        }

        internal PokemonCardState AfterRemovingEnergy(PokemonType type)
        {
            IImmutableList<PokemonCard> energy = Energy.Remove(
                Energy.First(card => CardUtil.GetEnergyType(card) == type)
                );
            return new PokemonCardState(
                            pokemonCard: PokemonCard,
                            energy: energy,
                            evolvedFrom: EvolvedFrom,
                            firstTurnInPlay: FirstTurnInPlay
                            );
        }

    }

}
