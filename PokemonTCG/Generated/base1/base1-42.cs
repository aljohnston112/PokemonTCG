
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_42
    {

        internal static bool Withdraw_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Withdraw_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Bite_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Bite_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
