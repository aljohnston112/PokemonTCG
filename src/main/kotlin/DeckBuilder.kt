import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material.Button
import androidx.compose.material.Text
import androidx.compose.material.TextField
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.TextRange
import androidx.compose.ui.text.input.TextFieldValue

@Composable
fun deckBuilder(
    deckBuilderState: DeckBuilderState,
    modifier: Modifier = Modifier
) {
    Column {
        cardsLeft(deckBuilderState, modifier)
        energyForAllAttacks(deckBuilderState, modifier)
        deckName(modifier)
        buttons(modifier)
    }
}

@Composable
fun deckBuilderItem(
    modifier: Modifier = Modifier
) {

}

@Composable
fun cardsLeft(
    deckBuilderState: DeckBuilderState,
    modifier: Modifier = Modifier
) {
    Row(modifier=modifier) {
        Text("Cards left: ${deckBuilderState.getCardsLeft()}")
    }
}

@Composable
fun energyForAllAttacks(
    deckBuilderState: DeckBuilderState,
    modifier: Modifier = Modifier
) {
    val energyCosts = deckBuilderState.pokemonCards
        .flatMap { it.attacks }
        .flatMap { it.energyCosts.entries }
        .groupingBy { it.key }
        .fold(0) { acc, entry -> acc + entry.value }

    Row(modifier=modifier){
        Text("Total energy costs")
        for ((energy, cost) in energyCosts){
            Column {
                Text("$cost ${energy.name} cards")
            }
        }
    }

}

@Composable
fun deckName(
    modifier: Modifier = Modifier
) {
    var text by rememberSaveable(stateSaver = TextFieldValue.Saver) {
        mutableStateOf(TextFieldValue("", TextRange(0, 7)))
    }
    Row(modifier = modifier) {
        TextField(
            value = text,
            label = { Text("Deck Name") },
            onValueChange = { text = it },
            modifier = modifier.weight(1f)
        )
    }
}

@Composable
fun buttons(
    modifier: Modifier = Modifier
) {
    Row(modifier = modifier.fillMaxSize()) {
        Button(
            content = { Text("Save") },
            onClick = { },
            modifier = modifier.weight(1f)
        )
        Button(
            content = { Text("Cancel") },
            onClick = { },
            modifier = modifier.weight(1f)
        )
    }
}