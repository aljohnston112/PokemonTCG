using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PokemonTCG.ViewModel;

namespace PokemonTCG.View
{
    /// <summary>
    /// The title page of the app.
    /// </summary>
    public sealed partial class TitlePage : Page
    {

        private readonly TitlePageViewModel titlePageViewModel = new();

        public TitlePage()
        {
            this.InitializeComponent();
            titlePageViewModel.OnInit();
        }


        /// <summary>
        /// Called when the user clicks to see the deck list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeckListClicked(object sender, RoutedEventArgs e)
        {
            titlePageViewModel.ButtonDeckListClicked(this.Frame);
        }

        /// <summary>
        /// Called when the user clicks to start a game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGameSettingsClicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GameSettingsPage));
        }

    }

}
