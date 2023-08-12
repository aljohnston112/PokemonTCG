using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PokemonTCG.Utilities;
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
            InitializeComponent();
            ViewModel.LoadDecks();
        }

        private void StartGameButtonClicked(object sender, RoutedEventArgs e)
        {
            if (PlayerDeckComboBox.SelectedItem == null)
            {
                FlyoutUtil.ShowTextFlyout("A deck is needed to play.", PlayerDeckComboBox);
            }
            else if (OpponentDeckComboBox.SelectedItem == null)
            {
                FlyoutUtil.ShowTextFlyout("Your opponent needs a deck.", PlayerDeckComboBox);
            }
            else
            {
                Frame.Navigate(
                    typeof(GamePage),
                    new GameArguments(
                        PlayerDeckComboBox.SelectedItem as string,
                        OpponentDeckComboBox.SelectedItem as string
                        )
                    );
            }
        }

        private void CancelGameEvent(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

    }

}
