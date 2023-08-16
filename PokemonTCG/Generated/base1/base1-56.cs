
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_56
    {

        internal static bool Rock_Throw_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Rock_Throw_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Harden_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Harden_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
