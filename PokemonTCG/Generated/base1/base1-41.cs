
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_41
    {

        internal static bool Headbutt_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static bool Headbutt_ShouldUse(GameState gameState, Attack attack)
        {
            bool shouldUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return shouldUse;
        }

        internal static GameState Headbutt_PlayerUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static GameState Headbutt_OpponentUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
