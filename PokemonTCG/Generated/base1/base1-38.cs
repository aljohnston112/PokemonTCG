
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_38
    {

        internal static bool Amnesia_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Amnesia_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Doubleslap_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Doubleslap_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
