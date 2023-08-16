
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_12
    {

        internal static bool Lure_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Lure_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Fire_Blast_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Fire_Blast_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
