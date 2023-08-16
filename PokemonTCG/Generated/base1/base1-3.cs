
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_3
    {

        internal static bool Scrunch_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Scrunch_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Double_edge_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Double_edge_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
