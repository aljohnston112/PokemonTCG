using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
