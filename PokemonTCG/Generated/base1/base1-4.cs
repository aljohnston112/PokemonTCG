
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_4
    {

        internal static bool Fire_Spin_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Fire_Spin_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Energy_Burn_CanUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Energy_Burn_Use(GameState gameState)
        {
            throw new NotImplementedException();
        }

    }

}
