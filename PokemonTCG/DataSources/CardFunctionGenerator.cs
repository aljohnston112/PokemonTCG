using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PokemonTCG.Enums;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
using Windows.Storage;

namespace PokemonTCG.DataSources
{
    internal class CardFunctionGenerator
    {

        private const string ROOT_FOLDER = "\\Generated\\";

        internal static async Task GenerateCardFunctions()
        {
            // Template: SetName is the folder
            //           CardId is the file name and class name
            //           Function is of one of the forms
            //              AttackName_CanUse, 
            //              AttackName_Use, 
            //              AbilityName_CanUse, 
            //              AbilityName_Use, 
            //              TrainerName_CanUse, or
            //              TrainerName_Use
            Directory.CreateDirectory(FileUtil.GetFullPath(ROOT_FOLDER));
            StorageFolder rootFolder = await FileUtil.GetFolder(ROOT_FOLDER);
            IImmutableDictionary<string, IImmutableList<PokemonCard>> sets = await SetDataSource.LoadSets();
            foreach ((string setName, IImmutableList<PokemonCard> cards) in sets)
            {
                StorageFolder setFolder = await rootFolder.CreateFolderAsync(
                    setName + "\\",
                    CreationCollisionOption.OpenIfExists
                    );
                foreach (PokemonCard card in cards)
                {
                    StorageFile storageFile = await setFolder.CreateFileAsync(
                        card.Id + ".cs",
                        CreationCollisionOption.ReplaceExisting
                        );
                    string fileContent;
                    if (card.Supertype == CardSupertype.POKéMON)
                    {
                        fileContent = GenerateCardFileContent(
                            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(card.Id.Replace("-", "_")),
                            card.Attacks.Select(attack => attack.Name.Replace(" ", "").Replace("-", "_")).ToImmutableList(),
                            card.Abilities.Select(ability => ability.Name.Replace(" ", "").Replace("-", "_")).ToImmutableList(),
                            null
                            );
                    }
                    else
                    {
                        fileContent = GenerateCardFileContent(
                            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(card.Id.Replace("-", "_")),
                            ImmutableList.Create<string>(),
                            ImmutableList.Create<string>(),
                            card.Name.Replace(" ", "").Replace("-", "_")
                            );
                    }
                    await FileIO.WriteTextAsync(storageFile, fileContent);
                }
            }
        }

        internal static string GenerateCardFileContent(
            string cardClassName,
            IImmutableList<string> attacks,
            IImmutableList<string> abilities,
            string trainer
            )
        {
            string template = @"
using System;
using PokemonTCG.Models;

namespace PokemonTCG.Generated 
{
    
    internal class " + cardClassName + @"
    {
";

            foreach (string attackName in attacks)
            {
                template += @"
        internal static bool " + attackName + @"_CanUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState " + attackName + @"_Use(GameState gameState)
        {
            throw new NotImplementedException();
        }
";
            }

            foreach (string abilityName in abilities)
            {
                template += @"
        internal static bool " + abilityName + @"_CanUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState " + abilityName + @"_Use(GameState gameState)
        {
            throw new NotImplementedException();
        }
";
            }
            if (trainer != null)
            {

                template += @"
        internal static bool " + trainer + @"_CanUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static void " + trainer + @"_Use(GameState gameState)
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
