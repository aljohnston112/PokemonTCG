
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_5
    {

        internal static bool Sing_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Sing_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Metronome_CanUse(GameState gameState, object[] attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack[0] as Attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Metronome_Use(GameState gameState, object[] attack)
        {
            throw new NotImplementedException();
        }

    }

}
