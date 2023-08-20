
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_21
    {

        internal static bool Electric_Shock_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static bool Electric_Shock_ShouldUse(GameState gameState, Attack attack)
        {
            bool shouldUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return shouldUse;
        }

        internal static GameState Electric_Shock_PlayerUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static GameState Electric_Shock_OpponentUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Buzzap_CanUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static bool Buzzap_ShouldUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Buzzap_PlayerUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Buzzap_OpponentUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

    }

}
