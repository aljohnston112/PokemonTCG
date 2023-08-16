
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_47
    {

        internal static bool Dig_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Dig_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Mud_Slap_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Mud_Slap_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
