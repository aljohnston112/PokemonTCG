
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_2
    {

        internal static bool Hydro_Pump_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Hydro_Pump_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Rain_Dance_CanUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Rain_Dance_Use(GameState gameState)
        {
            throw new NotImplementedException();
        }

    }

}
