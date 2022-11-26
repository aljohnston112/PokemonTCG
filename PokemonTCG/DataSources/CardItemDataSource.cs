using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using System;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace PokemonTCG.DataSources
{
    /// <summary>
    /// Contains a static method for getting the CardItems to be diplayed by the <c>DeckEditorPage</c>.
    /// </summary>
    internal class CardItemDataSource
    {

        /// <summary>
        /// Creates a <c>CardItem</c> from a json value in the set file.
        /// </summary>
        /// <param name="element">The json value from the json array loaded from the set file</param>
        /// <param name="index">The index of the json value in the json array from the set file</param>
        /// <returns>A <c>Task</c> that will return a <c>CardItem></c></returns>
        public static async Task<CardItem> GetCard(DispatcherQueue dispatcherQueue, JsonValue element, int index)
        {

            JsonObject jObject = element.GetObject();
            String id = jObject.GetNamedString("id");
            string name = jObject.GetNamedString("name");

            // Get the card limit per deck. 4 is normal
            int limit = 4;
            string type = jObject.GetNamedString("supertype");

            // Energies do not have a limit; colorless energy does though
            if (type == "Energy" && name != "Double Colorless Energy")
            {
                limit = -1;
            }

            // Get the image url
            string url = "Assets/Decks/0 - Base/" + index + " - " + name + ".png";
            BitmapImage bitmapImage = new();
            bitmapImage.SetSource(await ImageLoader.OpenImage(url));
            CardItem cardItem = new(id, name, bitmapImage, limit);
            return cardItem;
        }

    }

}
