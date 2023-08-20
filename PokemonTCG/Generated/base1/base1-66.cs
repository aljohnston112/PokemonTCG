
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class Base1_66
    {

        internal static bool Bind_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static bool Bind_ShouldUse(GameState gameState, Attack attack)
        {
            bool shouldUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return shouldUse;
        }

        internal static GameState Bind_PlayerUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static GameState Bind_OpponentUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static bool Poisonpowder_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static bool Poisonpowder_ShouldUse(GameState gameState, Attack attack)
        {
            bool shouldUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return shouldUse;
        }

        internal static GameState Poisonpowder_PlayerUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

        internal static GameState Poisonpowder_OpponentUse(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }

    }

}
