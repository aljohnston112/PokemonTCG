using PokemonTCG.Utilities;

using System;
using System.Reflection;

namespace PokemonTCG.DataSources
{
    internal class CardFunctionDataSource
    {

        internal static MethodInfo GetPlayerUseFunction(string cardId, string toUse)
        {
            string className = StringUtil.GetValidClassIdentifier(cardId);
            string methodName = StringUtil.MakeValidIdentifierFrom(toUse) + CardFunctionGenerator.PlayerUseFunctionSuffix;

            Type type = Type.GetType($"{CardFunctionGenerator.NamespacePath}.{className}");
            MethodInfo methodInfo = type?.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            return methodInfo;
        }

        internal static MethodInfo GetCanUseFunction(string cardId, string toUse)
        {
            string className = StringUtil.GetValidClassIdentifier(cardId);
            string methodName = StringUtil.MakeValidIdentifierFrom(toUse) + CardFunctionGenerator.CanUseFunctionSuffix;

            Type type = Type.GetType($"{CardFunctionGenerator.NamespacePath}.{className}");
            MethodInfo methodInfo = type?.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            return methodInfo;
        }

    }

}