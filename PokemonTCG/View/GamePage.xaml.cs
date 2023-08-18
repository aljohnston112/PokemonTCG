using System.ComponentModel;

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

        private readonly HandPage handPage = new();
        private readonly HandPageViewModel HandViewModel = new();

        private readonly PlayerPage PlayerPage;
        private readonly PlayerPageViewModel PlayerPageViewModel = new();

        private readonly PlayerPage OpponentPage;
        private readonly PlayerPageViewModel OpponentPageViewModel = new();

        public GamePage()
        {
            InitializeComponent();

            GamePageViewModel.PropertyChanged += OnGameStateChange;

            handPage.SetViewModel(GamePageViewModel, HandViewModel);
            WindowUtil.OpenPageInNewWindow(handPage);

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
                HandViewModel.GameStateChanged(GamePageViewModel);
                PlayerPageViewModel.OnStateChange(gameState.PlayerState);
                OpponentPageViewModel.OnStateChange(gameState.OpponentState);
                
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
