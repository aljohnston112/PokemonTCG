import pokemon_tcg_api.Card

class CardState(val card: Card) {

    private var damage = 0
    private var statusConditions = listOf<Status>()
    var isVisible = false
        private set

    fun makeVisible(){
        isVisible = true
    }

    fun makeInvisible(){
        isVisible = false
    }

    /**
     * @param damage The damage to add to this card.
     */
    fun addDamage(damage: Int) {
        this.damage += damage
    }

    /**
     * @param damage The damage to remove from this card.
     */
    fun removeDamage(damage: Int) {
        this.damage -= damage
        if (this.damage < 0) {
            this.damage = 0
        }
    }

    /**
     * @return The hp this card has left.
     */
    fun healthLeft(): Int {
        return card.hp - this.damage
    }

    /**
     * @return true is this card is asleep, else false
     */
    fun isAsleep(): Boolean {
        return Status.ASLEEP in statusConditions
    }

    /**
     * @return true is this card is confused, else false
     */
    fun isConfused(): Boolean {
        return Status.CONFUSED in statusConditions
    }

    /**
     * @return true is this card is paralyzed, else false
     */
    fun isParalyzed(): Boolean {
        return Status.PARALYZED in statusConditions
    }

}