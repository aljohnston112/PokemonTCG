
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_79
    {

        internal static bool Super_Energy_Removal_CanUse(GameState gameState)
        {
            bool canUse = false;
            if (gameState.CurrentPlayerState().NumberOfEnergyOnAllPokemon() > 0 && 
                gameState.CurrentNonPlayerState().NumberOfEnergyOnAllPokemon() > 0)
            {
                canUse = true;
            }
            return canUse;
        }

        internal static bool Super_Energy_Removal_ShouldUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Super_Energy_Removal_PlayerUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Super_Energy_Removal_OpponentUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

    }

}
