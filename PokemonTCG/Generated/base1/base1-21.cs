
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_21
    {

        internal static bool Electric_Shock_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Electric_Shock_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Buzzap_CanUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Buzzap_Use(GameState gameState)
        {
            throw new NotImplementedException();
        }

    }

}
