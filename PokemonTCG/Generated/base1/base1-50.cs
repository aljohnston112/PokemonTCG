
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_50
    {

        internal static bool Sleeping_Gas_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Sleeping_Gas_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Destiny_Bond_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Destiny_Bond_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
