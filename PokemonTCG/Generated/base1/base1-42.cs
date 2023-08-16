
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_42
    {

        internal static bool Withdraw_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Withdraw_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Bite_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Bite_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
