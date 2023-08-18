
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_2
    {

        internal static bool Hydro_Pump_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Hydro_Pump_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Rain_Dance_CanUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState Rain_Dance_Use(GameState gameState)
        {
            throw new NotImplementedException();
        }

    }

}
