using System;
using System.Collections;
using System.Collections.Immutable;
using System.Linq;
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
                if (IsBasicPokemon(pokemonCard))
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

        internal static int IndexOfCard(IImmutableList<PokemonCardState> cardStates, string benchedCardId)
        {
            bool found = false;
            int i = -1;
            while (i < cardStates.Count && !found)
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

        internal static int IndexOfCard(IImmutableList<PokemonCard> cards, string benchedCardId)
        {
            bool found = false;
            int i = -1;
            while (i < cards.Count && !found)
            {
                i++;
                PokemonCard card = cards[i];
                if (card.Id == benchedCardId)
                {
                    found = true;
                }
            }
            return i;
        }

        internal static IImmutableDictionary<PokemonType, int> GetNumberOfEveryEnergy(IImmutableList<PokemonCard> cards)
        {
            return cards
                .Where(card => card.Supertype == CardSupertype.ENERGY)
                .GroupBy(card => CardUtil.GetEnergyType(card))
                .ToImmutableDictionary(group => group.Key, group => group.Count());
        }

        internal static int GetNumberOfEnergy(IImmutableList<PokemonCard> cards)
        {
            return cards
                .Where(card => card.Supertype == CardSupertype.ENERGY)
                .Count();
        }

        internal static bool IsEnoughEnergyForAttack(IImmutableList<PokemonCard> cards, Attack attack)
        {
            bool enoughEnergyForAttack = true;

            // Count energy cards from hand
            IImmutableDictionary<PokemonType, int> numberOfEveryEnergy = GetNumberOfEveryEnergy(cards);
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
