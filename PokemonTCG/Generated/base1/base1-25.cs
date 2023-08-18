
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_25
    {

        internal static bool Aurora_Beam_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Aurora_Beam_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Ice_Beam_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Ice_Beam_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
