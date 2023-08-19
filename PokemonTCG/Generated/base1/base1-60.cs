
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_60
    {

        internal static bool Smash_Kick_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Smash_Kick_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Flame_Tail_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Flame_Tail_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
