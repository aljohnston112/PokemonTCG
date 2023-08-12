namespace PokemonTCG.Models
{

    class CardViewState
    {
        internal readonly string ImagePath;
        internal readonly bool CanMakeActive;
        internal readonly bool CanAddToBench;
        internal readonly bool CanUse;
        internal readonly bool CanAttachToPokemon;

        internal CardViewState(string imagePath, bool canMakeActive, bool canAddToBench, bool canUse, bool canAttachToPokemon)
        {
            ImagePath = imagePath;
            CanMakeActive = canMakeActive;
            CanAddToBench = canAddToBench;
            CanUse = canUse;
            CanAttachToPokemon = canAttachToPokemon;
        }

    }

}
