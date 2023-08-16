
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_13
    {

        internal static bool Water_Gun_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Water_Gun_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Whirlpool_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Whirlpool_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
