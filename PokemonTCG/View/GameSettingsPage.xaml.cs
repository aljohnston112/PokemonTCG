using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PokemonTCG.ViewModel;

namespace PokemonTCG.View
{
    /// <summary>
    /// The UI for setting up a game.
    /// </summary>
    public sealed partial class GameSettingsPage : Page
    {

        private readonly DeckListViewmodel ViewModel = new();

        public GameSettingsPage()
        {
            this.InitializeComponent();
            ViewModel.LoadDecks();
        }

        private void StartGameEvent(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(
                typeof(GamePage),
                new GameArguments(
                    PlayerDeckComboBox.SelectedItem as string, 
                    OpponentDeckComboBox.SelectedItem as string
                    )
                );
        }

        private void CancelGameEvent(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
