using Microsoft.UI.Xaml;
using PokemonTCG.View;

namespace PokemonTCG
{
    /// <summary>
    /// Represents the window of the current Application.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        /// <summary>
        /// Naviagates to the title page.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.MainFrame.Navigate(typeof(TitlePage));
        }

        /// <summary>
        /// If there is a previous page, then the current page is replaced with the previous page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

    }

}
