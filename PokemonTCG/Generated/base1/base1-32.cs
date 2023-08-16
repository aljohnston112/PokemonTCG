
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_32
    {

        internal static bool Recover_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Recover_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Super_Psy_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Super_Psy_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
