
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_66
    {

        internal static bool Bind_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Bind_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Poisonpowder_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Poisonpowder_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
