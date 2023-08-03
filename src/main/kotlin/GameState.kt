import pokemon_tcg_api.Card
import kotlin.properties.Delegates

class GameState(
    playerHand: List<Card>,
    playerPrizes: List<Card>,
    opponentPrizes: List<Card>,
    opponentHand: List<Card>,
    ) {

    private var currentPlayer by Delegates.notNull<Int>()
    val playerState = PlayerState(playerHand, playerPrizes)
    val opponentState = PlayerState(opponentHand, opponentPrizes)

    init {
        currentPlayer = 1
    }

}