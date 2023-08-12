using System;
using PokemonTCG.Models;

namespace PokemonTCG.ViewModel
{
    internal class CardViewViewModel
    {
        private Action<CardViewState> OnCardStateChanged;

        internal void SetOnCardStateChanged(Action<CardViewState> onCardStateChanged)
        {
            OnCardStateChanged = onCardStateChanged;
        }

        internal void SetCardViewState(CardViewState cardViewState)
        {
            OnCardStateChanged?.Invoke(cardViewState);
        }
        
    }

}
