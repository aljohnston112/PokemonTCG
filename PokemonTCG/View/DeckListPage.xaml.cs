using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using PokemonTCG.ViewModel;

using System;

namespace PokemonTCG.View
{
    /// <summary>
    /// Displays a UI that lists the user's decks and pre-made decks.
    /// Also lets the user create a new deck.
    /// </summary>
    public sealed partial class DeckListPage : Page
    {

        private readonly DeckListViewmodel ViewModel = new();

        public DeckListPage()
        {
            InitializeComponent();
            ViewModel.PopulateDeckNames();
        }

        private void CreateNewDeck(object sender, RoutedEventArgs args)
        {
            Frame.Navigate(typeof(DeckEditorPage));
        }

        private void EditDeck(object sender, ItemClickEventArgs e)
        {
            string deckName = e.ClickedItem.ToString();
            Frame.Navigate(typeof(DeckEditorPage), deckName);
        }

    }

}
