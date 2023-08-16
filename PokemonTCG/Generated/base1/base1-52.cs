
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_52
    {

        internal static bool Low_Kick_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Low_Kick_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
