using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
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
            Flyout.SetAttachedFlyout(element, flyout);
            Flyout.ShowAttachedFlyout(element);
            Flyout.SetAttachedFlyout(element, null);
        }

        internal static CommandBarFlyout CreateCommandBarFlyout(IImmutableList<string> commands)
        {
            CommandBarFlyout flyout = new()
            {
                AlwaysExpanded = true
            };
            foreach(string command in commands)
            {
                AppBarButton button = new()
                {
                    Label = command
                };
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

    }

}
