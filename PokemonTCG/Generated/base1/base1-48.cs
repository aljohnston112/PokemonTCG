
using System;
using PokemonTCG.CardModels;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_48
    {

        internal static bool Fury_Attack_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState Fury_Attack_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
