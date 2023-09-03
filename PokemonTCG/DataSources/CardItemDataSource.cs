using PokemonTCG.CardModels;
using PokemonTCG.States;
using PokemonTCG.Utilities;

using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonTCG.DataSources
{

    internal class CardItemDataSource
    {

        internal static async Task<IImmutableList<CardItem>> GetCardItemsForSet(string setName)
        {
            IImmutableList<PokemonCard> cards = await CardDataSource.LoadSet(setName);
            return cards.Select(card => CreateCardItem(card)).ToImmutableList();
        }

        private static CardItem CreateCardItem(PokemonCard card)
        {
            string name = card.Name;
            int cardLimit = DeckUtil.GetCardLimit(card);
            return new CardItem(
                id: card.Id, 
                number: card.Number, 
                name: name, 
                supertype: card.Supertype,
                types: card.Types,
                imagePath: card.ImagePath,
                limit: cardLimit, 
                count: 0
                );
        }

    }

}
