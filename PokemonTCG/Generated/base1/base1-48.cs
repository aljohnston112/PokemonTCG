
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_48
    {

        internal static bool Fury_Attack_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Fury_Attack_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
