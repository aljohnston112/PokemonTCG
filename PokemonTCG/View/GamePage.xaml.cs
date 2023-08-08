using System;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PokemonTCG.ViewModel;
using Windows.Foundation;
using Windows.Graphics;
using Windows.UI.ViewManagement;
using WinRT.Interop;

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


        public GamePage()
        {
            InitializeComponent();

            PlayerPage = PagePlayer;
            OpponentPage = PageOpponent;

            RotateOpponentPage();
            OpponentPage.HideAttacks();

            ShowHand();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            GameArguments gameArguments = e.Parameter as GameArguments;
            ViewModel.StartGame(gameArguments);
        }

        private void ShowHand()
        {
            int width = 480;
            int height = 270;
            ApplicationView.PreferredLaunchViewSize = new Size(width, height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            HandPage hand = new();
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
