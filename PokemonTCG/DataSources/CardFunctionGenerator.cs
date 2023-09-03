using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.Utilities;

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;

namespace PokemonTCG.DataSources
{
    internal class CardFunctionGenerator
    {

        private const string ROOT_FOLDER = "\\Generated\\";
        private const string AttackFunctionParameters = "(GameState gameState, Attack attack)";
        private const string AbilityFunctionParameters = "(GameState gameState, PokemonCardState userCardState)";
        private const string TrainerFunctionParameters = "(GameState gameState)";

        internal const string NamespacePath = "PokemonTCG.Generated";
        internal const string PlayerUseFunctionSuffix = "_PlayerUse";
        internal const string CanUseFunctionSuffix = "_CanUse";
        internal const string OpponentUseFunctionSuffix = "_OpponentUse";
        internal const string ShouldUseFunctionSuffix = "_ShouldUse";

        internal static async Task GenerateCardFunctions()
        {
            Directory.CreateDirectory(FileUtil.GetFullPath(ROOT_FOLDER));
            StorageFolder rootFolder = await FileUtil.GetFolder(ROOT_FOLDER);
            IImmutableDictionary<string, IImmutableList<PokemonCard>> sets = await CardDataSource.LoadSets();
            foreach ((string setName, IImmutableList<PokemonCard> cards) in sets)
            {
                StorageFolder setFolder;
                try
                {
                    setFolder = await rootFolder.CreateFolderAsync(
                    setName + "\\",
                    CreationCollisionOption.FailIfExists
                    );
                } catch(SystemException)
                {
                    setFolder = null;
                }
                if (setFolder != null)
                {
                    foreach (PokemonCard card in cards)
                    {
                        StorageFile storageFile = await setFolder.CreateFileAsync(
                            card.Id + ".cs",
                            CreationCollisionOption.ReplaceExisting
                            );
                        string fileContent = null;
                        if (card.Supertype == CardSupertype.POKéMON)
                        {
                            fileContent = GenerateCardFileContent(
                                card.Id,
                                card.Attacks.Select(attack => attack.Name).ToImmutableList(),
                                card.Abilities.Select(ability => ability.Name).ToImmutableList(),
                                null
                                );
                        }
                        else if(card.Supertype == CardSupertype.TRAINER)
                        {
                            fileContent = GenerateCardFileContent(
                                card.Id,
                                ImmutableList.Create<string>(),
                                ImmutableList.Create<string>(),
                                card.Name
                                );
                        }
                        if (fileContent != null)
                        {
                            await FileIO.WriteTextAsync(storageFile, fileContent);
                        }
                    }
                }
            }
        }

        internal static string GenerateCardFileContent(
            string cardClassName,
            IImmutableList<string> attackNames,
            IImmutableList<string> abilityNames,
            string trainerName
            )
        {
            string template = @"
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace" + NamespacePath + @"
{
    
    internal class " + StringUtil.GetValidClassIdentifier(cardClassName) + @"
    {
";

            foreach (string attack in attackNames)
            {
                string attackName = StringUtil.MakeValidIdentifierFrom(attack);
                template += @"
        internal static bool " + attackName + CanUseFunctionSuffix + AttackFunctionParameters + @"
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static bool " + attackName + ShouldUseFunctionSuffix + AttackFunctionParameters + @"
        {
            bool shouldUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return shouldUse;
        }

        internal static GameState " + attackName + PlayerUseFunctionSuffix + AttackFunctionParameters + @"
        {
            throw new NotImplementedException();
        }

        internal static GameState " + attackName + OpponentUseFunctionSuffix + AttackFunctionParameters + @"
        {
            throw new NotImplementedException();
        }
";
            }

            foreach (string abilityName in abilityNames)
            {
                string newAbilityName = StringUtil.MakeValidIdentifierFrom(abilityName);
                template += @"
        internal static bool " + newAbilityName + CanUseFunctionSuffix + AbilityFunctionParameters + @"
        {
            throw new NotImplementedException();
        }

        internal static bool " + newAbilityName + ShouldUseFunctionSuffix + AbilityFunctionParameters + @"
        {
            throw new NotImplementedException();
        }

        internal static GameState " + newAbilityName + PlayerUseFunctionSuffix + AbilityFunctionParameters + @"
        {
            throw new NotImplementedException();
        }

        internal static GameState " + newAbilityName + OpponentUseFunctionSuffix + AbilityFunctionParameters + @"
        {
            throw new NotImplementedException();
        }
";
            }
            if (trainerName != null)
            {
                string newTrainerName = StringUtil.MakeValidIdentifierFrom(trainerName);
                template += @"
        internal static bool " + newTrainerName + CanUseFunctionSuffix + TrainerFunctionParameters + @"
        {
            throw new NotImplementedException();
        }

        internal static bool " + newTrainerName + ShouldUseFunctionSuffix + TrainerFunctionParameters + @"
        {
            throw new NotImplementedException();
        }

        internal static GameState " + newTrainerName + PlayerUseFunctionSuffix + TrainerFunctionParameters + @"
        {
            throw new NotImplementedException();
        }

        internal static GameState " + newTrainerName + OpponentUseFunctionSuffix + TrainerFunctionParameters + @"
        {
            throw new NotImplementedException();
        }
";
            }

            template += @"
    }

}
";
            return template;
        }

    }

}