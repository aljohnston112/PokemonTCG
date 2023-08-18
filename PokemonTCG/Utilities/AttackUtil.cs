using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using System.Collections.Immutable;

namespace PokemonTCG.Utilities
{
    internal class AttackUtil
    {

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
