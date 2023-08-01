import androidx.compose.foundation.layout.padding
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Window
import androidx.compose.ui.window.application

@Composable
fun App(gameState: GameState) {
//    UI.askUserToPickFromCards(
//        "Pick a Card",
//        listOf(BaseSet.cards[0]),
//        CardState(null)
//    )
//    UI.playingField(gameState)
    val deckBuilderState = DeckBuilderState()
    deckBuilder(deckBuilderState, modifier = Modifier.padding(4.dp))
}

fun main() = application {
    val prizes = listOf(
        BaseSet.cards[0],
        BaseSet.cards[0],
        BaseSet.cards[0],
        BaseSet.cards[0],
        BaseSet.cards[0],
        BaseSet.cards[0],
    )
    val gameState = GameState(
        prizes,
        prizes,
        prizes,
        prizes
    )
    gameState.opponentState.moveFromHandToBench(BaseSet.cards[0])
    gameState.opponentState.flipOverBenchedPokemon(0)
    Window(onCloseRequest = ::exitApplication) {
        App(gameState)
    }
}
