using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Diagnostics;
using Windows.Foundation;

namespace PokemonTCG.Models
{

    /// <summary>
    /// The backing class for the <c>DeckEditor</c>'s <c>ListView</c>
    /// <param name="Name">The card name,
    /// because you can only have 4 cards with the same name in a deck.</param>
    /// <param name="Image">A <c>BitmapImage</c> with a pic to the front of the card</param>
    /// <param name="Limit">How many of this card can be in a deck; -1 if unlimited</param>
    /// </summary>
    public class CardItem
    {

        public readonly string Id;
        public readonly string Name;
        public readonly BitmapImage Image;
        public readonly NumberBox NumberBox;
        public readonly int Limit;
        public readonly int Number;

        private bool handlerAttached = false;


        // TODO move to the ViewModel; put it in a Dictionary<string, int>
        private int _count = 0;

        /// <summary>
        /// Returns the Count used in the <c>DeckEditor</c>.
        /// </summary>
        /// <returns>The Count used to keep track of how many times this card appears in a deck</returns>
        public int GetCount()
        {
            return (int)NumberBox.Value;
        }

        /// <summary>
        /// Sets the Count of this <c>CardItem</c>.
        /// </summary>
        /// <param name="count"></param>
        /// <returns>True if the Count was updated, and false if the count is over Limit or under 0.</returns>
        public void SetCount(int count)
        {
            if (Limit != -1 && (count > Limit || count < 0 || count > 60) || (count < 0 || count > 60))
            {
                throw new ArgumentException("Count was out of range: " + count);
            }
            else
            {
                _count = count;
                NumberBox.Value = count;
            }
            AssertCount();
        }

        /// <summary>
        /// Used to assert the Limit is in range.
        /// </summary>
        private void AssertCount()
        {
            if (Limit != -1)
            {
                Debug.Assert(_count <= Limit);
            }
            Debug.Assert(_count >= 0);
        }

        internal void SetHandler(TypedEventHandler<NumberBox, NumberBoxValueChangedEventArgs> handler)
        {
            if (!handlerAttached)
            {
                NumberBox.ValueChanged += handler;
            }
            handlerAttached = true;
        }

        /// <summary>
        /// Creates a <c>CardItem</c> to be used with the <c>DeckEditorPage</c>
        /// </summary>
        /// <param name="id">The named shared between the Image and JSON file</param>
        /// <param name="name">The name of the card</param>
        /// <param name="image">The image of the card</param>
        /// <param name="limit">How many of this card wil be allowed in a deck</param>
        public CardItem(string id, string name, BitmapImage image, int limit, int number)
        {
            this.Id = id;
            this.Name = name;
            this.Image = image;
            this.Limit = limit;
            this.Number = number;
            this.NumberBox = new NumberBox();
            this.NumberBox.Value = 0;
            if(Limit == -1)
            {
                this.NumberBox.Maximum = 60;
            }
            else
            {
                this.NumberBox.Maximum = Limit;
            }
            

            this.NumberBox.SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline;
            this.NumberBox.SmallChange = 1;
        }

    }

}