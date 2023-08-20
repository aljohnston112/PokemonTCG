
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_8
    {

        internal static bool Seismic_Toss_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static bool Seismic_Toss_ShouldUse(GameState gameState, Attack attack)
        {
            bool shouldUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return shouldUse;
        }

        internal static GameState Seismic_Toss_PlayerUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static GameState Seismic_Toss_OpponentUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Strikes_Back_CanUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static bool Strikes_Back_ShouldUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Strikes_Back_PlayerUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Strikes_Back_OpponentUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

    }

}
