
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_49
    {

        internal static bool Pound_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Pound_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Confuse_Ray_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Confuse_Ray_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
