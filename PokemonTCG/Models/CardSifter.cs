using PokemonTCG.DataSources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using static System.Net.Mime.MediaTypeNames;

namespace PokemonTCG.Models
{
    /// <summary>
    /// Used to sift <c>CardItem</c>a based on criteria.
    /// </summary>
    internal class CardSifter
    {

        private List<PokemonType> _types = new();
        private bool _pokemon = true;
        private bool _trainer = true;
        private bool _energy = true;
        private bool _inDeck = false;
        private string _sifter = "";

        /// <summary>
        /// Creates a Sifter with default values.
        /// No cards are sifted by PokemonType, 
        /// whether they represent a PokemonCard, a TrainerCard, or an EnergyCard,
        /// or by whether the card name contains a string.
        /// </summary>
        public CardSifter() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types">The types to sift the cards by.
        ///                     If there are no types, then there is no sifting based on types.</param>
        /// <param name="pokemon">Sifts out PokemonCard representations if false</param>
        /// <param name="trainer">Sifts out TrainerCard representations if false</param>
        /// <param name="energy">Sifts out Energy card representations if false</param>
        /// <param name="sifter">Sift out CardItems whose Names do not contain this string</param>
        private CardSifter(
            List<PokemonType> types,
            bool pokemon,
            bool trainer,
            bool energy,
            string sifter,
            bool inDeck
            )
        {
            _types = types;
            _pokemon = pokemon;
            _trainer = trainer;
            _energy = energy;
            _sifter = sifter.ToLower();
            _inDeck = inDeck;
        }

        /// <summary>
        /// Sift through CardItems.
        /// </summary>
        /// <param name="values">The CardItems to sift</param>
        /// <returns>A Collection of CardItems that match the criteria of this CardSifter</returns>
        public Collection<CardItem> Sift(Collection<CardItem> values)
        {
            Collection<CardItem> cards = new();
            foreach (CardItem card in values)
            {
                bool isInTypes = CardTypeIsInTypes(card);
                bool isACorrectCardType = CardIsACorrectCardType(card);
                bool containsText = CardContainsText(card);
                bool isInDeck = CardIsInDeck(card);
                if (isInTypes && isACorrectCardType && containsText && isInDeck)
                {
                    cards.Add(card);
                }
            }
            return cards;
        }

        private bool CardIsInDeck(CardItem card)
        {
            return !_inDeck || card.GetCount() > 0;
        }

        /// <summary>
        /// Checks if a CardItem's Name contains _sifter.
        /// The empty string is contained in all names.
        /// </summary>
        /// <param name="card">The CardItem whose Name is to be checked</param>
        /// <returns>true if the card's Name contained </returns>
        private bool CardContainsText(CardItem card)
        {

            return _sifter == "" || card.Name.ToLower().Contains(_sifter);
        }

        /// <summary>
        /// Checks if the Card that is represented by CardItem has a System.Type that is allowed by this CardSifter.
        /// </summary>
        /// <param name="card">The CardItem that represents the Card whose System.Type is to checked</param>
        /// <returns>true if the Card represented by the CardItem has a System.Type that is allowed by this CardSifter</returns>
        private bool CardIsACorrectCardType(CardItem card)
        {
            Card c = CardDataSource.GetInstance(card.Id);
            System.Type type = c.GetType();
            bool isPokemon = (type == typeof(PokemonCard));
            bool isTrainer = (type == typeof(TrainerCard));
            bool isEnergy = (type == typeof(EnergyCard));
            Debug.Assert(isPokemon || isTrainer || isEnergy);
            return ((isPokemon && _pokemon) || (isTrainer && _trainer) || (isEnergy && _energy));
        }

        /// <summary>
        /// Checks if a card's PokemonType is in _types.
        /// An empty _types List will contain all types.
        /// </summary>
        /// <param name="card">The CardItem that corresponds to the Card whose PokemonType will be checked</param>
        /// <returns>true is the PokemonType of the Card is contained in _types</returns>
        private bool CardTypeIsInTypes(CardItem card)
        {
            bool isIn = false;
            if (_types.Count == 0)
            {
                isIn = true;
            }
            else
            {
                Card c = CardDataSource.GetInstance(card.Id);
                if (c.GetType() == typeof(PokemonCard))
                {
                    ImmutableList<PokemonType> types = (c as PokemonCard).Types;
                    foreach (PokemonType t in types)
                    {
                        if (_types.Contains(t))
                        {
                            isIn = true;
                        }
                    }
                }
                else if (c.GetType() == typeof(EnergyCard))
                {
                    if (_types.Contains((c as EnergyCard).Type))
                    {
                        isIn = true;
                    }
                }
            }
            return isIn;
        }

        /// <summary>
        /// Creates a new CardSifter with a new sift string.
        /// </summary>
        /// <param name="text">The text to be used for sifting.
        ///                    If empty, it will not be used to sift.</param>
        /// <returns></returns>
        internal CardSifter NewString(string text)
        {
            return new CardSifter(_types, _pokemon, _trainer, _energy, text, _inDeck);
        }

        /// <summary>
        /// Creates a new CardSifter that will include or exclude CardItems that represent PokemonCards.
        /// </summary>
        /// <param name="isChecked">The new CardSifter will sift PokemonCards out if false.</param>
        /// <returns></returns>
        internal CardSifter PokemonUpdate(bool isChecked)
        {
            return new CardSifter(_types, isChecked, _trainer, _energy, _sifter, _inDeck);
        }

        /// <summary>
        /// Creates a new CardSifter that will include or exclude CardItems that represent TrainerCards.
        /// </summary>
        /// <param name="isChecked">The new CardSifter will sift TrainerCards out if false.</param>
        /// <returns></returns>
        internal CardSifter TrainerUpdate(bool isChecked)
        {
            return new CardSifter(_types, _pokemon, isChecked, _energy, _sifter, _inDeck);
        }

        /// <summary>
        /// Creates a new CardSifter that will include or exclude CardItems that represent EnergyCards.
        /// </summary>
        /// <param name="isChecked">The new CardSifter will sift EnergyCards out if false.</param>
        /// <returns></returns>
        internal CardSifter EnergyUpdate(bool isChecked)
        {
            return new CardSifter(_types, _pokemon, _trainer, isChecked, _sifter, _inDeck);
        }

        /// <summary>
        /// Creates a new CardSifter that will include or exclude CardItems that have the type.
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="isChecked">Will exclude the type if this is false</param>
        /// <returns></returns>
        internal CardSifter TypeUpdate(PokemonType type, bool isChecked)
        {
            List<PokemonType> types = new();
            foreach (PokemonType t in _types)
            {
                types.Add(t);
            }
            if (isChecked)
            {
                types.Add(type);
            }
            else
            {
                types.Remove(type);
            }
            return new CardSifter(types, _pokemon, _trainer, _energy, _sifter, _inDeck);
        }

        internal CardSifter InDeckUpdate(bool value)
        {
            return new CardSifter(_types, _pokemon, _trainer, _energy, _sifter, value);
        }
    }
}
