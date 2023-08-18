using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using PokemonTCG.Models;
using System;
using System.Resources;
using System.Collections.Immutable;

namespace PokemonTCG.Utilities
{
    internal class FlyoutUtil
    {

        internal static void ShowTextFlyout(string text, FrameworkElement element)
        {
            Flyout flyout = new();
            TextBlock textBlock = new()
            {
                Text = text
            };
            flyout.Content = textBlock;
            FlyoutBase.SetAttachedFlyout(element, flyout);
            FlyoutBase.ShowAttachedFlyout(element);
            FlyoutBase.SetAttachedFlyout(element, null);
        }

        internal static CommandBarFlyout CreateCommandBarFlyout(
            IImmutableDictionary<string, TappedEventHandler> commands
            )
        {
            CommandBarFlyout flyout = new()
            {
                AlwaysExpanded = true
            };
            foreach ((string command, TappedEventHandler tappedEventHandler) in commands)
            {
                AppBarButton button = new()
                {
                    Label = command,
                };
                button.Tapped += tappedEventHandler;
                flyout.SecondaryCommands.Add(button);
            }
            return flyout;
        }

        internal static void ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            Flyout flyout = FlyoutBase.GetAttachedFlyout(sender as Image) as Flyout;
            Image image = flyout.Content as Image;
            image.Source = (sender as Image).Source;
            FlyoutBase.ShowAttachedFlyout(sender as Image);
        }

        internal static void SetImageTappedFlyout(ResourceDictionary resources, Image image)
        {
            image.Tapped += ImageTapped;
            Flyout flyout = resources["ImagePreviewFlyout"] as Flyout;
            FlyoutBase.SetAttachedFlyout(image, flyout);
        }

    }

}
