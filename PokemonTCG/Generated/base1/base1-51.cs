
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_51
    {

        internal static bool Foul_Gas_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Foul_Gas_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
