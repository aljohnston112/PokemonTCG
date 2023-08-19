
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_11
    {

        internal static bool Thrash_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thrash_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Toxic_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Toxic_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
