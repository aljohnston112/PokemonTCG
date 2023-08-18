
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_58
    {

        internal static bool Gnaw_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Gnaw_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Thunder_Jolt_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thunder_Jolt_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
