
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_6
    {

        internal static bool Dragon_Rage_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Dragon_Rage_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Bubblebeam_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Bubblebeam_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
