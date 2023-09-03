using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.Utilities;

using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace PokemonTCG.States
{
    internal class PokemonCardState
    {
        internal readonly PokemonCard PokemonCard;
        internal readonly IImmutableList<PokemonCard> Energy;
        internal readonly IImmutableList<PokemonCard> EvolvedFrom;
        internal readonly int DamageTaken;
        internal readonly MutuallyExclusiveStatusCondition MutuallyExclusiveStatusConditions;
        internal readonly IImmutableList<StatusCondition> StatusConditions;
        internal readonly bool FirstTurnInPlay;

        /// <summary>
        /// Creates a new card with no energy, 
        /// that has not been evolved, 
        /// has taken no damage, 
        /// has no status conditions and 
        /// is at its first turn in play.
        /// </summary>
        /// <param name="pokemonCard"></param>
        internal PokemonCardState(
            PokemonCard pokemonCard
            ) : this(
                pokemonCard: pokemonCard,
                energy: ImmutableList<PokemonCard>.Empty,
                evolvedFrom: ImmutableList<PokemonCard>.Empty,
                damageTaken: 0,
                mutuallyExclusiveStatusConditions: MutuallyExclusiveStatusCondition.NONE,
                statusesConditions: ImmutableList<StatusCondition>.Empty,
                firstTurnInPlay: true
                )
        { }

        internal PokemonCardState(
            PokemonCard pokemonCard,
            IImmutableList<PokemonCard> energy,
            IImmutableList<PokemonCard> evolvedFrom,
            int damageTaken,
            MutuallyExclusiveStatusCondition mutuallyExclusiveStatusConditions,
            IImmutableList<StatusCondition> statusesConditions,
            bool firstTurnInPlay
            )
        {
            PokemonCard = pokemonCard;
            Energy = energy;
            EvolvedFrom = evolvedFrom;
            DamageTaken = damageTaken;
            MutuallyExclusiveStatusConditions = mutuallyExclusiveStatusConditions;
            StatusConditions = statusesConditions;
            FirstTurnInPlay = firstTurnInPlay;
        }

        internal PokemonCardState AfterAttachingEnergy(PokemonCard card)
        {
            return new PokemonCardState(
                pokemonCard: PokemonCard,
                energy: Energy.Add(card),
                evolvedFrom: EvolvedFrom,
                damageTaken: DamageTaken,
                mutuallyExclusiveStatusConditions: MutuallyExclusiveStatusConditions,
                statusesConditions: StatusConditions,
                firstTurnInPlay: FirstTurnInPlay
                );
        }

        internal PokemonCardState AfterEvolvingTo(PokemonCard evolvedCard)
        {
            Debug.Assert(!FirstTurnInPlay);
            // TODO Remove attack effects and status condition.
            // TODO Turn ends if the evolution is a mega evolution or a primal reversion Pokemon.
            return new PokemonCardState(
                pokemonCard: evolvedCard,
                energy: Energy,
                evolvedFrom: EvolvedFrom.Add(PokemonCard),
                damageTaken: DamageTaken,
                mutuallyExclusiveStatusConditions: MutuallyExclusiveStatusConditions,
                statusesConditions: StatusConditions,
                firstTurnInPlay: true
                );
        }

        internal PokemonCardState WithoutAttack(Attack attack)
        {
            return new PokemonCardState(
                pokemonCard: PokemonCard.WithoutAttack(attack),
                energy: Energy,
                evolvedFrom: EvolvedFrom,
                damageTaken: DamageTaken,
                mutuallyExclusiveStatusConditions: MutuallyExclusiveStatusConditions,
                statusesConditions: StatusConditions,
                firstTurnInPlay: FirstTurnInPlay);
        }

        internal PokemonCardState AfterRemovingEnergy(PokemonType type)
        {
            PokemonCard energyCard = Energy.First(card => CardUtil.GetEnergyType(card) == type);
            IImmutableList<PokemonCard> newEnergy = Energy.Remove(energyCard);
            Debug.Assert(newEnergy.Count == Energy.Count - 1);
            return new PokemonCardState(
                pokemonCard: PokemonCard,
                energy: newEnergy,
                evolvedFrom: EvolvedFrom,
                damageTaken: DamageTaken,
                mutuallyExclusiveStatusConditions: MutuallyExclusiveStatusConditions,
                statusesConditions: StatusConditions,
                firstTurnInPlay: FirstTurnInPlay
                );
        }

        internal PokemonCardState AfterRemovingEnergy(IImmutableList<PokemonCard> energyCard)
        {
            IImmutableList<PokemonCard> newEnergy = Energy.RemoveRange(energyCard);
            Debug.Assert(newEnergy.Count == Energy.Count - 1);
            return new PokemonCardState(
                pokemonCard: PokemonCard,
                energy: newEnergy,
                evolvedFrom: EvolvedFrom,
                damageTaken: DamageTaken,
                mutuallyExclusiveStatusConditions: MutuallyExclusiveStatusConditions,
                statusesConditions: StatusConditions,
                firstTurnInPlay: FirstTurnInPlay
                );
        }

        internal bool CanRetreat()
        {
            return (Energy.Count >= PokemonCard.ConvertedRetreatCost);
        }

        internal int HealthLeft()
        {
            return PokemonCard.Hp - DamageTaken;
        }

    }

}