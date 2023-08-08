using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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

        public static async Task ShowImageFlyout(string imagePath, FrameworkElement element)
        {
            Flyout flyout = new();
            FlyoutPresenter flyoutPresenter = new();
            BitmapImage image = new();
            image.SetSource(await ImageLoader.OpenImage(imagePath));

            flyoutPresenter.Content = image;
            flyout.Content = flyoutPresenter;
            Flyout.SetAttachedFlyout(element, flyout);
            Flyout.ShowAttachedFlyout(element);
            Flyout.SetAttachedFlyout(element, null);
        }

    }

}
