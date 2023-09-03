using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.States;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PokemonTCG.Utilities
{

    internal class AttackUtil
    {

        /// <summary>
        /// Gets the amount of energy needed to attack after removing the given energy from the attack cost.
        /// </summary>
        /// <param name="attack">The attack to get the energy needed for.</param>
        /// <param name="energy">The energy to remove from the attack cost.</param>
        /// <returns>A dictionary of pokemon types to numbers representing the number of each energy needed for the attack cost.</returns>
        internal static IImmutableDictionary<PokemonType, int> GetEnergyNeededToAttack(
            Attack attack, 
            IImmutableList<PokemonCard> energy
            )
        {
            Dictionary<PokemonType, int> attackEnergyCostLeft = new(attack.EnergyCost);

            int colorlessEnergyNeeded = attackEnergyCostLeft[PokemonType.Colorless];
            attackEnergyCostLeft.Remove(PokemonType.Colorless);

            foreach(PokemonCard energyCard in energy)
            {
                PokemonType energyType = CardUtil.GetEnergyType(energyCard);
                if (attackEnergyCostLeft.ContainsKey(energyType)){
                    if (attackEnergyCostLeft[energyType] > 0)
                    {
                        attackEnergyCostLeft[energyType]--;
                    } else
                    {
                        colorlessEnergyNeeded--;
                    }
                    
                } else
                {
                    colorlessEnergyNeeded--;
                }
            }

            attackEnergyCostLeft[PokemonType.Colorless] = Math.Max(0, colorlessEnergyNeeded);
            return attackEnergyCostLeft.ToImmutableDictionary();
        }

        internal static int GetEnergyNeededToAttack(Attack attack, PokemonCardState playerCard)
        {
            Dictionary<PokemonType, int> attackEnergyCostLeft = new(attack.EnergyCost);

            int colorlessEnergyNeeded = attackEnergyCostLeft[PokemonType.Colorless];
            attackEnergyCostLeft.Remove(PokemonType.Colorless);

            foreach (PokemonCard energyCard in playerCard.Energy)
            {
                PokemonType energyType = CardUtil.GetEnergyType(energyCard);
                if (attackEnergyCostLeft.ContainsKey(energyType))
                {
                    if (attackEnergyCostLeft[energyType] > 0)
                    {
                        attackEnergyCostLeft[energyType]--;
                    }
                    else
                    {
                        colorlessEnergyNeeded--;
                    }

                }
                else
                {
                    colorlessEnergyNeeded--;
                }
            }

            int numberOfEnergy = Math.Max(0, colorlessEnergyNeeded);
            foreach(int numEnergy in attackEnergyCostLeft.Values)
            {
                numberOfEnergy += numEnergy;
            }
            return numberOfEnergy;
        }

        internal static bool IsEnoughEnergyForAttack(IImmutableList<PokemonCard> cards, Attack attack)
        {
            bool enoughEnergyForAttack = true;

            // Count energy cards from hand
            IImmutableDictionary<PokemonType, int> numberOfEveryEnergy = CardUtil.GetNumberOfEachEnergy(cards);
            int numberOfEnergies = CardUtil.GetNumberOfEnergy(cards);
            int energyLeftForColorless = numberOfEnergies;

            foreach ((PokemonType type, int count) in attack.EnergyCost)
            {
                if ((!numberOfEveryEnergy.ContainsKey(type) || (numberOfEveryEnergy[type] < count)) ||
                    (type == PokemonType.Colorless && energyLeftForColorless < count))
                {
                    enoughEnergyForAttack = false;
                }
                energyLeftForColorless -= count;
            }
            return enoughEnergyForAttack;
        }

    }

}