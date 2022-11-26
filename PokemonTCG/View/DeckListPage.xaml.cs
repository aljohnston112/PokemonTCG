using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PokemonTCG.Models;
using PokemonTCG.ViewModel;
using System;
using System.Collections.Immutable;

namespace PokemonTCG.View
{
    /// <summary>
    /// Displays a UI that lists the user's decks.
    /// Also lets the user create a new deck.
    /// </summary>
    public sealed partial class DeckListPage : Page
    {

        private ViewModelDeckListPage _viewModel = new();
        public DeckListPage()
        {
            InitializeComponent();
            _viewModel.GetDecks();
        }



        /// <summary>
        /// For when the user clicks to make a new deck.
        /// </summary>
        /// <param name="sender">The sender of the click event</param>
        /// <param name="args">The args for the click event</param>
        private void CreateNewDeckEvent(object sender, RoutedEventArgs args)
        {
            this.Frame.Navigate(typeof(DeckEditorPage));
        }

    }

}
