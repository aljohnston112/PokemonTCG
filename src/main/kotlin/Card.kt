/**
 * @param name The name of the card
 * @param maxHP The hp of the card divided by 10.
 * @param actions The list of pre-attack actions the card can perform.
 * @param visible Whether this card is visible to the other player
 */
class Card(
    val imagePath: String,
    val name: String,
    val maxHP: Int,
    private val actions: List<PreAttackAction>,
)