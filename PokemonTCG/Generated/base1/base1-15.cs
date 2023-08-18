
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_15
    {

        internal static bool Solarbeam_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Solarbeam_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Energy_Trans_CanUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Energy_Trans_Use(GameState gameState)
        {
            throw new NotImplementedException();
        }

    }

}
