
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_10
    {

        internal static bool Psychic_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Psychic_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Barrier_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Barrier_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
