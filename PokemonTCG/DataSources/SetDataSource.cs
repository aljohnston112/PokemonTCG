using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Search;
using Windows.Storage;
using PokemonTCG.Models;

namespace PokemonTCG.DataSources
{
    internal class SetDataSource
    {
        private static readonly string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string setFolder = baseFolder + @"Assets\sets\";

        private static readonly IDictionary<string, ICollection<PokemonCard>> setsToCards = new Dictionary<string, ICollection<PokemonCard>>();

        /// <summary>
        /// Loads all <c>Card</c> instances from folder.
        /// </summary>
        public static async Task LoadSets()
        {
            // Get the json files from the folder
            StorageFolder storageFolder = await FileUtil.GetFolder(setFolder);
            IList<string> fileTypeFilter = new List<string>() { ".json" };
            QueryOptions queryOptions = new(CommonFileQuery.OrderByName, fileTypeFilter);
            StorageFileQueryResult query = storageFolder.CreateFileQueryWithOptions(queryOptions);
            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            foreach (StorageFile file in fileList)
            {
                if (!file.Name.Contains("sets.json"))
                {
                    String setName = file.Name[..file.Name.IndexOf(".")];
                    if (!setsToCards.ContainsKey(setName))
                    {
                        ICollection<PokemonCard> cards = await CardDataSource.LoadCardsFromSet(file);
                        setsToCards.Add(setName, cards);
                    }
                }
            }
        }

    }

}
