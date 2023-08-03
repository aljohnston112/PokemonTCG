import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.foundation.rememberScrollState
import androidx.compose.material.*
import androidx.compose.runtime.*
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.TextRange
import androidx.compose.ui.text.input.TextFieldValue
import androidx.compose.ui.unit.dp
import pokemon_tcg_api.Card

@Composable
fun deckBuilder(
    deckBuilderViewModel: DeckBuilderViewModel,
    modifier: Modifier = Modifier
) {
    val deckBuilderState by deckBuilderViewModel.state.collectAsState()
    Column(modifier = modifier.fillMaxSize()) {
        deckBuilderState?.let {
            cards(it, modifier.weight(1f))
            cardsLeft(it, modifier)
            energyForAllAttacks(it, modifier)
            deckName(modifier)
            buttons(modifier)
        }
    }
}

@Composable
fun cards(
    deckBuilderState: DeckBuilderState,
    modifier: Modifier
) {
    LazyVerticalGrid(
        columns = GridCells.Adaptive(minSize = 128.dp),
        modifier = modifier
    ) {
        deckBuilderState.cards.forEach { (card, count) ->
            item(card) {
                deckBuilderItem(
                    deckBuilderState,
                    card,
                    count,
                )
            }
        }
    }
}

@Composable
fun deckBuilderItem(
    deckBuilderState: DeckBuilderState,
    card: Card,
    count: Int,
    modifier: Modifier = Modifier
) {

    val imagePath = "${deckBuilderState.imagesFolder}/${card.id}-small.png"
    val image = painterResource(imagePath)
    Column(modifier = modifier) {
        Box(
            modifier = modifier.height(image.intrinsicSize.height.dp).padding(16.dp)
        ) {
            Image(
                image,
                card.name,
                contentScale = ContentScale.Fit
            )
        }

    }
}


@Composable
fun dropDown(
    deckBuilderState: DeckBuilderState,
    card: Card,
    modifier: Modifier = Modifier
) {

    var lastValue by remember { mutableStateOf(0F) }

    val cardsAllowed = if (card.supertype == "Energy" || card.supertype == "Trainer") {
        deckBuilderState.getCardsLeft()
    } else {
        4
    }

    Box() {
        Slider(
            onValueChange = { lastValue = it },
            value = lastValue,
            valueRange = 0F.rangeTo(cardsAllowed.toFloat()),
            steps = cardsAllowed,
            modifier = modifier
        )
    }

}

@Composable
fun cardsLeft(
    deckBuilderState: DeckBuilderState,
    modifier: Modifier = Modifier
) {
    Row(modifier = modifier) {
        Text("Cards left: ${deckBuilderState.getCardsLeft()}", modifier = modifier)
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

    Row(modifier = modifier) {
        Text("Total energy costs", modifier = modifier)
        for ((energy, cost) in energyCosts) {
            Column(modifier = modifier) {
                Text("$cost ${energy.name} cards", modifier = modifier)
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
    Row(modifier = modifier.fillMaxWidth()) {
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