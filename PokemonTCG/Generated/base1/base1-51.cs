
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_51
    {

        internal static bool Foul_Gas_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Foul_Gas_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
