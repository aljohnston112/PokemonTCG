using System;

namespace PokemonTCG.Utilities
{
    internal class CoinUtil
    {

        internal static bool FlipCoin()
        {
            return new Random().Next(2) == 0;
        }

    }
}
