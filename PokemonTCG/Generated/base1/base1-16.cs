
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_16
    {

        internal static bool Thunder_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thunder_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Thunderbolt_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thunderbolt_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
