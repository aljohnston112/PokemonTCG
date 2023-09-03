using System.Globalization;

namespace PokemonTCG.Utilities
{
    internal class StringUtil
    {
        internal static string GetValidClassIdentifier(string id)
        {
            return MakeValidIdentifierFrom(
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(id)
                );
        }

        internal static string MakeValidIdentifierFrom(string id)
        {
            return id.Replace("-", "_").Replace(" ", "_");
        }

    }

}