using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace PokemonTCG.View
{
    /// <summary>
    /// The UI for one player's field.
    /// </summary>
    public sealed partial class PlayerPage : Page
    {
        public PlayerPageViewModel ViewModel;
        
        public PlayerPage()
        {
            InitializeComponent();
        }

        internal void SetViewModel(PlayerPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
        }

        internal void HideAttacks()
        {
            ComboBoxAttacks.Visibility = Visibility.Collapsed;
        }

    }

}
