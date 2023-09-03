using PokemonTCG.Enums;
using PokemonTCG.Utilities;

using System.Collections.Generic;
using System.Collections.Immutable;

namespace PokemonTCG.States
{
    /// <summary>
    /// Used to sift <c>CardItem</c>s based on criteria.
    /// </summary>
    internal class CardSifter
    {

        private readonly IImmutableList<PokemonType> TypesToInclude;
        private readonly string SearchString = "";
        private readonly bool IncludeOnlyCardsInDeck = false;
        private readonly bool IncludePokemonCards = true;
        private readonly bool IncludeTrainerCards = true;
        private readonly bool IncludeEnergyCards = true;

        /// <summary>
        /// Creates a Sifter with default values.
        /// No cards will be sifted.
        /// </summary>
        internal CardSifter()
        {
            TypesToInclude = ImmutableList.Create<PokemonType>();
        }

        /// <summary>
        /// Creates a card sifter with the specified settings.
        /// </summary>
        /// <param name="typesToInclude">Will include all types if this list is empty.</param>
        /// <param name="searchString">
        /// Only cards with this string contained in their name will be included.
        /// All cards will be included if this is an empty string.
        /// </param>
        /// <param name="includeOnlyCardsInDeck">Whether to only incude cards in the deck.</param>
        /// <param name="includePokemonCards">Whether to include pokemon cards.</param>
        /// <param name="includeTrainerCards">Whether to include trainer cards.</param>
        /// <param name="includeEnergyCards">Whether to include energy cards.</param>
        private CardSifter(
            IImmutableList<PokemonType> typesToInclude,
            string searchString,
            bool includePokemonCards,
            bool includeTrainerCards,
            bool includeEnergyCards,
            bool includeOnlyCardsInDeck
            )
        {
            TypesToInclude = typesToInclude;
            SearchString = searchString;
            IncludePokemonCards = includePokemonCards;
            IncludeTrainerCards = includeTrainerCards;
            IncludeEnergyCards = includeEnergyCards;
            IncludeOnlyCardsInDeck = includeOnlyCardsInDeck;
        }

        /// <summary>
        /// Sift through CardItems.
        /// </summary>
        /// <param name="cardItems">The CardItems to sift.</param>
        /// <returns>A Collection of CardItems that match the criteria of this CardSifter.</returns>
        internal ICollection<CardItem> Sift(ICollection<CardItem> cardItems)
        {
            ICollection<CardItem> matchingCardItems = new HashSet<CardItem>();
            foreach (CardItem card in cardItems)
            {
                bool hasMatchingType = IsCardTypeInIncludedPokemonTypes(card);
                bool containsText = CardContainsText(card);
                bool isInDeck = IsCardInDeck(card);
                bool hasMatchingSupertype = IsCardSuperTypeInIncludedSupertypes(card);
                if (hasMatchingType && containsText && isInDeck && hasMatchingSupertype)
                {
                    matchingCardItems.Add(card);
                }
            }
            return matchingCardItems;
        }

        private bool IsCardTypeInIncludedPokemonTypes(CardItem cardItem)
        {
            bool typeMatches = false;
            if (TypesToInclude.Count == 0)
            {
                typeMatches = true;
            }
            else
            {

                // Get types
                IImmutableList<PokemonType> types;
                if (cardItem.Supertype == CardSupertype.POKéMON)
                {
                    types = cardItem.Types;

                }
                else if (cardItem.Supertype == CardSupertype.ENERGY)
                {
                    PokemonType type = CardUtil.GetEnergyType(cardItem.Name);
                    types = ImmutableList.Create(type);
                }
                else
                {
                    types = ImmutableList.Create<PokemonType>();
                }

                // Check type
                foreach (PokemonType type in types)
                {
                    if (TypesToInclude.IndexOf(type) != -1)
                    {
                        typeMatches = true;
                    }
                }
            }
            return typeMatches;
        }

        private bool CardContainsText(CardItem card)
        {

            return SearchString == "" || card.Name.ToLower().Contains(SearchString.ToLower());
        }

        private bool IsCardInDeck(CardItem card)
        {
            return !IncludeOnlyCardsInDeck || card.Count > 0;
        }

        private bool IsCardSuperTypeInIncludedSupertypes(CardItem cardItem)
        {
            CardSupertype supertype = cardItem.Supertype;
            bool isPokemon = supertype == CardSupertype.POKéMON;
            bool isTrainer = supertype == CardSupertype.TRAINER;
            bool isEnergy = supertype == CardSupertype.ENERGY;
            return isPokemon && IncludePokemonCards || isTrainer && IncludeTrainerCards || isEnergy && IncludeEnergyCards;
        }

        internal CardSifter WithTypeIncluded(PokemonType pokemonType, bool include)
        {
            List<PokemonType> types = new();
            foreach (PokemonType type in TypesToInclude)
            {
                types.Add(type);
            }
            if (include)
            {
                types.Add(pokemonType);
            }
            else
            {
                types.Remove(pokemonType);
            }
            return new CardSifter(
                typesToInclude: types.ToImmutableList(),
                searchString: SearchString,
                includeOnlyCardsInDeck: IncludeOnlyCardsInDeck,
                includePokemonCards: IncludePokemonCards,
                includeTrainerCards: IncludeTrainerCards,
                includeEnergyCards: IncludeEnergyCards
                );
        }

        internal CardSifter WithNewSearchString(string searchString)
        {
            return new CardSifter(
                typesToInclude: TypesToInclude,
                searchString: searchString,
                includeOnlyCardsInDeck: IncludeOnlyCardsInDeck,
                includePokemonCards: IncludePokemonCards,
                includeTrainerCards: IncludeTrainerCards,
                includeEnergyCards: IncludeEnergyCards
                );
        }

        internal CardSifter IncludeOnlyCardsFromDeck(bool includeOnlyCardsInDeck)
        {
            return new CardSifter(
                typesToInclude: TypesToInclude,
                searchString: SearchString,
                includeOnlyCardsInDeck: includeOnlyCardsInDeck,
                includePokemonCards: IncludePokemonCards,
                includeTrainerCards: IncludeTrainerCards,
                includeEnergyCards: IncludeEnergyCards
                );
        }

        internal CardSifter WithPokemonIncluded(bool includePokemonCards)
        {
            return new CardSifter(
                typesToInclude: TypesToInclude,
                searchString: SearchString,
                includeOnlyCardsInDeck: IncludeOnlyCardsInDeck,
                includePokemonCards: includePokemonCards,
                includeTrainerCards: IncludeTrainerCards,
                includeEnergyCards: IncludeEnergyCards
                );
        }

        internal CardSifter WithTrainersIncluded(bool includeTrainerCards)
        {
            return new CardSifter(
                typesToInclude: TypesToInclude,
                searchString: SearchString,
                includeOnlyCardsInDeck: IncludeOnlyCardsInDeck,
                includePokemonCards: IncludePokemonCards,
                includeTrainerCards: includeTrainerCards,
                includeEnergyCards: IncludeEnergyCards
                );
        }

        internal CardSifter WithEnergiesIncluded(bool includeEnergyCards)
        {
            return new CardSifter(
                typesToInclude: TypesToInclude,
                searchString: SearchString,
                includeOnlyCardsInDeck: IncludeOnlyCardsInDeck,
                includePokemonCards: IncludePokemonCards,
                includeTrainerCards: IncludeTrainerCards,
                includeEnergyCards: includeEnergyCards
                );
        }

    }

}
