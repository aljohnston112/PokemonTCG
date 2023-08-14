using Microsoft.UI.Xaml.Controls;
using PokemonTCG.DataSources;
using PokemonTCG.View;
using System;
using System.Threading.Tasks;

namespace PokemonTCG.ViewModel
{
    class TitlePageViewModel
    {

        /// <summary>
        /// Call to start a task that loads the sets and decks from disk.
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
