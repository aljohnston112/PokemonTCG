
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.States;
using PokemonTCG.Utilities;

namespace PokemonTCG.Generated
{

    internal class Base1_7
    {

        internal static bool Jab_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Jab_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Special_Punch_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Special_Punch_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
