
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_41
    {

        internal static bool Headbutt_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Headbutt_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
