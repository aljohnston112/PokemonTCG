using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.Enums;
using PokemonTCG.Models;
using PokemonTCG.Utilities;

namespace PokemonTCG.View
{
    /// <summary>
    /// The UI for one player's field.
    /// </summary>
    public sealed partial class PlayerPage : Page
    {

        public PlayerPage()
        {
            InitializeComponent();
        }

        internal void SetViewModels(
            PlayerPageViewModel playerPageViewModel
            )
        {

            void onPlayerStateChanged(PlayerState playerState)
            {
                string path= FileUtil.GetFullPath(
                    playerState.Active?.PokemonCard?.ImagePaths[ImageSize.LARGE] ??
                    "/Assets/BlankCard2.png"
                    );
                Uri uri = new(path);
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
                    benchImages[i].Source = new BitmapImage(new Uri(FileUtil.GetFullPath(playerState.Bench[i].PokemonCard.ImagePaths[ImageSize.LARGE])));
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
            }

            playerPageViewModel.SetOnPlayerStateChanged(onPlayerStateChanged);
        }

    }

}
