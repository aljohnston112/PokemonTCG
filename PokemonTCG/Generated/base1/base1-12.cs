
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_12
    {

        internal static bool Lure_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Lure_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Fire_Blast_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Fire_Blast_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
