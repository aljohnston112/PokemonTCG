
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_55
    {

        internal static bool Horn_Hazard_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Horn_Hazard_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
