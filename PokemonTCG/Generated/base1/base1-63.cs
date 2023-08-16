
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_63
    {

        internal static bool Bubble_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Bubble_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Withdraw_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Withdraw_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
