
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_36
    {

        internal static bool Fire_Punch_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Fire_Punch_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Flamethrower_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Flamethrower_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
