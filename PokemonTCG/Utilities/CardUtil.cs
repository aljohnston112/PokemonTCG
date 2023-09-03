using System;
using System.Collections.Immutable;
using System.Linq;

using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.States;

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
                if (IsBasicPokemon(pokemonCard))
                {
                    count++;
                }
            }
            return count;
        }

        internal static PokemonType GetEnergyType(PokemonCard card)
        {
            return GetEnergyType(card.Name);
        }

        internal static PokemonType GetEnergyType(string name)
        {
            string[] energyTypes = name.Split();
            string energyType = energyTypes[^2];
            return EnumUtil.Parse<PokemonType>(energyType);
        }

        internal static int IndexOfCardWithId(IImmutableList<PokemonCardState> cardStates, string benchedCardId)
        {
            bool found = false;
            int i = -1;
            while (i < cardStates.Count - 1 && !found)
            {
                i++;
                PokemonCard card = cardStates[i].PokemonCard;
                if (card.Id == benchedCardId)
                {
                    found = true;
                }
            }
            return i;
        }

        internal static int IndexOfEnergyCardWithType(IImmutableList<PokemonCard> cards, PokemonType energyType)
        {
            bool found = false;
            int i = -1;
            while (i < cards.Count - 1 && !found)
            {
                i++;
                PokemonCard card = cards[i];
                if (GetEnergyType(card) == energyType)
                {
                    found = true;
                }
            }
            return i;
        }

        internal static IImmutableDictionary<PokemonType, int> GetNumberOfEachEnergy(IImmutableList<PokemonCard> cards)
        {
            return cards
                .Where(card => card.Supertype == CardSupertype.ENERGY)
                .GroupBy(card => GetEnergyType(card))
                .ToImmutableDictionary(group => group.Key, group => group.Count());
        }

        internal static int GetNumberOfEnergy(IImmutableList<PokemonCard> cards)
        {
            return cards
                .Where(card => card.Supertype == CardSupertype.ENERGY)
                .Count();
        }

        internal static void AssertCardEvolvesFrom(PokemonCardState active, PokemonCard evolutionCard)
        {
            if (!CardEvolvesFrom(active, evolutionCard))
            {
                throw new ArgumentException($"Card with id: {evolutionCard.Id} does not evolve from {active.PokemonCard.Name}");
            }
        }

        internal static bool CardEvolvesFrom(PokemonCardState active, PokemonCard evolutionCard)
        {
            bool evolvesFrom = true;
            string lowerStageName = active.PokemonCard.Name;
            if (lowerStageName != evolutionCard.EvolvesFrom)
            {
                evolvesFrom = false;
            }
            return evolvesFrom;
        }

    }

}
