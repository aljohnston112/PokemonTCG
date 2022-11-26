namespace PokemonTCG.Models
{

    // TODO perhaps use this instead of fetching instances to get the type?
/*
    public enum CardType
    {
        Pokemon, 
        Trainer,
        Energy
    }
*/

    /// <summary>
    /// To be used in the GamePage.
    /// </summary>
    public abstract class Card
    {
        public readonly string ImageFileName;
        public readonly string Id;
        public readonly string Name;

        /// <summary>
        /// Creates a <c>Card</c> to be used in the GamePage.
        /// </summary>
        /// <param name="imageFileName">The card image's url</param>
        /// <param name="id">The unique id of the card</param>
        /// <param name="name">The name of the card</param>
        public Card(string imageFileName, string id, string name)
        {
            this.ImageFileName = imageFileName;
            this.Id = id;
            this.Name = name;
        }

        
    }
}
