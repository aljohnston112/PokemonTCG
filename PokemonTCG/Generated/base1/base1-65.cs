
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_65
    {

        internal static bool Slap_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Slap_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
