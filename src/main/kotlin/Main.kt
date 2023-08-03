import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.material.DropdownMenu
import androidx.compose.material.DropdownMenuItem
import androidx.compose.material.Text
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.style.LineHeightStyle
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Window
import androidx.compose.ui.window.application
import kotlinx.coroutines.runBlocking
import pokemon_tcg_api.APIHelper

@Composable
fun app() {
    val deckBuilderViewModel = DeckBuilderViewModel()
    // deckBuilder(deckBuilderViewModel, modifier = Modifier.padding(4.dp))

    val deckBuilderState by deckBuilderViewModel.state.collectAsState()
    deckBuilderState?.let {
        dropDown(it, it.cards.keys.elementAt(0))
    }
}


fun main() = application {
    Window(onCloseRequest = ::exitApplication) {
        app()
    }
}
