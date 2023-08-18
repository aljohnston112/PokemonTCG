using System;
using System.Collections.Immutable;
using System.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using PokemonTCG.ViewModel;

namespace PokemonTCG.View
{
    /// <summary>
    /// The UI for the player's hand during a game
    /// </summary>
    public sealed partial class HandPage : Page
    {

        private GamePageViewModel GamePageViewModel;
        private HandPageViewModel HandViewModel;

        public HandPage()
        {
            InitializeComponent();

        }

        internal void SetViewModel(
            GamePageViewModel gamePageViewModel,
            HandPageViewModel handViewModel
            )
        {
            GamePageViewModel = gamePageViewModel;
            HandViewModel = handViewModel;
            HandViewModel.PropertyChanged += HandChanged;
        }

        private void HandChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HandCardActionState")
            {
                HandGrid.ColumnDefinitions.Clear();
                HandGrid.Children.Clear();
                int i = 0;
                foreach (CardActionState<PokemonCard> actionState in HandViewModel.HandCardActionState.HandActions)
                {
                    Image image = new()
                    {
                        Source = new BitmapImage(new Uri(FileUtil.GetFullPath(actionState.Card.ImagePath)))
                    };
                    FlyoutUtil.SetImageTappedFlyout(Resources, image);

                    IImmutableDictionary<string, TappedEventHandler> cardActions = actionState.Actions;
                    if (cardActions.Count > 0)
                    {
                        image.ContextFlyout = FlyoutUtil.CreateCommandBarFlyout(cardActions);
                    }
                    Grid.SetColumn(image, i);
                    ColumnDefinition columnDefinition = new()
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    };
                    HandGrid.ColumnDefinitions.Add(columnDefinition);
                    HandGrid.Children.Add(image);
                    i++;
                }
            }
            else if(e.PropertyName == "ShowStartGameButton")
            {
                if (HandViewModel.ShowStartGameButton)
                {
                    StartGameButton.Visibility = Visibility.Visible;
                } else
                {
                    StartGameButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void StartGame(object sender, TappedRoutedEventArgs e)
        {
            GamePageViewModel.OnUsersFirstTurnSetUp();
        }

    }

}
