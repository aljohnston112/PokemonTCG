using PokemonTCG.DataSources;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace PokemonTCG.Models
{
    /// <summary>
    /// Used to sift <c>CardItem</c>s based on criteria.
    /// </summary>
    internal class CardSifter
    {

        private readonly ImmutableList<PokemonType> OnlyIncludeTheseTypes;
        private readonly bool IncludePokemonCards = true;
        private readonly bool IncludeTrainerCards = true;
        private readonly bool IncludeEnergyCards = true;
        private readonly bool IncludeOnlyThoseInDeck = false;
        private readonly string SearchString = "";

        /// <summary>
        /// Creates a Sifter with default values.
        /// No cards will be sifted.
        /// </summary>
        public CardSifter() {
            OnlyIncludeTheseTypes = ImmutableList.Create<PokemonType>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onlyIncludeTheseTypes">Will inlcude all types if this list is empty.</param>
        /// <param name="includePokemonCards"></param>
        /// <param name="includeTrainerCards"></param>
        /// <param name="includeEnergyCards"></param>
        /// <param name="searchString"></param>
        /// <param name="includeOnlyThoseInDeck"></param>
        private CardSifter(
            ImmutableList<PokemonType> onlyIncludeTheseTypes,
            bool includePokemonCards,
            bool includeTrainerCards,
            bool includeEnergyCards,
            string searchString,
            bool includeOnlyThoseInDeck
            )
        {
            OnlyIncludeTheseTypes = onlyIncludeTheseTypes;
            IncludePokemonCards = includePokemonCards;
            IncludeTrainerCards = includeTrainerCards;
            IncludeEnergyCards = includeEnergyCards;
            SearchString = searchString.ToLower();
            IncludeOnlyThoseInDeck = includeOnlyThoseInDeck;
        }

        /// <summary>
        /// Sift through CardItems.
        /// </summary>
        /// <param name="cardItems">The CardItems to sift.</param>
        /// <returns>A Collection of CardItems that match the criteria of this CardSifter.</returns>
        public Collection<CardItem> Sift(ICollection<CardItem> cardItems)
        {
            Collection<CardItem> matchingCardItems = new();
            foreach (CardItem card in cardItems)
            {
                bool hasMatchingType = CardTypeMatchesTypeInPokemonTypes(card);
                bool hasMatchingSupertype = CardHasMatchingSuperType(card);
                bool containsText = CardContainsText(card);
                bool isInDeck = CardIsInDeck(card);
                if (hasMatchingType && hasMatchingSupertype && containsText && isInDeck)
                {
                    matchingCardItems.Add(card);
                }
            }
            return matchingCardItems;
        }

        private bool CardIsInDeck(CardItem card)
        {
            return !IncludeOnlyThoseInDeck || card.Count > 0;
        }

        private bool CardContainsText(CardItem card)
        {

            return SearchString == "" || card.Name.ToLower().Contains(SearchString);
        }

        private bool CardHasMatchingSuperType(CardItem cardItem)
        {
            PokemonCard card = CardDataSource.GetCardById(cardItem.Id);
            CardSupertype supertype = card.Supertype;
            bool isPokemon = (supertype == CardSupertype.Pokemon);
            bool isTrainer = (supertype == CardSupertype.Trainer);
            bool isEnergy = (supertype == CardSupertype.Energy);
            return ((isPokemon && IncludePokemonCards) || (isTrainer && IncludeTrainerCards) || (isEnergy && IncludeEnergyCards));
        }

        private bool CardTypeMatchesTypeInPokemonTypes(CardItem cardItem)
        {
            bool typeMatches = false;
            if (OnlyIncludeTheseTypes.Count == 0)
            {
                typeMatches = true;
            }
            else
            {
                PokemonCard card = CardDataSource.GetCardById(cardItem.Id);

                // Get types
                ImmutableList<PokemonType> types;
                if (card.Supertype == CardSupertype.Pokemon)
                {
                    types = card.Types;

                }
                else if (card.Supertype == CardSupertype.Energy)
                {
                    PokemonType type = PokemonCard.GetEnergyType(card);
                    types = ImmutableList.Create(type);
                } else
                {
                    types = ImmutableList.Create<PokemonType>();
                }

                // Check type
                foreach (PokemonType type in types)
                {
                    if (OnlyIncludeTheseTypes.Contains(type))
                    {
                        typeMatches = true;
                    }
                }
            }
            return typeMatches;
        }

        /// <summary>
        /// Creates a new CardSifter with a new sift string.
        /// </summary>
        /// <param name="text">The text to be used for sifting.
        ///                    If empty, cards will not be sifted by name.</param>
        /// <returns></returns>
        internal CardSifter NewString(string text)
        {
            return new CardSifter(OnlyIncludeTheseTypes, IncludePokemonCards, IncludeTrainerCards, IncludeEnergyCards, text, IncludeOnlyThoseInDeck);
        }

        /// <summary>
        /// Creates a new CardSifter that will include or exclude CardItems that represent PokemonCards.
        /// </summary>
        /// <param name="isChecked">The new CardSifter will sift PokemonCards out if false.</param>
        /// <returns></returns>
        internal CardSifter IncludePokemon(bool isChecked)
        {
            return new CardSifter(OnlyIncludeTheseTypes, isChecked, IncludeTrainerCards, IncludeEnergyCards, SearchString, IncludeOnlyThoseInDeck);
        }

        /// <summary>
        /// Creates a new CardSifter that will include or exclude CardItems that represent TrainerCards.
        /// </summary>
        /// <param name="isChecked">The new CardSifter will sift TrainerCards out if false.</param>
        /// <returns></returns>
        internal CardSifter IncludeTrainer(bool isChecked)
        {
            return new CardSifter(OnlyIncludeTheseTypes, IncludePokemonCards, isChecked, IncludeEnergyCards, SearchString, IncludeOnlyThoseInDeck);
        }

        /// <summary>
        /// Creates a new CardSifter that will include or exclude CardItems that represent EnergyCards.
        /// </summary>
        /// <param name="isChecked">The new CardSifter will sift EnergyCards out if false.</param>
        /// <returns></returns>
        internal CardSifter IncludeEnergy(bool isChecked)
        {
            return new CardSifter(OnlyIncludeTheseTypes, IncludePokemonCards, IncludeTrainerCards, isChecked, SearchString, IncludeOnlyThoseInDeck);
        }

        /// <summary>
        /// Creates a new CardSifter that will include or exclude CardItems that have the type.
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="includeType">Will exclude the type if this is false</param>
        /// <returns></returns>
        internal CardSifter InludeType(PokemonType type, bool includeType)
        {
            List<PokemonType> types = new();
            foreach (PokemonType t in OnlyIncludeTheseTypes)
            {
                types.Add(t);
            }
            if (includeType)
            {
                types.Add(type);
            }
            else
            {
                types.Remove(type);
            }
            return new CardSifter(types.ToImmutableList(), IncludePokemonCards, IncludeTrainerCards, IncludeEnergyCards, SearchString, IncludeOnlyThoseInDeck);
        }

        /// <summary>
        /// Creates a new CardSifter that will include or exclude CardItems based on if they are in the deck or not.
        /// </summary>
        /// <param name="includeOnlyThoseFromDeck">Will exclude cards not in the deck if true.</param>
        /// <returns></returns>
        internal CardSifter IncludeOnlyThoseFromDeck(bool includeOnlyThoseFromDeck)
        {
            return new CardSifter(OnlyIncludeTheseTypes, IncludePokemonCards, IncludeTrainerCards, IncludeEnergyCards, SearchString, includeOnlyThoseFromDeck);
        }

    }

}
