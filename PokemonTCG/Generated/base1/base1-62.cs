
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_62
    {

        internal static bool Sand_attack_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Sand_attack_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
