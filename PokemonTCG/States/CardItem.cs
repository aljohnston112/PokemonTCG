using PokemonTCG.Enums;

using System.Collections.Immutable;

namespace PokemonTCG.States
{

    /// <summary>
    /// The data class for the <c>DeckEditor</c>'s <c>ListView</c>.
    /// </summary>
    internal class CardItem
    {

        internal readonly string Id;
        internal readonly int Number;
        internal readonly string Name;
        internal readonly CardSupertype Supertype;
        internal readonly IImmutableList<PokemonType> Types;
        internal readonly string ImagePath;
        internal readonly int Limit;
        internal readonly int Count;


        /// <summary>
        /// Creates a <c>CardItem</c> to be used with the <c>DeckEditorPage</c>.
        /// </summary>
        /// <param name="id">The id of the card.</param>
        /// <param name="number">The card number.</param>
        /// <param name="name">The name of the card.</param>
        /// <param name="supertype">The supertype of the card.</param>
        /// <param name="types"> They types of the card if it is a pokemon card.</param>
        /// <param name="imagePath">A path to a png of th0e Card.</param>
        /// <param name="limit">How many of this card wil be allowed in a deck. 
        ///                    -1 is used when there is no limit.</param>
        /// <param name="count">How many of this card are in the deck.</param>
        internal CardItem(
            string id,
            int number,
            string name,
            CardSupertype supertype,
            IImmutableList<PokemonType> types,
            string imagePath,
            int limit,
            int count
            )
        {
            Id = id;
            Number = number;
            Name = name;
            Supertype = supertype;
            Types = types;
            ImagePath = imagePath;
            Limit = limit;
            Count = count;
        }

        internal CardItem WithCount(int count)
        {
            return new CardItem(Id, Number, Name, Supertype, Types, ImagePath, Limit, count);
        }

    }

}