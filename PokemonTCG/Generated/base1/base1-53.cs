
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_53
    {

        internal static bool Thunder_Wave_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thunder_Wave_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Selfdestruct_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Selfdestruct_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
