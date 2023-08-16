
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_23
    {

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

        internal static bool Take_Down_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Take_Down_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
