using System.Collections.Immutable;
using System.Collections.ObjectModel;
using PokemonTCG.Enums;
using PokemonTCG.Models;

namespace PokemonTCG.ViewModel
{

    internal class HandViewModel
    {

        internal ObservableCollection<string> Images = new();

        internal void SetHand(ImmutableList<PokemonCard> hand)
        {
            Images.Clear();
            foreach (PokemonCard card in hand)
            {
                Images.Add(card.ImagePaths[ImageSize.LARGE]);
            }
        }

    }

}
