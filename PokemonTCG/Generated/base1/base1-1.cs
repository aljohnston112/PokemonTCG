
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_1
    {

        internal static bool Confuse_Ray_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static bool Confuse_Ray_ShouldUse(GameState gameState, Attack attack)
        {
            bool shouldUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return shouldUse;
        }

        internal static GameState Confuse_Ray_PlayerUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static GameState Confuse_Ray_OpponentUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Damage_Swap_CanUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static bool Damage_Swap_ShouldUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Damage_Swap_PlayerUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Damage_Swap_OpponentUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

    }

}
