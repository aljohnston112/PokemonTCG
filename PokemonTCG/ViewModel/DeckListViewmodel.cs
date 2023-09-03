using PokemonTCG.DataSources;
using PokemonTCG.States;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace PokemonTCG.ViewModel
{
    internal class DeckListViewmodel
    {

        internal readonly ObservableCollection<string> DeckNames = new();

        internal void PopulateDeckNames()
        {
            IImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
            foreach (string deckName in decks.Keys)
            {
                DeckNames.Add(deckName);
            }
        }

    }

}