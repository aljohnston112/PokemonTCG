using System;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using PokemonTCG.ViewModel;
using Windows.Foundation;
using Windows.Graphics;
using Windows.UI.ViewManagement;
using WinRT.Interop;
using static PokemonTCG.ViewModel.GamePageViewModel;

namespace PokemonTCG.View
{
    public class GameArguments
    {

        public readonly string PlayerDeck;
        public readonly string OpponentDeck;

        public GameArguments(string playerDeck, string opponentDeck)
        {
            PlayerDeck = playerDeck;
            OpponentDeck = opponentDeck;
        }

    }

    /// <summary>
    /// The UI for the game.
    /// </summary>
    public sealed partial class GamePage : Page
    {

        GamePageViewModel ViewModel = new();

        private readonly PlayerPage PlayerPage;
        private readonly PlayerPage OpponentPage;
        private HandPage hand = new();

        private class GameCallbacks : IGameCallbacks
        {

            private readonly PlayerPageViewModel PlayerPageViewModel;
            private readonly PlayerPageViewModel OpponentPageViewModel;

            public GameCallbacks(
                PlayerPageViewModel playerViewModel, 
                PlayerPageViewModel opponentViewModel
                )
            {
                PlayerPageViewModel = playerViewModel;
                OpponentPageViewModel = opponentViewModel;
            }

            public void OnGameStateChanged(GameState gameState)
            {
                PlayerPageViewModel.OnStateChange(gameState.PlayerState);
                OpponentPageViewModel.OnStateChange(gameState.OpponentState);
            }

            public void OnReadyForUSerToSetUp()
            {

            }

        }

        private readonly PlayerPageViewModel PlayerPageViewModel = new();
        private readonly PlayerPageViewModel OpponentPageViewModel = new();
        private GameCallbacks Callbacks;

        public GamePage()
        {
            InitializeComponent();
            Callbacks = new(PlayerPageViewModel, OpponentPageViewModel);

            PlayerPage = PagePlayer;
            PlayerPage.SetViewModel(PlayerPageViewModel);
            OpponentPage = PageOpponent;
            OpponentPage.SetViewModel(OpponentPageViewModel);

            RotateOpponentPage();
            OpponentPage.HideAttacks();

            ShowHand();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            GameArguments gameArguments = e.Parameter as GameArguments;
            _ = ViewModel.StartGame(gameArguments, Callbacks);
        }

        private void ShowHand()
        {
            int width = 480;
            int height = 270;
            ApplicationView.PreferredLaunchViewSize = new Size(width, height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Window window = new()
            {
                Content = hand
            };
            IntPtr hWnd = WindowNative.GetWindowHandle(window);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow?.Resize(new SizeInt32(width, height));
            window.Activate();
        }

        private void RotateOpponentPage()
        {
            OpponentPage.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotateTransform = new()
            {
                CenterX = OpponentPage.Width / 2,
                CenterY = OpponentPage.Height / 2,
                Angle = 180
            };
            OpponentPage.RenderTransform = rotateTransform;
        }

    }

}
