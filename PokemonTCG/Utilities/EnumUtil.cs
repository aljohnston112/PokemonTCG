using System;

namespace PokemonTCG.Utilities
{
    internal class EnumUtil
    {

        internal static T Parse<T>(string value)
        {
            return (T) Enum.Parse(typeof(T), value.Replace(" ", "_"), true);
        }

    }
}
