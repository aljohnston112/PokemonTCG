
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_27
    {

        internal static bool Leek_Slap_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Leek_Slap_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Pot_Smash_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            return canUse;
        }

        internal static GameState Pot_Smash_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
