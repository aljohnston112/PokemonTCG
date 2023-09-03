using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.States;
using PokemonTCG.Utilities;

namespace PokemonTCG.View
{
    /// <summary>
    /// The UI for one player's field.
    /// </summary>
    public sealed partial class PlayerPage : Page
    {

        PlayerPageViewModel PlayerPageViewModel;

        public PlayerPage()
        {
            InitializeComponent();
        }

        internal void SetViewModels(
            PlayerPageViewModel playerPageViewModel
            )
        {
            PlayerPageViewModel = playerPageViewModel;
            PlayerPageViewModel.PropertyChanged += OnStateChange;
        }

        private void OnStateChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PlayerState")
            {
                OnPlayerStateChanged(PlayerPageViewModel.PlayerState);
            }
            else if (e.PropertyName == "FieldCardActionState")
            {
                OnFieldCardActionStateChanged(PlayerPageViewModel);
            }
        }

        private void OnFieldCardActionStateChanged(PlayerPageViewModel playerPageViewModel)
        {
            FieldCardActionState fieldCardActionState = PlayerPageViewModel.FieldCardActionState;
            Debug.Assert(fieldCardActionState.ActiveCardActions.Count == 1);
            IImmutableDictionary<string, Action> cardActions =
                fieldCardActionState.ActiveCardActions[0].Actions;
            ActiveImage.ContextFlyout = null;
            if (cardActions.Count > 0)
            {
                ActiveImage.ContextFlyout = FlyoutUtil.CreateCommandBarFlyout(cardActions);
            }

            List<Image> benchImages = new()
                {
                    Bench1Image,
                    Bench2Image,
                    Bench3Image,
                    Bench4Image,
                    Bench5Image
                };
            int i = 0;
            while(i < 5)
            {
                benchImages[i].ContextFlyout = null;
                i++;
            }
            i = 0;
            foreach (CardActionState<PokemonCardState> actionState in fieldCardActionState.BenchCardActions)
            {
                cardActions = actionState.Actions;
                if (cardActions.Count > 0)
                {
                    benchImages[i].ContextFlyout = FlyoutUtil.CreateCommandBarFlyout(cardActions);
                }
                i++;
            }
        }

        private void OnPlayerStateChanged(PlayerState playerState)
        {
            string path = FileUtil.GetFullPath(
                playerState.Active?.PokemonCard?.ImagePath ??
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

            if (playerState.DiscardPile.Count > 0)
            {
                DiscardImage.Source = new BitmapImage(new Uri(FileUtil.GetFullPath("/Assets/CardBack.png")));
            }
            else
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
                benchImages[i].Source = new BitmapImage(new Uri(FileUtil.GetFullPath(playerState.Bench[i].PokemonCard.ImagePath)));
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

    }

}
