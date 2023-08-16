
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_35
    {

        internal static bool Tackle_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Tackle_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Flail_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Flail_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
