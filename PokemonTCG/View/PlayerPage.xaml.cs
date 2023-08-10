using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using PokemonTCG.Models;
using PokemonTCG.Utilities;

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

            void onPlayerStateChanged(PlayerState playerState)
            {
                string path= FileUtil.GetFullPath(playerState.Active?.ImageFileNames[ImageSize.LARGE] ?? "/Assets/BlankCard2.png");
                Uri uri = new Uri(path);
                ActiveImage.Source = new BitmapImage(uri);

                if (playerState.Deck.Count > 0)
                {
                    DeckImage.Source = new BitmapImage(new Uri(FileUtil.GetFullPath("/Assets/CardBack.png")));
                }
                else
                {
                    DeckImage.Source = new BitmapImage(new Uri(FileUtil.GetFullPath("/Assets/BlankCard2.png")));
                }

                if(playerState.DiscardPile.Count > 0)
                {
                    DiscardImage.Source = new BitmapImage(new Uri(FileUtil.GetFullPath("/Assets/CardBack.png")));
                } else
                {
                    DiscardImage.Source = new BitmapImage(new Uri(FileUtil.GetFullPath("/Assets/BlankCard2.png")));
                }

                List<Image> benchImages = new()
                {
                    Bench1Image,
                    Bench2Image,
                    Bench3Image,
                    Bench4Image,
                    Bench5Image
                };

                for (int i = 0; i < playerState.Bench.Count; i++)
                {
                    benchImages[i].Source = new BitmapImage(new Uri(FileUtil.GetFullPath(playerState.Bench[i].ImageFileNames[ImageSize.LARGE])));
                }
                for (int i = playerState.Bench.Count; i < benchImages.Count; i++)
                {
                    benchImages[i].Source = new BitmapImage(new Uri(FileUtil.GetFullPath("/Assets/BlankCard2.png")));
                }

                List<Image> prizeImages = new()
                {
                    Prize1Image,
                    Prize2Image,
                    Prize3Image,
                    Prize4Image,
                    Prize5Image,
                    Prize6Image
                };

                for (int i = 0; i < playerState.Prizes.Count; i++)
                {
                    prizeImages[i].Source = new BitmapImage(new Uri(FileUtil.GetFullPath("/Assets/CardBack.png")));
                }
                for (int i = playerState.Prizes.Count; i < prizeImages.Count; i++)
                {
                    prizeImages[i].Source = new BitmapImage(new Uri(FileUtil.GetFullPath("/Assets/BlankCard2.png")));
                }

                // TODO ComboBoxActions

            }

            ViewModel.SetOnPlayerStateChanged(onPlayerStateChanged);
        }

        private void ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        internal void HideAttacks()
        {
            ComboBoxActions.Visibility = Visibility.Collapsed;
        }

    }

}
