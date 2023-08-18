
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated
{

    internal class Base1_5
    {

        internal static bool Sing_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Sing_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Metronome_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Metronome_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
