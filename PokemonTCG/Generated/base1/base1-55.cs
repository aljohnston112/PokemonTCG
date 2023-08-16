
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_55
    {

        internal static bool Horn_Hazard_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Horn_Hazard_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
