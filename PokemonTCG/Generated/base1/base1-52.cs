
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_52
    {

        internal static bool Low_Kick_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Low_Kick_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
