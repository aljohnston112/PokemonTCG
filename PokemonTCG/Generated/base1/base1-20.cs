
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_20
    {

        internal static bool Thundershock_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thundershock_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Thunderpunch_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thunderpunch_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
