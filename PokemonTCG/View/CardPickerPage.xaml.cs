using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using PokemonTCG.ViewModel;

namespace PokemonTCG.View
{
    public sealed partial class CardPickerPage : Page
    {
        private readonly CardPickerViewModel ViewModel = new();

        public CardPickerPage()
        {
            InitializeComponent();
        }

        internal void SetArgs(CardPickerPageArgs args)
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
