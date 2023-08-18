
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_47
    {

        internal static bool Dig_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Dig_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Mud_Slap_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Mud_Slap_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
