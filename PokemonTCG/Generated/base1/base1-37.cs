
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_37
    {

        internal static bool Double_Kick_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Double_Kick_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Horn_Drill_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Horn_Drill_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
