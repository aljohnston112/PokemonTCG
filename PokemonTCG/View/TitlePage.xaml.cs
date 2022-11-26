using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using PokemonTCG.DataSources;
using PokemonTCG.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokemonTCG.View
{
    /// <summary>
    /// The title page of the app.
    /// </summary>
    public sealed partial class TitlePage : Page
    {


        public TitlePage()
        {
            this.InitializeComponent();

            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string folder = baseFolder + @"Assets\Decks\0 - Base\";
            Load(folder);
        }

        private async void Load(string folder)
        {
            await CardDataSource.LoadCards(folder);
            await PokemonDeck.LoadDecks();
        }

        /// <summary>
        /// Called when the user clicks to see the deck list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeckList(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DeckListPage));
        }

        /// <summary>
        /// Called when the user clicks to start a game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGameSettings(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GameSettingsPage));
        }
    }
}
