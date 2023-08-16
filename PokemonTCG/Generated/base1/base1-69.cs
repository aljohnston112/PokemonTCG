
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_69
    {

        internal static bool Poison_Sting_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Poison_Sting_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
