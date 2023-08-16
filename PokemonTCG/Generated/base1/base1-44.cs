
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_44
    {

        internal static bool Leech_Seed_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Leech_Seed_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
