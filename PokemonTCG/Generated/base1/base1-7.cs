
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_7
    {

        internal static bool Jab_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Jab_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Special_Punch_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Special_Punch_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
