using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PokemonTCG.View;
using System;

namespace PokemonTCG
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.MainFrame.Navigate(typeof(TitlePage));
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).TryGoBack();
        }

        internal bool TryGoBack()
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
                return true;
            }
            return false;
        }


    }
}
