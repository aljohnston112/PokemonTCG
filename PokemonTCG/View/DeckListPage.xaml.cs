using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PokemonTCG.ViewModel;
using System;

namespace PokemonTCG.View
{
    /// <summary>
    /// Displays a UI that lists the user's decks.
    /// Also lets the user create a new deck.
    /// </summary>
    public sealed partial class DeckListPage : Page
    {

        private readonly DeckListViewmodel ViewModel = new();
        public DeckListPage()
        {
            InitializeComponent();
            ViewModel.LoadDecks();
        }

        private void CreateNewDeckEvent(object sender, RoutedEventArgs args)
        {
            Frame.Navigate(typeof(DeckEditorPage));
        }

        private void SetUpDeckClickListener(object sender, ItemClickEventArgs e)
        {
            String deckName = e.ClickedItem.ToString();
            Frame.Navigate(typeof(DeckEditorPage), deckName);
        }

    }

}
