
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_10
    {

        internal static bool Psychic_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Psychic_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Barrier_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Barrier_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
