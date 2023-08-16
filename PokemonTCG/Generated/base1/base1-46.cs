
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_46
    {

        internal static bool Scratch_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Scratch_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Ember_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Ember_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
