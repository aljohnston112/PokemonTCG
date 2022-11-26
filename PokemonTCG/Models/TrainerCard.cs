namespace PokemonTCG.Models
{
    /// <summary>
    /// To be used in the <c>GamePage</c>
    /// </summary>
    class TrainerCard : Card
    {

        /// <summary>
        /// Creates a <c>TrainerCard</c>.
        /// </summary>
        /// <param name="imageFileName">The card image's url</param>
        /// <param name="id">The unique id of the card</param>
        /// <param name="name">The name of the card</param>
        public TrainerCard(
            string imageFileName,
            string id,
            string name
        ) : base(imageFileName, id, name) { }

    }
}
