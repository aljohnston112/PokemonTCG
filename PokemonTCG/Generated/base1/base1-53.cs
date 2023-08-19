
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_53
    {

        internal static bool Thunder_Wave_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thunder_Wave_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Selfdestruct_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Selfdestruct_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
