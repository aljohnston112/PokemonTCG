using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace PokemonTCG.DataSources
{

    internal class CardItemDataSource
    {

        /// <summary>
        /// Gets the card items from a deck.
        /// </summary>
        /// <param name="deckFile">The deckFile</param>
        /// <returns></returns>
        internal static async IAsyncEnumerable<CardItem> GetCardItemsForSet(string deckFile)
        {
            StorageFile file = await FileUtil.GetFile(deckFile);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject.GetNamedArray("data");

            // Load the cards
            foreach (IJsonValue element in jsonArray)
            {
                CardItem card = await GetCardItem(element);
                yield return card;
            }
        }

        private static async Task<CardItem> GetCardItem(IJsonValue element)
        {

            JsonObject jObject = element.GetObject();
            String id = jObject.GetNamedString("id");
            PokemonCard card = CardDataSource.GetCardById(id);

            string name = card.Name;

            // Get the card limit per deck. 4 is normal
            int limit = 4;
            CardSupertype type = card.Supertype;

            // Energies do not have a limit; colorless energy does though
            if (type == CardSupertype.Energy && name != "Double Colorless Energy")
            {
                limit = -1;
            }

            // Create the Bitmap
            string url = card.ImageFileNames[ImageSize.LARGE];
            BitmapImage bitmapImage = new();
            bitmapImage.SetSource(await ImageLoader.OpenImage(url));

            return new CardItem(id, name, bitmapImage, limit);
        }

    }

}
