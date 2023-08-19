using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using PokemonTCG.ViewModel;

using System.ComponentModel;

namespace PokemonTCG.View
{
    public sealed partial class CardPickerPage: Page
    {
        private readonly CardPickerViewModel<object> ViewModel = new();

        public CardPickerPage()
        {
            InitializeComponent();
        }

        internal void SetArgs<T>(CardPickerPageArgs<T> args)
        {
            if(args.NumberToPick == 1)
            {
                CardGridView.SelectionMode = ListViewSelectionMode.Single;
            }
            else
            {
                CardGridView.SelectionMode = ListViewSelectionMode.Multiple;
            }
            ViewModel.SetArgs(args);
            ViewModel.PropertyChanged += CanSubmit;
        }

        private void CanSubmit(object sender, PropertyChangedEventArgs e)
        {
            SubmitButton.Visibility = Visibility.Visible;
        }

        private void CardSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.NumberOfCards == 1)
            {
                ViewModel.NewCardSelected(CardGridView.SelectedIndex);
            } else
            {
                if (CardGridView.SelectedItems.Count <= ViewModel.NumberOfCards)
                {
                    ViewModel.NewCardsSelected(CardGridView.SelectedItems);
                } else
                {
                    CardGridView.SelectedItems.RemoveAt(CardGridView.SelectedItems.Count - 1);
                }
            }
        }

        private void SubmitSelected(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.SubmitSelection();
        }

    }

}
