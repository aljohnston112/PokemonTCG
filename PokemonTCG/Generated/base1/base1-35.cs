
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_35
    {

        internal static bool Tackle_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Tackle_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Flail_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Flail_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
