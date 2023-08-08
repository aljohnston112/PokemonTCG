using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.Utilities;

namespace PokemonTCG.View
{
    /// <summary>
    /// The UI for one player's field.
    /// </summary>
    public sealed partial class PlayerPage : Page
    {
        private readonly Image[] Bench = new Image[5];
        private readonly Image[] Prizes = new Image[6];
        private readonly Image Active = new();
        private readonly Image Deck = new();
        private readonly Image Discard = new();

        public PlayerPage()
        {
            this.InitializeComponent();

            Bench[0] = Bench1Image;
            Bench[1] = Bench2Image;
            Bench[2] = Bench3Image;
            Bench[3] = Bench4Image;
            Bench[4] = Bench5Image;

            Prizes[0] = Prize1Image;
            Prizes[1] = Prize2Image;
            Prizes[2] = Prize3Image;
            Prizes[3] = Prize4Image;
            Prizes[4] = Prize5Image;
            Prizes[5] = Prize6Image;

            Active = ActiveImage;
            Deck = DeckImage;
            Discard = DiscardImage;
        }

        public void HideAttacks()
        {
            ComboBoxAttacks.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// To be used for blowing up images in the field 
        /// </summary>
        /// <param name="path">The path to the image to display</param>
        private async void SetFlyout(string path)
        {
            BitmapImage bitmapImage = new();
            bitmapImage.SetSource(await ImageLoader.OpenImage(path));
            Active.Source = bitmapImage;
            Flyout flyout = Resources["FlyoutPickFromTo"] as Flyout;
            FlyoutBase.SetAttachedFlyout(Active, flyout);
            Active.Tapped +=  (sender, e) => { FlyoutBase.ShowAttachedFlyout((sender as FrameworkElement)); };
        }

    }

}
