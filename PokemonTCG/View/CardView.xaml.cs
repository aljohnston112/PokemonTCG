using System;
using System.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using PokemonTCG.ViewModel;
using static System.Net.Mime.MediaTypeNames;
using Image = Microsoft.UI.Xaml.Controls.Image;

namespace PokemonTCG.View
{
    public sealed partial class CardView : UserControl
    {

        public CardView()
        {
            InitializeComponent();
        }

        internal void SetViewModel(CardViewViewModel viewModel)
        {
            viewModel.SetOnCardStateChanged(CardViewStateUpdated);

        }

        private void CardViewStateUpdated(CardViewState cardViewState)
        {
            CardImage.Source = new BitmapImage(new Uri(FileUtil.GetFullPath(cardViewState.ImagePath)));
            CardImage.Tapped += ImageTapped;
            FlyoutBase.SetAttachedFlyout(CardImage, ImagePreviewFlyout);

            MakeActiveButton.Visibility = cardViewState.CanMakeActive ? Visibility.Visible : Visibility.Collapsed;
            AddToBenchButton.Visibility = cardViewState.CanAddToBench ? Visibility.Visible : Visibility.Collapsed;
            UseButton.Visibility = cardViewState.CanUse ? Visibility.Visible : Visibility.Collapsed;
            AttachToPokemonButton.Visibility = cardViewState.CanAttachToPokemon ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            ((Flyout.GetAttachedFlyout(sender as Image) as Flyout).Content as Image).Source = (sender as Image).Source;
            Flyout.ShowAttachedFlyout(sender as Image);
        }

    }

}
