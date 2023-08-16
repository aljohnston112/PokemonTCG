
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_15
    {

        internal static bool Solarbeam_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Solarbeam_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Energy_Trans_CanUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Energy_Trans_Use(GameState gameState)
        {
            throw new NotImplementedException();
        }

    }

}
