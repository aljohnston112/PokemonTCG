
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_3
    {

        internal static bool Scrunch_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Scrunch_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Double_edge_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Double_edge_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
