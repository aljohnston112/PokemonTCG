
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_43
    {

        internal static bool Psyshock_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Psyshock_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
