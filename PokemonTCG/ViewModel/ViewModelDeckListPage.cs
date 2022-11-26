using PokemonTCG.Models;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PokemonTCG.ViewModel
{
    internal class ViewModelDeckListPage
    {

        private readonly Collection<string> _deckNames = new();
        public readonly ObservableCollection<string> DeckNames = new();

        internal async Task GetDecks()
        {
            ImmutableDictionary<string, PokemonDeck> decks = await PokemonDeck.GetDecks();
            foreach (string name in decks.Keys)
            {
                _deckNames.Add(name);
                DeckNames.Add(name);
            }
        }
    }

}