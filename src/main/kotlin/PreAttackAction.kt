/**
 * @param name The name of the action
 * @param frequency How many times the action can be performed.
 *                  Use 0 if it can be performed any number of times.
 * @param action The function to be called and given the game state when this action is activated.
 */
class PreAttackAction(
    private val name: String,
    private val frequency: Int,
    private val action: (gameState: GameState) -> Unit
) {

    private var timesUsed = 0

    /**
     * @param gameState The game state to perform the action on.
     * @return true if the action performable else false.
     */
    fun performAction(gameState: GameState): Boolean {
        val performable = frequency == 0 || timesUsed < frequency
        if (performable) {

            // Put before the action to prevent the action from
            // performing this action too many times.
            timesUsed += 1
            action(gameState)
        }
        return performable
    }

}