using System;
using Microsoft.UI.Xaml;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace TestPokemonTCG
{
    
    public partial class App : Application
    {

        private Window Window;

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            UnitTestClient.CreateDefaultUI();

            Window = new MainWindow();
            Window.Activate();

            UITestMethodAttribute.DispatcherQueue = Window.DispatcherQueue;
            UnitTestClient.Run(Environment.CommandLine);
        }

    }

}
