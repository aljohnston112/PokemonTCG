
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_29
    {

        internal static bool Hypnosis_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Hypnosis_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Dream_Eater_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Dream_Eater_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
