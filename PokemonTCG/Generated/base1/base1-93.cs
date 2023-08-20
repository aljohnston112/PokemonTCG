
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_93
    {

        internal static bool Gust_of_Wind_CanUse(GameState gameState)
        {
            return gameState.CurrentOpponentState().Bench.Count > 0;
        }

        internal static bool Gust_of_Wind_ShouldUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Gust_of_Wind_PlayerUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Gust_of_Wind_OpponentUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

    }

}
