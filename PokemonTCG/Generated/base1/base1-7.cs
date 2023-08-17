
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using PokemonTCG.Enums;
using PokemonTCG.Models;
using PokemonTCG.Utilities;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_7
    {

        internal static bool Jab_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Jab_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Special_Punch_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Special_Punch_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
