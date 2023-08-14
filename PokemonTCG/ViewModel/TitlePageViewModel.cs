using PokemonTCG.DataSources;
using System.Threading.Tasks;

namespace PokemonTCG.ViewModel
{
    class TitlePageViewModel
    {

        /// <summary>
        /// Starts a task that loads the sets and decks from disk.
        /// </summary>
        internal static async Task LoadAssets()
        {
            await LoadSetsAndDecks();
        }

        private static async Task LoadSetsAndDecks()
        {
            await SetDataSource.LoadSets();
            await DeckDataSource.LoadDecks();
        }

    }

}
