using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Search;
using Windows.Storage;
using PokemonTCG.Models;
using System.Linq;

namespace PokemonTCG.DataSources
{
    internal class SetDataSource
    {
        internal static readonly string setFolder = "\\Assets\\sets\\";

        private static readonly Dictionary<string, ICollection<PokemonCard>> setsToCards = new();

        /// <summary>
        /// Loads all <c>Card</c> instances.
        /// </summary>
        internal static async Task LoadSets()
        {
            StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(FileUtil.GetFullPath(setFolder));
            List<string> fileTypeFilter = new() { ".json" };
            QueryOptions queryOptions = new(CommonFileQuery.OrderByName, fileTypeFilter);
            StorageFileQueryResult query = storageFolder.CreateFileQueryWithOptions(queryOptions);
            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            foreach (StorageFile file in fileList.Where(name => name.Name != "sets.json"))
            {
                string setName = file.Name[..file.Name.IndexOf(".")];
                if (!setsToCards.ContainsKey(setName))
                {
                    ICollection<PokemonCard> cards = await CardDataSource.LoadCardsFromSet(file);
                    setsToCards.Add(setName, cards);
                }
            }
        }

        internal static async Task<ICollection<PokemonCard>> LoadSet(string setName)
        {
            if (!setsToCards.ContainsKey(setName))
            {
                await LoadSets();
            }
            return setsToCards[setName];
        }

    }

}
