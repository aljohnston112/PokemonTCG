
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_64
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

        internal static bool Star_Freeze_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Star_Freeze_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
