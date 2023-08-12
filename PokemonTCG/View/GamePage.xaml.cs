using System;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PokemonTCG.Models;
using PokemonTCG.ViewModel;
using Windows.Foundation;
using Windows.Graphics;
using Windows.UI.ViewManagement;
using WinRT.Interop;

namespace PokemonTCG.View
{
    internal class GameArguments
    {

        internal readonly string PlayerDeck;
        internal readonly string OpponentDeck;

        internal GameArguments(string playerDeck, string opponentDeck)
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

        private readonly GamePageViewModel GamePageViewModel;
        private readonly CardViewViewModel CardViewViewModel;


        private readonly PlayerPage PlayerPage;
        private readonly PlayerPage OpponentPage;
        private readonly HandPage handPage = new();

        private readonly PlayerPageViewModel PlayerPageViewModel = new();
        private readonly PlayerPageViewModel OpponentPageViewModel = new();
        private readonly HandViewModel HandViewModel = new();


        public GamePage()
        {
            InitializeComponent();

            GamePageViewModel = new(
                (GameState gameState) =>
                    {
                        PlayerPageViewModel.OnStateChange(gameState.PlayerState);
                        OpponentPageViewModel.OnStateChange(gameState.OpponentState);
                        HandViewModel.SetHand(gameState.PlayerState.Hand);
                    }
                );

            CardViewViewModel = new();

            PlayerPage = PagePlayer;
            PlayerPage.SetViewModel(PlayerPageViewModel);
            OpponentPage = PageOpponent;
            OpponentPage.SetViewModel(OpponentPageViewModel);

            RotateOpponentPage();
            OpponentPage.HideAttacks();

            handPage.SetViewModels(HandViewModel, CardViewViewModel);
            ShowHand();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            GameArguments gameArguments = e.Parameter as GameArguments;
            _ = GamePageViewModel.StartGame(gameArguments);
        }

        private void ShowHand()
        {
            int width = 480;
            int height = 270;
            ApplicationView.PreferredLaunchViewSize = new Size(width, height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Window window = new()
            {
                Content = handPage
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
