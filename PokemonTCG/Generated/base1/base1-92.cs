
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_92
    {

        internal static bool Energy_Removal_CanUse(GameState gameState)
        {
            return gameState.CurrentOpponentState().NumberOfEnergyOnAllPokemon() > 0;
        }

        internal static bool Energy_Removal_ShouldUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Energy_Removal_PlayerUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Energy_Removal_OpponentUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

    }

}
