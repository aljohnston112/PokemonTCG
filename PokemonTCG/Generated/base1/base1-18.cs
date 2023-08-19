
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_18
    {

        internal static bool Slam_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Slam_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Hyper_Beam_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Hyper_Beam_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
