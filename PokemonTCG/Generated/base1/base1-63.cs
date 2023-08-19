
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_63
    {

        internal static bool Bubble_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Bubble_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Withdraw_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Withdraw_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
