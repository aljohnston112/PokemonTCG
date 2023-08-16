
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_60
    {

        internal static bool Smash_Kick_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Smash_Kick_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Flame_Tail_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Flame_Tail_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
