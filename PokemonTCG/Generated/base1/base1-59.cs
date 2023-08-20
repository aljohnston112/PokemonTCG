
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_59
    {

        internal static bool Water_Gun_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static bool Water_Gun_ShouldUse(GameState gameState, Attack attack)
        {
            bool shouldUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return shouldUse;
        }

        internal static GameState Water_Gun_PlayerUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static GameState Water_Gun_OpponentUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
