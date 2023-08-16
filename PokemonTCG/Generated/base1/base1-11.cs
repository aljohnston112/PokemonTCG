
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_11
    {

        internal static bool Thrash_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thrash_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Toxic_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Toxic_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
