
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_20
    {

        internal static bool Thundershock_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thundershock_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Thunderpunch_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Thunderpunch_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
