namespace PokemonTCG.Models
{
    /// <summary>
    /// The state of the game. To be used for the <c>GamePage</c>
    /// </summary>
    internal class GameState
    {
        internal readonly PlayerState PlayerState;
        internal readonly PlayerState OpponentState;
        internal readonly PokemonCard StadiumCard;

        internal GameState(
            PlayerState playerState, 
            PlayerState newOpponentState, 
            PokemonCard stadiumCard
            ) : this(playerState, newOpponentState)
        {
            StadiumCard = stadiumCard;
        }

        internal GameState(
            PlayerState playerState, 
            PlayerState newOpponentState
            )
        {
            PlayerState = playerState;
            OpponentState = newOpponentState;
        }

        internal GameState AddStadiumCard(PokemonCard card)
        {
            // TODO Stadium cards replace any stadium cards in play.You can't play a stadium that is already active. 
            return new GameState(PlayerState, OpponentState, card);
        }

        // TODO I need to figure out how to design the cards and their effects.
        /*
        internal GameState PlayTrainerCard(PokemonCard pokemonCard)
        {
            // TODO Supporters cannot be played on the first player's first turn.
            
        }
        */

    }

}