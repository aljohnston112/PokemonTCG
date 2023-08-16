
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_34
    {

        internal static bool Karate_Chop_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Karate_Chop_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Submission_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Submission_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
