
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_44
    {

        internal static bool Leech_Seed_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static bool Leech_Seed_ShouldUse(GameState gameState, Attack attack)
        {
            bool shouldUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return shouldUse;
        }

        internal static GameState Leech_Seed_PlayerUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static GameState Leech_Seed_OpponentUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
