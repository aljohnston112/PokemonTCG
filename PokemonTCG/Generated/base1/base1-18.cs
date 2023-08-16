
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_18
    {

        internal static bool Slam_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Slam_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Hyper_Beam_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Hyper_Beam_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
