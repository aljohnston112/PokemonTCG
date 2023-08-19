
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_32
    {

        internal static bool Recover_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Recover_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Super_Psy_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Super_Psy_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
