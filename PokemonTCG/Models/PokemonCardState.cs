using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.Utilities;

using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;


namespace PokemonTCG.Models
{
    internal class PokemonCardState
    {
        internal readonly PokemonCard PokemonCard;
        internal readonly IImmutableList<PokemonCard> Energy;
        internal readonly IImmutableList<PokemonCard> EvolvedFrom;
        internal readonly MutuallyExclusiveStatus MutuallyExclusiveStatus;
        internal readonly IImmutableList<Status> Statuses;
        internal readonly bool FirstTurnInPlay;

        internal PokemonCardState(
            PokemonCard pokemonCard,
            IImmutableList<PokemonCard> energy,
            IImmutableList<PokemonCard> evolvedFrom,
            MutuallyExclusiveStatus mutuallyExclusiveStatus,
            IImmutableList<Status> statuses,
            bool firstTurnInPlay
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
                pokemonCard: pokemonCard,
                energy: ImmutableList<PokemonCard>.Empty,
                evolvedFrom: ImmutableList<PokemonCard>.Empty,
                mutuallyExclusiveStatus: MutuallyExclusiveStatus.NONE,
                statuses: ImmutableList<Status>.Empty,
                firstTurnInPlay: true
                )
        { }

        internal PokemonCardState AfterAttachingEnergy(PokemonCard card)
        {
            return new PokemonCardState(
                pokemonCard: PokemonCard,
                energy: Energy.Add(card),
                evolvedFrom: EvolvedFrom,
                mutuallyExclusiveStatus: MutuallyExclusiveStatus,
                statuses: Statuses,
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
                mutuallyExclusiveStatus: MutuallyExclusiveStatus,
                statuses: Statuses,
                firstTurnInPlay: FirstTurnInPlay
                );
        }

        internal PokemonCardState WithoutAttack(Attack attack)
        {
            return new PokemonCardState(
                pokemonCard: PokemonCard.WithoutAttack(attack),
                energy: Energy,
                evolvedFrom: EvolvedFrom,
                mutuallyExclusiveStatus: MutuallyExclusiveStatus,
                statuses: Statuses,
                firstTurnInPlay: FirstTurnInPlay);
        }

        internal PokemonCardState AfterRemovingEnergy(PokemonType type)
        {
            PokemonCard energyCard =  Energy.First(card => CardUtil.GetEnergyType(card) == type);
            Debug.Assert(energyCard != null);
            IImmutableList<PokemonCard> newEnergy = Energy.Remove(energyCard);
            return new PokemonCardState(
                pokemonCard: PokemonCard,
                energy: newEnergy,
                evolvedFrom: EvolvedFrom,
                mutuallyExclusiveStatus: MutuallyExclusiveStatus,
                statuses: Statuses,
                firstTurnInPlay: FirstTurnInPlay
                );
        }

    }

}
