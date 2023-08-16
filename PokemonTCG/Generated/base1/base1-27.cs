
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_27
    {

        internal static bool Leek_Slap_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Leek_Slap_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Pot_Smash_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            return canUse;
        }

        internal static GameState Pot_Smash_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
