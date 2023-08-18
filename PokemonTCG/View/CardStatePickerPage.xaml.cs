using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using PokemonTCG.ViewModel;

namespace PokemonTCG.View
{



    /// <summary>
    /// Used to select a Card for different contexts.
    /// </summary>
    public sealed partial class CardStatePickerPage : Page
    {

        private readonly CardStatePickerViewModel ViewModel = new();

        public CardStatePickerPage()
        {
            InitializeComponent();
        }

        internal void SetArgs(CardStatePickerPageArgs args)
        {
            ViewModel.SetArgs(args);
        }

        private void CardSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.NewCardSelected(CardGridView.SelectedItem);
            SubmitButton.Visibility = Visibility.Visible;
        }

        private void SubmitSelected(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.SubmitSelected();
        }

    }

}