
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_13
    {

        internal static bool Water_Gun_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Water_Gun_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Whirlpool_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Whirlpool_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
