using Microsoft.UI.Xaml;

namespace PokemonTCG
{
    public sealed partial class App : Application
    {

        private MainWindow m_window;

        internal App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched. 
        /// Override this method to perform application initialization and to create a new window.
        /// </summary>
        /// <param name="args">
        /// Provides event information for the Application.OnLaunched event.
        /// The Arguments and UWPLaunchActivatedEventArgs properties are not supported in Windows App SDK apps.
        /// </param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

    }

}
