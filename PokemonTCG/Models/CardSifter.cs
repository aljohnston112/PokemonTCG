using PokemonTCG.DataSources;
using PokemonTCG.Enums;
using PokemonTCG.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

namespace PokemonTCG.Models
{
    /// <summary>
    /// Used to sift <c>CardItem</c>s based on criteria.
    /// </summary>
    internal class CardSifter
    {

        private readonly IImmutableList<PokemonType> TypesToInclude;
        private readonly string SearchString = "";
        private readonly bool IncludeOnlyThoseInDeck = false;
        private readonly bool IncludePokemonCards = true;
        private readonly bool IncludeTrainerCards = true;
        private readonly bool IncludeEnergyCards = true;

        /// <summary>
        /// Creates a Sifter with default values.
        /// No cards will be sifted.
        /// </summary>
        internal CardSifter() {
            TypesToInclude = ImmutableList.Create<PokemonType>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typesToInclude">Will include all types if this list is empty.</param>
        /// <param name="searchString"></param>
        /// <param name="includeOnlyThoseInDeck"></param>
        /// <param name="includePokemonCards"></param>
        /// <param name="includeTrainerCards"></param>
        /// <param name="includeEnergyCards"></param>
        private CardSifter(
            IImmutableList<PokemonType> typesToInclude,
            string searchString,
            bool includePokemonCards,
            bool includeTrainerCards,
            bool includeEnergyCards,
            bool includeOnlyThoseInDeck
            )
        {
            TypesToInclude = typesToInclude;
            SearchString = searchString;
            IncludePokemonCards = includePokemonCards;
            IncludeTrainerCards = includeTrainerCards;
            IncludeEnergyCards = includeEnergyCards;
            IncludeOnlyThoseInDeck = includeOnlyThoseInDeck;
        }

        /// <summary>
        /// Sift through CardItems.
        /// </summary>
        /// <param name="cardItems">The CardItems to sift.</param>
        /// <returns>A Collection of CardItems that match the criteria of this CardSifter.</returns>
        internal ICollection<CardItem> Sift(ICollection<CardItem> cardItems)
        {
            Collection<CardItem> matchingCardItems = new();
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
                PokemonCard card = CardDataSource.GetCardById(cardItem.Id);

                // Get types
                IImmutableList<PokemonType> types;
                if (card.Supertype == CardSupertype.POKéMON)
                {
                    types = card.Types;

                }
                else if (card.Supertype == CardSupertype.ENERGY)
                {
                    PokemonType type = CardUtil.GetEnergyType(card);
                    types = ImmutableList.Create(type);
                }
                else
                {
                    types = ImmutableList.Create<PokemonType>();
                }

                // Check type
                foreach (PokemonType type in types)
                {
                    if (TypesToInclude.Contains(type))
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
            return !IncludeOnlyThoseInDeck || card.Count > 0;
        }

        private bool IsCardSuperTypeInIncludedSupertypes(CardItem cardItem)
        {
            PokemonCard card = CardDataSource.GetCardById(cardItem.Id);
            CardSupertype supertype = card.Supertype;
            bool isPokemon = (supertype == CardSupertype.POKéMON);
            bool isTrainer = (supertype == CardSupertype.TRAINER);
            bool isEnergy = (supertype == CardSupertype.ENERGY);
            return ((isPokemon && IncludePokemonCards) || (isTrainer && IncludeTrainerCards) || (isEnergy && IncludeEnergyCards));
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
                includeOnlyThoseInDeck: IncludeOnlyThoseInDeck,
                includePokemonCards: IncludePokemonCards,
                includeTrainerCards: IncludeTrainerCards,
                includeEnergyCards: IncludeEnergyCards
                );
        }

        internal CardSifter WithNewSearchString(string text)
        {
            return new CardSifter(
                typesToInclude: TypesToInclude,
                searchString: text,
                includeOnlyThoseInDeck: IncludeOnlyThoseInDeck,
                includePokemonCards: IncludePokemonCards, 
                includeTrainerCards: IncludeTrainerCards, 
                includeEnergyCards: IncludeEnergyCards
                );
        }

        internal CardSifter IncludeOnlyCardsFromDeck(bool includeOnlyThoseFromDeck)
        {
            return new CardSifter(
                typesToInclude: TypesToInclude,
                searchString: SearchString,
                includeOnlyThoseInDeck: includeOnlyThoseFromDeck,
                includePokemonCards: IncludePokemonCards,
                includeTrainerCards: IncludeTrainerCards,
                includeEnergyCards: IncludeEnergyCards
                );
        }

        internal CardSifter WithPokemonIncluded(bool include)
        {
            return new CardSifter(
                typesToInclude: TypesToInclude,
                searchString: SearchString,
                includeOnlyThoseInDeck: IncludeOnlyThoseInDeck,
                includePokemonCards: include,
                includeTrainerCards: IncludeTrainerCards,
                includeEnergyCards: IncludeEnergyCards
                );
        }

        internal CardSifter WithTrainersIncluded(bool include)
        {
            return new CardSifter(
                typesToInclude: TypesToInclude,
                searchString: SearchString,
                includeOnlyThoseInDeck: IncludeOnlyThoseInDeck,
                includePokemonCards: IncludePokemonCards,
                includeTrainerCards: include,
                includeEnergyCards: IncludeEnergyCards
                );
        }

        internal CardSifter WithEnergiesIncluded(bool include)
        {
            return new CardSifter(
                typesToInclude: TypesToInclude,
                searchString: SearchString,
                includeOnlyThoseInDeck: IncludeOnlyThoseInDeck,
                includePokemonCards: IncludePokemonCards,
                includeTrainerCards: IncludeTrainerCards,
                includeEnergyCards: include
                );
        }

    }

}
