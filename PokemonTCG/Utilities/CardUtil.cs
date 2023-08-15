using System.Collections;
using System.Collections.Immutable;
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

        internal static int NumberOfBasicPokemon(IImmutableList<PokemonCard> pokemonCards)
        {
            int count = 0;
            foreach (PokemonCard pokemonCard in pokemonCards)
            {
                if(IsBasicPokemon(pokemonCard))
                {
                    count++;
                }
            }
            return count;
        }

        internal static PokemonType GetEnergyType(PokemonCard card)
        {
            string[] energyTypes = card.Name.Split();
            string energyType = energyTypes[^2];
            return EnumUtil.Parse<PokemonType>(energyType);
        }

    }

}
