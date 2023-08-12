using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

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

        internal static void ShowImageFlyout(string imagePath, FrameworkElement element)
        {
            Flyout flyout = new();
            FlyoutPresenter flyoutPresenter = new();
            Image image = new()
            {
                Source = new BitmapImage(new Uri(FileUtil.GetFullPath(imagePath))),
            };
            flyoutPresenter.Content = image;
            flyout.Content = flyoutPresenter;
            Flyout.SetAttachedFlyout(element, flyout);
            Flyout.ShowAttachedFlyout(element);
            Flyout.SetAttachedFlyout(element, null);
        }

    }

}
