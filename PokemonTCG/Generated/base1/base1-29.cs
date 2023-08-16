
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_29
    {

        internal static bool Hypnosis_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Hypnosis_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Dream_Eater_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Dream_Eater_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
