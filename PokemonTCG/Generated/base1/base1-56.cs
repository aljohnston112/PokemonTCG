
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_56
    {

        internal static bool Rock_Throw_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Rock_Throw_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Harden_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Harden_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
