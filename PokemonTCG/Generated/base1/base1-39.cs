
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_39
    {

        internal static bool Conversion_1_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Conversion_1_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Conversion_2_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Conversion_2_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
