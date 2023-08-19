using System.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PokemonTCG.States;
using PokemonTCG.Utilities;
using PokemonTCG.ViewModel;

using Windows.Foundation;

namespace PokemonTCG.View
{

    /// <summary>
    /// The UI for the playing field.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private readonly GamePageViewModel GamePageViewModel = new();

        private readonly Window HandWindow;
        private readonly HandPage HandPage = new();
        private readonly HandPageViewModel HandViewModel = new();

        private readonly PlayerPage PlayerPage;
        private readonly PlayerPageViewModel PlayerPageViewModel = new();

        private readonly PlayerPage OpponentPage;
        private readonly PlayerPageViewModel OpponentPageViewModel = new();

        public GamePage()
        {
            InitializeComponent();

            GamePageViewModel.PropertyChanged += OnGameStateChange;

            HandPage.SetViewModel(GamePageViewModel, HandViewModel);
            HandWindow = WindowUtil.OpenPageInNewWindow(HandPage);

            PlayerPage = PagePlayer;
            PlayerPage.SetViewModels(PlayerPageViewModel);

            OpponentPage = PageOpponent;
            OpponentPage.SetViewModels(OpponentPageViewModel);
            RotateOpponentPage();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // TODO does not work like this
            base.OnNavigatedFrom(e);
            HandWindow.Close();
        }

        private void OnGameStateChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameState")
            {
                HandViewModel.OnStateChange(GamePageViewModel);
                PlayerPageViewModel.OnStateChange(GamePageViewModel, isPlayerState: true);
                OpponentPageViewModel.OnStateChange(GamePageViewModel, isPlayerState: false);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            GameArguments gameArguments = e.Parameter as GameArguments;
            GamePageViewModel.StartGame(gameArguments);
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
