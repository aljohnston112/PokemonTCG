using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PokemonTCG.DataSources;
using PokemonTCG.Models;
using PokemonTCG.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTCG.ViewModel
{
    class TitlePageViewModel
    {
        private Task loadTask;

        /// <summary>
        /// Starts a task that loads the decks and cards from disk.
        /// </summary>
        internal void OnInit()
        {
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string setFolder = baseFolder + @"Assets\sets\";
            loadTask = Load(setFolder);

        }


        /// <summary>
        /// A Task that loads cards and decks from disk.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static async Task Load(string folder)
        {
            await CardDataSource.LoadSets(folder);
            await PokemonDeck.LoadDecks();

        }

        /// <summary>
        /// Waits for loading to complete, then navigates to the deck list page.
        /// </summary>
        /// <param name="frame">The frame used for naviagation.</param>
        internal async void ButtonDeckListClicked(Frame frame)
        {
            await loadTask;
            frame.Navigate(typeof(DeckListPage));
        }


        /// <summary>
        /// Waits for loading to complete, then navigates to the settings page.
        /// </summary>
        /// <param name="frame">The frame used for naviagation.</param>
        internal static void ButtonGameSettingsListClicked(Frame frame)
        {
            frame.Navigate(typeof(GameSettingsPage));
        }

    }

}
