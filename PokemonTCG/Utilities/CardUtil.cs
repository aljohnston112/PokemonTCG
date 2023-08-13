using PokemonTCG.Enums;
using PokemonTCG.Models;

namespace PokemonTCG.Utilities
{
    internal class CardUtil
    {

        internal static bool IsBasicPokemon(PokemonCard card)
        {
            return card.Supertype == CardSupertype.POKéMON && card.Subtypes.Contains(CardSubtype.BASIC);
        }

        internal static PokemonType GetEnergyType(PokemonCard card)
        {
            string[] energyTypes = card.Name.Split();
            string energyType = energyTypes[^2];
            return EnumUtil.Parse<PokemonType>(energyType);
        }

    }

}
