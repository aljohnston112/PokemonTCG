
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_54
    {

        internal static bool Stiffen_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Stiffen_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Stun_Spore_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Stun_Spore_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
