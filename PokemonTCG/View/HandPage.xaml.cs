using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using PokemonTCG.ViewModel;
using static PokemonTCG.Models.HandCardActionState;

namespace PokemonTCG.View
{
    /// <summary>
    /// The UI for the player's hand during a game
    /// </summary>
    public sealed partial class HandPage : Page
    {

        private GamePageViewModel GameViewModel;
        private HandViewModel HandViewModel = new();

        public HandPage()
        {
            InitializeComponent();
        }

        internal void SetViewModels(GamePageViewModel gamePageViewModel)
        {
            GameViewModel = gamePageViewModel;
            GameViewModel.PropertyChanged += GameStateChanged;

            HandViewModel.PropertyChanged += HandChanged;
        }

        private void GameStateChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameState")
            {
                HandViewModel.GameStateChanged(GameViewModel.GameState);
            }
        }

        private void HandChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HandCardActionState")
            {
                HandGrid.ColumnDefinitions.Clear();
                HandGrid.Children.Clear();
                int i = 0;
                foreach (CardActionState actionState in HandViewModel.HandCardActionState.HandActions)
                {
                    Image image = new()
                    {
                        Source = new BitmapImage(new Uri(FileUtil.GetFullPath(actionState.Card.ImagePaths[Enums.ImageSize.LARGE])))
                    };
                    image.Tapped += FlyoutUtil.ImageTapped;

                    Flyout flyout = Resources["ImagePreviewFlyout"] as Flyout;
                    FlyoutBase.SetAttachedFlyout(image, flyout);

                    IImmutableDictionary<string, CardFunction> cardActions = actionState.Actions;
                    if (cardActions.Count > 0)
                    {
                        Dictionary<string, TappedEventHandler> commands = new();
                        foreach ((string command, CardFunction function) in cardActions)
                        {
                            if(command == MAKE_ACTIVE_ACTION)
                            {
                                commands[MAKE_ACTIVE_ACTION] = new TappedEventHandler(
                                        (object sender, TappedRoutedEventArgs e) =>
                                        GameViewModel.UpdateGameState(function.Invoke(GameViewModel.GameState, actionState.Card)));
                            } else if (command == PUT_ON_BENCH_ACTION)
                            {
                                commands[PUT_ON_BENCH_ACTION] = new TappedEventHandler(
                                        (object sender, TappedRoutedEventArgs e) =>
                                        GameViewModel.UpdateGameState(function.Invoke(GameViewModel.GameState, actionState.Card)));
                            } else if(command == USE_ACTION)
                            {
                                commands[USE_ACTION] = new TappedEventHandler(
                                        (object sender, TappedRoutedEventArgs e) =>
                                        GameViewModel.UpdateGameState(function.Invoke(GameViewModel.GameState)));
                            }
                        }
                        image.ContextFlyout = FlyoutUtil.CreateCommandBarFlyout(commands.ToImmutableDictionary());
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
        }

    }

}
