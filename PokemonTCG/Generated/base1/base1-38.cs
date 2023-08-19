
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_38
    {

        internal static bool Amnesia_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Amnesia_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Doubleslap_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Doubleslap_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
