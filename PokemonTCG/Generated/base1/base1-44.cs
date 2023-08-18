
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_44
    {

        internal static bool Leech_Seed_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Leech_Seed_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
