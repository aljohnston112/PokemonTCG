
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_34
    {

        internal static bool Karate_Chop_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Karate_Chop_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Submission_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Submission_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
