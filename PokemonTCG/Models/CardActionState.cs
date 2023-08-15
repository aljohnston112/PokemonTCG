namespace PokemonTCG.Models
{

    /// <summary>
    /// To be used for card context actions in the game page and the hand page.
    /// </summary>
    class CardActionState
    {


        internal CardActionState(GameState gameState)
        {
            if (gameState.IsPreGame)
            {

            }
        }

    }

}

/*
 * Hand Pokemon
 *      Put on bench
 *      Make Active
 * 
 * Active Pokemon
 *      Use ability
 *      Attack
 *      Evolve
 *      Retreat
 *      Attach energy
 * 
 * Bench Pokemon
 *      Use ability
 *      Evolve
 *      Make active
 *      Attach energy
 *      
 * Trainer cards
 *      Use
 */


/* 
 * Pokemon
 *      Use ability - If ability condition is met
 *          Bench
 *          Active
 *      Attack - If energy condition is met and attack condition is met
 *          Active
 *      Put on bench - "If room on bench and is a basic Pokemon" or "All 4 cards of a V-UNION (from the discard pile)".
 *          Hand
 *      Evolve - If evolution is available in hand and evolves from the Pokemon to be evolved
 *          Active
 *          Bench
 *      Retreat - If retreat cost is met
 *          Active
 *      Make active - If start of game or active was knocked out
 *          Hand
 *          Bench
 *      Attach energy - If not done
 *          Active
 *          Bench
 *          
 * Trainer - If trainer condition is met
 *      Use
 */
