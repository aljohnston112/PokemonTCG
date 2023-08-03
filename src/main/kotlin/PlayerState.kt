import pokemon_tcg_api.Card

class PlayerState(
    hand: List<Card>,
    prizes: List<Card>
) {

    var active: CardState? = null
        private set

    private val hand = mutableListOf<Card>()
    private val prizes = mutableListOf<Card>()
    private val bench = mutableListOf<CardState>()

    init {
        assert(prizes.size == 6) { "There must be 6 prizes" }
        this.prizes.addAll(prizes)
        this.hand.addAll(hand)
    }

    fun moveFromHandToBench(card: Card){
        assert(bench.size < 5){ "A bench can only have 5 Pokemon" }
        assert(card in hand)
        bench.add(CardState(card))
    }

    fun flipOverBenchedPokemon(i: Int){
        bench[i].makeVisible()
    }

    fun getFaceUpBenchedPokemon(): List<CardState> {
        val cards = mutableListOf<CardState>()
        for(card in bench){
            if(card.isVisible){
                cards.add(card)
            }
        }
        return cards
    }

    fun getNumberOfBenchedPokemon(): Int {
        return bench.size
    }

    fun numberOfPrizesLeft(): Int {
        return prizes.size
    }

    fun getFaceUpPokemon(): List<CardState> {
        val cards = mutableListOf<CardState>()
        cards.addAll(getFaceUpBenchedPokemon())
        cards.add(active!!)
        return cards
    }

}