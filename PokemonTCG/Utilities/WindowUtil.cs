using Microsoft.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;

using PokemonTCG.View;
using PokemonTCG.ViewModel;

using System;
using System.Collections.Immutable;

using Windows.Foundation;
using Windows.Graphics;
using Windows.UI.ViewManagement;

using WinRT.Interop;

namespace PokemonTCG.Utilities
{
    internal class WindowUtil
    {

        internal static void OpenCardPickerPageAndGetSelectedCards<T>(
            IImmutableList<T> cards,
            Action<IImmutableList<T>> onCardsSelected,
            int numberOfCardsToPick
            )
        {
            CardPickerPage cardPickerPage = new();
            Window window = null;

            void OnCardSelected(IImmutableList<T> cardStates)
            {
                onCardsSelected(cardStates);
                window.Close();
            }

            CardPickerPageArgs<T> args = new(
                cards,
                OnCardSelected,
                numberOfCardsToPick
                );

            cardPickerPage.SetArgs(args);
            OpenPageInNewWindow(cardPickerPage);
        }

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
            presenter.SetBorderAndTitleBar(true, true);

            window.Activate();
            return window;
        }

    }

}
