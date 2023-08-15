using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PokemonTCG.ViewModel;
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
            InitializeComponent();
        }

        private async void NavigateToDeckListPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DeckListPage));
        }

        private async void NavigateToGameSettingsPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GameSettingsPage));
        }

    }

}
