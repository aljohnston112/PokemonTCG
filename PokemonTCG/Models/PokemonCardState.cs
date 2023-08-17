using PokemonTCG.Enums;
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
        internal readonly MutuallyExclusiveStatus MutuallyExclusiveStatus;
        internal readonly ImmutableList<Status> Statuses;
        internal readonly bool FirstTurnInPlay;

        internal PokemonCardState(
            PokemonCard pokemonCard,
            IImmutableList<PokemonCard> energy,
            IImmutableList<PokemonCard> evolvedFrom,
            bool firstTurnInPlay
            IImmutableList<PokemonCard> evolvedFrom,
            MutuallyExclusiveStatus mutuallyExclusiveStatus,
            ImmutableList<Status> statuses
            )
        {
            PokemonCard = pokemonCard;
            Energy = energy;
            EvolvedFrom = evolvedFrom;
            MutuallyExclusiveStatus = mutuallyExclusiveStatus;
            Statuses = statuses;
            FirstTurnInPlay = firstTurnInPlay;
        }

        internal PokemonCardState(
            PokemonCard pokemonCard
            ) : this(
                pokemonCard, 
                ImmutableList<PokemonCard>.Empty, 
                ImmutableList<PokemonCard>.Empty,
                firstTurnInPlay: true
                ImmutableList<PokemonCard>.Empty,
                MutuallyExclusiveStatus.NONE,
                ImmutableList<Status>.Empty
            )
        { }

        internal PokemonCardState WithEnergyAttached(PokemonCard card)
        {
            return new PokemonCardState(
                pokemonCard: PokemonCard, 
                energy: Energy.Add(card),
                evolvedFrom: EvolvedFrom
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
                evolvedFrom: EvolvedFrom.Add(PokemonCard),
                mutuallyExclusiveStatus: MutuallyExclusiveStatus.NONE,
                statuses: ImmutableList<Status>.Empty
                );
        }

    }

}
