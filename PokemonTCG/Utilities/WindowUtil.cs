using System;

using Microsoft.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;

using Windows.Foundation;
using Windows.Graphics;
using Windows.UI.ViewManagement;

using WinRT.Interop;

namespace PokemonTCG.Utilities
{
    internal class WindowUtil
    {
        internal static Window OpenPageInNewWindow(Page page)
        {
            int width = 960;
            int height = 540;
            ApplicationView.PreferredLaunchViewSize = new Size(width, height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Window window = new()
            {
                Content = page
            };

            IntPtr handWindowHandle = WindowNative.GetWindowHandle(window);
            WindowId handWindowId = Win32Interop.GetWindowIdFromWindow(handWindowHandle);
            AppWindow handWindow = AppWindow.GetFromWindowId(handWindowId);
            handWindow?.Resize(new SizeInt32(width, height));
            handWindow.IsShownInSwitchers = false;

            OverlappedPresenter presenter = handWindow.Presenter as OverlappedPresenter;
            presenter.SetBorderAndTitleBar(true, false);

            window.Activate();
            return window;
        }

    }

}
