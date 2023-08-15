using System;
using System.ComponentModel;
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

    /// <summary>
    /// The UI for the playing field.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private readonly GamePageViewModel GamePageViewModel = new();

        private readonly HandPage handPage = new();

        private readonly PlayerPage PlayerPage;
        private readonly PlayerPageViewModel PlayerPageViewModel = new();

        private readonly PlayerPage OpponentPage;
        private readonly PlayerPageViewModel OpponentPageViewModel = new();

        public GamePage()
        {
            InitializeComponent();

            GamePageViewModel.PropertyChanged += OnGameStateChange;

            handPage.SetViewModels(GamePageViewModel);
            ShowHand();

            PlayerPage = PagePlayer;
            PlayerPage.SetViewModels(PlayerPageViewModel);

            OpponentPage = PageOpponent;
            OpponentPage.SetViewModels(OpponentPageViewModel);
            RotateOpponentPage();
        }

        private void OnGameStateChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameState")
            {
                GameState gameState = GamePageViewModel.GameState;
                PlayerPageViewModel.OnStateChange(gameState.GameFieldState.PlayerState);
                OpponentPageViewModel.OnStateChange(gameState.GameFieldState.OpponentState);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            GameArguments gameArguments = e.Parameter as GameArguments;
            GamePageViewModel.StartGame(gameArguments);
        }

        private void ShowHand()
        {
            int width = 960;
            int height = 540;
            ApplicationView.PreferredLaunchViewSize = new Size(width, height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Window window = new()
            {
                Content = handPage
            };
            IntPtr handWindowHandle = WindowNative.GetWindowHandle(window);
            WindowId handWindowId = Win32Interop.GetWindowIdFromWindow(handWindowHandle);
            AppWindow handWindow = AppWindow.GetFromWindowId(handWindowId);
            handWindow?.Resize(new SizeInt32(width, height));
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
