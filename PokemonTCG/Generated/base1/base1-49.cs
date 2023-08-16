
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_49
    {

        internal static bool Pound_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Pound_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Confuse_Ray_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Confuse_Ray_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
