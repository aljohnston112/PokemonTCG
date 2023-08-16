
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_16
    {

        internal static bool Thunder_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thunder_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Thunderbolt_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thunderbolt_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
