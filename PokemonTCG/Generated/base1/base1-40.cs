
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_40
    {

        internal static bool Bite_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Bite_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Super_Fang_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Super_Fang_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
