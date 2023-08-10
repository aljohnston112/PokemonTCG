using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace PokemonTCG.Utilities
{
    internal class FlyoutUtil
    {

        public static void ShowTextFlyout(string text, FrameworkElement element)
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

        public static void ShowImageFlyout(string imagePath, FrameworkElement element)
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
