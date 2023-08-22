using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.Utilities;
using System;
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
        internal readonly int DamageTaken;
        internal readonly MutuallyExclusiveStatus MutuallyExclusiveStatus;
        internal readonly IImmutableList<Status> Statuses;
        internal readonly bool FirstTurnInPlay;

        internal PokemonCardState(
            PokemonCard pokemonCard,
            IImmutableList<PokemonCard> energy,
            IImmutableList<PokemonCard> evolvedFrom,
            int damageTaken,
            MutuallyExclusiveStatus mutuallyExclusiveStatus,
            IImmutableList<Status> statuses,
            bool firstTurnInPlay
            )
        {
            PokemonCard = pokemonCard;
            Energy = energy;
            EvolvedFrom = evolvedFrom;
            DamageTaken = damageTaken;
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
                damageTaken: 0,
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
                damageTaken: DamageTaken,
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
                damageTaken: DamageTaken,
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
                damageTaken: DamageTaken,
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
                damageTaken: DamageTaken,
                mutuallyExclusiveStatus: MutuallyExclusiveStatus,
                statuses: Statuses,
                firstTurnInPlay: FirstTurnInPlay
                );
        }

        internal PokemonCardState AfterRemovingEnergy(IImmutableList<PokemonCard> energy)
        {
            IImmutableList<PokemonCard> newEnergy = Energy.RemoveRange(energy);
            return new PokemonCardState(
                pokemonCard: PokemonCard,
                energy: newEnergy,
                evolvedFrom: EvolvedFrom,
                damageTaken: DamageTaken,
                mutuallyExclusiveStatus: MutuallyExclusiveStatus,
                statuses: Statuses,
                firstTurnInPlay: FirstTurnInPlay
                );
        }

        internal bool CanRetreat()
        {
            bool canRetreat = true;
            if(Energy.Count < PokemonCard.ConvertedRetreatCost)
            {
                canRetreat = false;
            }
            return canRetreat;
        }

        internal int HealthLeft()
        {
            return PokemonCard.Hp - DamageTaken;
        }

    }

}
