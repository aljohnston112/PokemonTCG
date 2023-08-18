
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_22
    {

        internal static bool Whirlwind_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Whirlwind_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Mirror_Move_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Mirror_Move_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
