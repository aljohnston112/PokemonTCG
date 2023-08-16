
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_37
    {

        internal static bool Double_Kick_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Double_Kick_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Horn_Drill_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Horn_Drill_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
