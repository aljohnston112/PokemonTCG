
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_58
    {

        internal static bool Gnaw_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Gnaw_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Thunder_Jolt_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thunder_Jolt_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
