using System.Collections.Immutable;
using System.Collections.ObjectModel;
using PokemonTCG.Models;
using PokemonTCG.Utilities;

namespace PokemonTCG.ViewModel
{

    public class HandViewModel
    {

        public ObservableCollection<string> images = new();

        internal void SetHand(ImmutableList<PokemonCard> hand)
        {
            images.Clear();
            foreach (PokemonCard card in hand)
            {
                images.Add(card.ImageFileNames[ImageSize.LARGE]);
            }
        }

    }

}
