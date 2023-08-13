namespace PokemonTCG.Models
{

    /// <summary>
    /// To be used for card context actions in the game page and the hand page.
    /// </summary>
    class CardViewState
    {
        internal readonly string ImagePath;
        internal readonly bool CanMakeActive;
        internal readonly bool CanAddToBench;
        internal readonly bool CanAttachToPokemon;
        internal readonly bool CanUse;

        internal CardViewState(
            string imagePath, 
            bool canMakeActive, 
            bool canAddToBench,
            bool canAttachToPokemon,
            bool canUse
            )
        {
            ImagePath = imagePath;
            CanMakeActive = canMakeActive;
            CanAddToBench = canAddToBench;
            CanAttachToPokemon = canAttachToPokemon;
            CanUse = canUse;
        }

    }

}
