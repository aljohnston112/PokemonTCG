
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_54
    {

        internal static bool Stiffen_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Stiffen_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Stun_Spore_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Stun_Spore_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
