using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Json;
using Windows.Storage;

namespace PokemonTCG.DataSources
{

    internal class CardItemDataSource
    {

        private static string baseFolder = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Gets the card items from a deck.
        /// </summary>
        /// <param name="deckFile">The deckFile</param>
        /// <returns></returns>
        internal static async IAsyncEnumerable<CardItem> GetCardItemsForSet(string setName)
        {
            string setFileName = baseFolder + "Assets/sets/" + setName + ".json";
            StorageFile setFile = await FileUtil.GetFile(setFileName);
            string jsonSetFileRoot = await FileIO.ReadTextAsync(setFile);
            JsonObject jsonSet = JsonObject.Parse(jsonSetFileRoot);
            JsonArray jsonCards = jsonSet.GetNamedArray("data");

            foreach (IJsonValue jsonCard in jsonCards)
            {
                CardItem cardItem = GetCardItem(jsonCard);
                yield return cardItem;
            }
        }

        private static CardItem GetCardItem(IJsonValue jsonCardValue)
        {

            JsonObject jsonCard = jsonCardValue.GetObject();
            String id = jsonCard.GetNamedString("id");
            PokemonCard card = CardDataSource.GetCardById(id);

            string name = card.Name;

            int cardLimit = GetCardLimit(card);

            return new CardItem(id, card.Number, name, card.ImageFileNames[ImageSize.LARGE] ,cardLimit, 0);
        }

        public static int GetCardLimit(PokemonCard card)
        {
            // Get the card limit per deck. 4 is normal
            int cardLimit = 4;
            CardSupertype type = card.Supertype;

            // Energies do not have a limit; colorless energy does though
            if (type == CardSupertype.Energy && card.Name != "Double Colorless Energy")
            {
                cardLimit = -1;
            }
            return cardLimit;
        }
    }

}
