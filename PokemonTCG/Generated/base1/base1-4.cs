
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_4
    {

        internal static bool Fire_Spin_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Fire_Spin_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Energy_Burn_CanUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Energy_Burn_Use(GameState gameState)
        {
            throw new NotImplementedException();
        }

    }

}
