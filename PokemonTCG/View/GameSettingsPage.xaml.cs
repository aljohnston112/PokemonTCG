using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace PokemonTCG.View
{
    /// <summary>
    /// The UI for setting up a game.
    /// </summary>
    public sealed partial class GameSettingsPage : Page
    {
        public GameSettingsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the user clicks to start a game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGameEvent(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamePage));
        }

    }
}
