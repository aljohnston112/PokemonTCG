using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using Windows.Foundation;

namespace PokemonTCG.View
{
    internal class CardItemView
    {

        internal readonly string Id;
        internal readonly BitmapImage Image = new();
        internal readonly NumberBox NumberBox = new();
        private bool handlerAttached = false;

        internal CardItemView(CardItem card)
        {
            Id = card.Id;
            _ = CreateBitmap(card.ImagePath);

            NumberBox.Value = 0;
            int limit = card.Limit;
            if (limit == -1)
            {
                NumberBox.Maximum = 60;
            }
            else
            {
                NumberBox.Maximum = limit;
            }
            NumberBox.Minimum = 0;
            NumberBox.SmallChange = 1;
            NumberBox.SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline;
        }

        private async Task CreateBitmap(string url)
        {
            Image.SetSource(await ImageLoader.OpenImage(url));
        }

        internal void SetNumberBoxHandler(TypedEventHandler<NumberBox, NumberBoxValueChangedEventArgs> handler)
        {
            if (!handlerAttached)
            {
                NumberBox.ValueChanged += handler;
                handlerAttached = true;
            }
        }

        internal void SetCount(int count)
        {
            NumberBox.Value = count;
        }
    }

}
