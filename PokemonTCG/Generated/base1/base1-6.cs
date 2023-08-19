
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_6
    {

        internal static bool Dragon_Rage_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Dragon_Rage_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Bubblebeam_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Bubblebeam_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
