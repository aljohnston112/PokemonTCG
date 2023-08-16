
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_50
    {

        internal static bool Sleeping_Gas_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Sleeping_Gas_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Destiny_Bond_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Destiny_Bond_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
