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

        private readonly Task LoadAssetTask;

        public TitlePage()
        {
            InitializeComponent();
            LoadAssetTask = TitlePageViewModel.LoadAssets();
        }

        private async void NavigateToDeckListPage(object sender, RoutedEventArgs e)
        {
            await LoadAssetTask;
            Frame.Navigate(typeof(DeckListPage));
        }

        private async void NavigateToGameSettingsPage(object sender, RoutedEventArgs e)
        {
            await LoadAssetTask;
            Frame.Navigate(typeof(GameSettingsPage));
        }

    }

}
