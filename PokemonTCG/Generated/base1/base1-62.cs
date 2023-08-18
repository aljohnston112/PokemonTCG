
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_62
    {

        internal static bool Sand_attack_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Sand_attack_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
