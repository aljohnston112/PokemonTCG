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
        private readonly Image[] _bench = new Image[5];
        private readonly Image[] _prizes = new Image[6];
        private readonly Image _active = new Image();
        private readonly Image _deck = new Image();
        private readonly Image _discard = new Image();

        public PlayerPage()
        {
            this.InitializeComponent();

            _bench[0] = Bench1Image;
            _bench[1] = Bench2Image;
            _bench[2] = Bench3Image;
            _bench[3] = Bench4Image;
            _bench[4] = Bench5Image;

            _prizes[0] = Prize1Image;
            _prizes[1] = Prize2Image;
            _prizes[2] = Prize3Image;
            _prizes[3] = Prize4Image;
            _prizes[4] = Prize5Image;
            _prizes[5] = Prize6Image;

            _active = ActiveImage;
            _deck = DeckImage;
            _discard = DiscardImage;
        }

        /// <summary>
        /// Used to hide the attacks combo box.
        /// </summary>
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
            // TODO Probably do this when images are loaded to hand, bench, or active.
            // _active.Source = await ImageLoader.GetImage("Assets\\Decks\\0 - Base\\0 - Alakazam.png");
            BitmapImage bitmapImage = new();
            bitmapImage.SetSource(await ImageLoader.OpenImage(path));
            _active.Source = bitmapImage;
            Flyout flyout = Resources["FlyoutPickFromTo"] as Flyout;
            FlyoutBase.SetAttachedFlyout(_active, flyout);
            _active.Tapped +=  (sender, e) => { FlyoutBase.ShowAttachedFlyout((sender as FrameworkElement)); };
        }

    }

}
