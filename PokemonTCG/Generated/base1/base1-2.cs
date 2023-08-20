
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_2
    {

        internal static bool Hydro_Pump_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static bool Hydro_Pump_ShouldUse(GameState gameState, Attack attack)
        {
            bool shouldUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return shouldUse;
        }

        internal static GameState Hydro_Pump_PlayerUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static GameState Hydro_Pump_OpponentUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Rain_Dance_CanUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static bool Rain_Dance_ShouldUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Rain_Dance_PlayerUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Rain_Dance_OpponentUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

    }

}
