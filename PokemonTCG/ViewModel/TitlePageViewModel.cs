using Microsoft.UI.Xaml.Controls;
using PokemonTCG.DataSources;
using PokemonTCG.Models;
using PokemonTCG.View;
using System;
using System.Threading.Tasks;

namespace PokemonTCG.ViewModel
{
    class TitlePageViewModel
    {
        private Task loadTask;

        /// <summary>
        /// Call to start a task that loads the sets and decks from disk.
        /// </summary>
        internal void OnInit()
        {
            loadTask = LoadSetsAndDecks();
        }

        private static async Task LoadSetsAndDecks()
        {
            await SetDataSource.LoadSets();
            await DeckDataSource.LoadDecks();
        }

        internal async void ButtonDeckListClicked(Frame frame)
        {
            // Need to wait for the sets and decks to load as the deck list page depends on them.
            // TODO might be a better solution.
            await loadTask;
            frame.Navigate(typeof(DeckListPage));
        }



    }

}
