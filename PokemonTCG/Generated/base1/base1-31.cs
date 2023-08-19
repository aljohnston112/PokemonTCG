
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_31
    {

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

        internal static bool Meditate_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Meditate_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
