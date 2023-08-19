using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PokemonTCG.CardModels;
using PokemonTCG.Enums;
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
                                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(card.Id),
                                card.Attacks,
                                card.Abilities.Select(ability => ability.Name).ToImmutableList(),
                                null
                                );
                        }
                        else if(card.Supertype == CardSupertype.TRAINER)
                        {
                            fileContent = GenerateCardFileContent(
                                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(card.Id),
                                ImmutableList.Create<Attack>(),
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
            IImmutableList<Attack> attacks,
            IImmutableList<string> abilities,
            string trainer
            )
        {
            string template = @"
using System;
using PokemonTCG.CardModels;
using PokemonTCG.Models;
using PokemonTCG.States;

namespace PokemonTCG.Generated 
{
    
    internal class " + cardClassName.Replace("-", "_").Replace(" ", "_") + @"
    {
";

            foreach (Attack attack in attacks)
            {
                string attackName = attack.Name.Replace("-", "_").Replace(" ", "_");
                template += @"
        internal static bool " + attackName + @"_CanUse(GameState gameState, Attack attack)
        {
            bool canUse = gameState.CurrentPlayersActiveCanUseAttack(attack);
            throw new NotImplementedException();
            return canUse;
        }

        internal static GameState " + attackName + @"_Use(GameState gameState, Attack attack)
        {
            throw new NotImplementedException();
        }
";
            }

            foreach (string abilityName in abilities)
            {
                string newAbilityName = abilityName.Replace("-", "_").Replace(" ", "_");
                template += @"
        internal static bool " + newAbilityName + @"_CanUse(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }

        internal static GameState " + newAbilityName + @"_Use(GameState gameState, PokemonCardState userCardState)
        {
            throw new NotImplementedException();
        }
";
            }
            if (trainer != null)
            {
                string trainerName = trainer.Replace("-", "_").Replace(" ", "_");
                template += @"
        internal static bool " + trainerName + @"_CanUse(GameState gameState)
        {
            throw new NotImplementedException();
        }

        internal static GameState " + trainerName + @"_Use(GameState gameState)
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
