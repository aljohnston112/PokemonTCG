
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_36
    {

        internal static bool Fire_Punch_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Fire_Punch_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Flamethrower_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Flamethrower_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
