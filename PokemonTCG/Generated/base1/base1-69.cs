
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_69
    {

        internal static bool Poison_Sting_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Poison_Sting_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
