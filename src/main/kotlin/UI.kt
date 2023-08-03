import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.runtime.Composable
import androidx.compose.runtime.State
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.rotate
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.window.Dialog

class UI {

    companion object {

        @Composable
        fun playingField(
            gameState: GameState,
            modifier: Modifier = Modifier
        ) {

            Column(
                modifier=modifier.fillMaxSize()
                    .background(Color(243, 209, 75))
            ) {
                val newModifier=modifier.fillMaxSize().weight(1f)
                Row(modifier=newModifier) {
                    prizeCard(gameState, 0, modifier=newModifier)
                    prizeCard(gameState, 1, modifier=newModifier)
                    background(modifier=newModifier)
                    benchedPokemon(gameState, 0, modifier=newModifier)
                    benchedPokemon(gameState, 1, modifier=newModifier)
                    benchedPokemon(gameState, 2, modifier=newModifier)
                    benchedPokemon(gameState, 3, modifier=newModifier)
                    benchedPokemon(gameState, 4, modifier=newModifier)
                    background(modifier=newModifier)
                    faceDownCard("Opponent discard pile", flipped=true, modifier=newModifier)
                }
                Row(modifier=newModifier) {
                    prizeCard(gameState, 2, modifier=newModifier)
                    prizeCard(gameState, 3, modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    faceDownCard("Opponent deck", flipped=true, modifier=newModifier)
                }
                Row(modifier=newModifier) {
                    prizeCard(gameState, 4, modifier=newModifier)
                    prizeCard(gameState, 5, modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    opponentPokemon(gameState, modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                }
                Row(modifier=newModifier) { }
                Row(modifier=newModifier) {
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    playerPokemon(gameState, modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    prizeCard(gameState, 6, modifier=newModifier)
                    prizeCard(gameState, 7, modifier=newModifier)
                }
                Row(modifier=newModifier) {
                    faceDownCard("Your discard pile", flipped=false, modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    background(modifier=newModifier)
                    prizeCard(gameState, 8, modifier=newModifier)
                    prizeCard(gameState, 9, modifier=newModifier)
                }
                Row(modifier=newModifier) {
                    faceDownCard("Your deck", flipped=false, modifier=newModifier)
                    background(modifier=newModifier)
                    benchedPokemon(gameState, 5, modifier=newModifier)
                    benchedPokemon(gameState, 6, modifier=newModifier)
                    benchedPokemon(gameState, 7, modifier=newModifier)
                    benchedPokemon(gameState, 8, modifier=newModifier)
                    benchedPokemon(gameState, 9, modifier=newModifier)
                    background(modifier=newModifier)
                    prizeCard(gameState, 10, modifier=newModifier)
                    prizeCard(gameState, 11, modifier=newModifier)
                }
            }

        }

        @Composable
        fun playerPokemon(gameState: GameState, modifier: Modifier) {
            val card = gameState.playerState.active?.card
            val imagePath = card?.images?.get("large")?:"decks/BlankCard2.png"
            val image = painterResource(imagePath)
            Box(
                contentAlignment = Alignment.Center,
                modifier = modifier.fillMaxSize().rotate(180f)
            ) {
                Image(
                    image,
                    "Your active ${card?.name?:"spot is empty"}",
                    contentScale = ContentScale.Fit
                )
            }
        }

        @Composable
        fun opponentPokemon(gameState: GameState, modifier: Modifier) {
            val card = gameState.opponentState.active?.card
            val imagePath = card?.images?.get("large")?:"decks/BlankCard2.png"
            val image = painterResource(imagePath)
            Box(
                contentAlignment = Alignment.Center,
                modifier = modifier.fillMaxSize().rotate(180f)
            ) {
                Image(
                    image,
                    "Opponent's active ${card?.name?:"spot is empty"}",
                    contentScale = ContentScale.Fit
                )
            }
        }

        @Composable
        private fun faceDownCard(
            contentDescription: String,
            flipped: Boolean,
            modifier: Modifier
        ) {
            val imagePath = if(flipped){
                "decks/CardBackFlipped.png"
            } else {
                "decks/CardBack.png"
            }
            val image = painterResource(imagePath)
            Box(
                contentAlignment = Alignment.Center,
                modifier = modifier.fillMaxSize()
            ) {

                Image(
                    image,
                    contentDescription,
                    contentScale = ContentScale.Fit
                )
            }
        }

        @Composable
        private fun benchedPokemon(
            gameState: GameState,
            i: Int,
            modifier: Modifier
        ) {
            var newModifier = modifier
            val (contentDescription, imagePath) = if (i in 0..5) {
                val opponentBenched = gameState.opponentState.getFaceUpBenchedPokemon()
                if (i < opponentBenched.size) {
                    newModifier = modifier.rotate(180F)
                    Pair(
                        "Opponent's benched ${opponentBenched[i].card.name}",
                        opponentBenched[i].card.images["large"]!!
                    )
                } else if(i < gameState.opponentState.getNumberOfBenchedPokemon()) {
                    Pair("Opponent's unknown benched Pokemon", "decks/CardBackFlipped.png")
                } else {
                    Pair(
                        "An empty bench spot",
                        "decks/BlankCard2.png"
                    )
                }
            } else {
                val playerBenched = gameState.playerState.getFaceUpBenchedPokemon()
                if ((i - 6) < playerBenched.size) {
                    Pair(
                        "Your benched ${playerBenched[i].card.name}",
                        playerBenched[i].card.images["large"]!!
                    )
                } else if((i - 6) < gameState.playerState.getNumberOfBenchedPokemon()) {
                    Pair("Your benched ${playerBenched[i].card.name}", "decks/CardBack.png")
                } else {
                    Pair(
                        "An empty bench spot",
                        "decks/BlankCard2.png"
                    )
                }
            }

            val image = painterResource(imagePath)
            Box(
                contentAlignment = Alignment.Center,
                modifier = modifier.fillMaxSize()
            ) {

                Image(
                    image,
                    contentDescription,
                    contentScale = ContentScale.Fit,
                    modifier=newModifier
                )
            }
        }

        @Composable
        private fun background(modifier: Modifier = Modifier) {
            val image = painterResource("decks/BlankCard2.png")
            Box(
                contentAlignment = Alignment.Center,
                modifier = modifier.fillMaxSize()
            ) {

                Image(
                    image,
                    "Playing field",
                    contentScale = ContentScale.Fit
                )
            }
        }

        /**
         * @param i 0 to 5 are the opponent prizes and 6 to 11 are the player prizes.
         *          The indices go from top-left to bottom-right.
         */
        @Composable
        fun prizeCard(
            gameState: GameState,
            i: Int,
            modifier: Modifier = Modifier
        ) {

            val (contentDescription, imagePath) = if (i in 0..5) {
                val opponentPrizes = gameState.opponentState.numberOfPrizesLeft()
                if (i < opponentPrizes) {
                    Pair("One of your opponent's prize cards", "decks/CardBackFlipped.png")
                } else {
                    Pair("Your opponent won this prize", "decks/BlankCard.png")
                }
            } else {
                val playerPrizes = gameState.playerState.numberOfPrizesLeft()
                if ((i - 6) < playerPrizes) {
                    Pair("One of your prize cards", "decks/CardBack.png")
                } else {
                    Pair("You won this prize", "decks/BlankCard.png")
                }
            }

            val image = painterResource(imagePath)
            Box(
                contentAlignment = Alignment.Center,
                modifier = modifier.fillMaxSize()
            ) {

                Image(
                    image,
                    contentDescription,
                    contentScale = ContentScale.Fit
                )
            }
        }

        @Composable
        fun askUserToPickFromCards(
            text: String,
            choices: List<CardState>,
            state: State<CardState?>,
            modifier: Modifier = Modifier
        ) {
            val selectedCard = cardPickerDialog(choices, state, modifier)
        }

        @Composable
        fun cardPickerDialog(
            choices: List<CardState>,
            state: State<CardState?>,
            modifier: Modifier
        ) {
            Dialog(
                onCloseRequest = {}
            ) {
                cardPicker(choices, state, modifier)
            }
        }

        @Composable
        private fun cardPicker(
            choices: List<CardState>,
            state: State<CardState?>,
            modifier: Modifier
        ) {
            Column {
                choices.forEach {
                    Image(
                        painterResource(it.card.images["large"]!!),
                        "${it.card.name} with ${it.healthLeft()}0 hp"
                    )
                }
            }
        }


    }

}