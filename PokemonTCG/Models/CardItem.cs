using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PokemonTCG.Models
{

    /// <summary>
    /// The backing class for the <c>DeckEditor</c>'s <c>ListView</c>.
    /// </summary>
    public class CardItem
    {

        public readonly string Id;
        public readonly int Number;
        public readonly string Name;
        public readonly string ImagePath;
        public readonly int Limit;
        public readonly int Count;

        /// <summary>
        /// Creates a <c>CardItem</c> to be used with the <c>DeckEditorPage</c>.
        /// </summary>
        /// <param name="id">The id of the card.</param>
        /// <param name="number">The card number.</param>
        /// <param name="name">The name of the card.</param>
        /// <param name="imagePath">A path to a png of th0e Card.</param>
        /// <param name="limit">How many of this card wil be allowed in a deck. 
        ///                    -1 is used when there is no limit.</param>
        /// <param name="count">How many of this card are in the deck.</param>
        public CardItem(string id, int number, string name, string imagePath, int limit, int count)
        {
            Id = id;
            Number = number;
            Name = name;
            ImagePath = imagePath;
            Limit = limit;
            Count = count;
        }

        internal CardItem WithCount(int count)
        {
            return new CardItem(Id, Number, Name, ImagePath, Limit, count);
        }

    }

}