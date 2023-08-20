
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_4
    {

        internal static bool Fire_Spin_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static bool Fire_Spin_ShouldUse(GameState gameState, Attack attack)
        {
            bool shouldUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return shouldUse;
        }

        internal static GameState Fire_Spin_PlayerUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static GameState Fire_Spin_OpponentUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Energy_Burn_CanUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static bool Energy_Burn_ShouldUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Energy_Burn_PlayerUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Energy_Burn_OpponentUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

    }

}
