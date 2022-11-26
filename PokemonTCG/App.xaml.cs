using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace PokemonTCG
{
    public sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        public bool TryGoBack()
        {
            return m_window.TryGoBack();
        }
            

        private MainWindow m_window;
    }
}
