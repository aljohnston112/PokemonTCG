using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Windows.Graphics;
using Windows.UI.ViewManagement;

namespace PokemonTCG.View
{
    /// <summary>
    /// The UI for the game.
    /// </summary>
    public sealed partial class GamePage : Page
    {

        private readonly PlayerPage _player1Page;
        private readonly PlayerPage _player2Page;

        private readonly bool _singlePlayer = true;

        public GamePage()
        {
            this.InitializeComponent();

            _player1Page = PagePlayer1;
            _player2Page = PagePlayer2;

            RotatePlayer2();

            if (_singlePlayer)
            {
                _player2Page.HideAttacks();
            }

            showHand();

        }

        /// <summary>
        /// Shows a UI that has player 1's hand.
        /// </summary>
        private void showHand()
        {
            HandPage hand = new();
            Window window = new();
            window.Content = hand;
            ApplicationView.PreferredLaunchViewSize = new Size(480, 270);
            ApplicationView.PreferredLaunchWindowingMode = 
                ApplicationViewWindowingMode.PreferredLaunchViewSize;

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            if (appWindow != null)
            {
                appWindow.Resize(new SizeInt32(480, 270));
            }


            window.Activate();
        }

        /// <summary>
        /// Rotates player 2's field so it is facing player 1's field.
        /// </summary>
        private void RotatePlayer2()
        {
            _player2Page.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotateTransform = new RotateTransform()
            {
                CenterX = _player2Page.Width / 2,
                CenterY = _player2Page.Height / 2,
                Angle = 180
            };
            _player2Page.RenderTransform = rotateTransform;
        }

    }

}
