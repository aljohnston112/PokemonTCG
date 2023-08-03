import androidx.compose.foundation.layout.padding
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Window
import androidx.compose.ui.window.application
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.runBlocking
import kotlinx.coroutines.withContext
import pokemon_tcg_api.APIHelper

@Composable
fun App(gameState: GameState) {

//    UI.playingField(gameState)

}

fun main() = application {

    val deckBuilderViewModel = DeckBuilderViewModel()
    deckBuilder(deckBuilderViewModel, modifier = Modifier.padding(4.dp))
    deckBuilderViewModel.state.value
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
