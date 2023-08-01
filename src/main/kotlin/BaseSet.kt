import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf

class BaseSet {

    companion object {
        val cards = listOf(
            Card(
                "decks/0 - Base/images/large/1 - Alakazam.png",
                "Alakazam",
                80,
                listOf(
                    // PreAttackAction("Pokemon Power: Damage Swap", 0, ::damageSwap)
                )
            )
        )
        
        @Composable
        private fun damageSwap(gameState: GameState) {

            val playerPokemon = gameState.playerState.getFaceUpPokemon()
            val validPlayerPokemonChoices = getPokemonWithDamage(playerPokemon)
            val playerPoke = UI.askUserToPickFromCards(
                "Select one of your Pokemon",
                validPlayerPokemonChoices,
                mutableStateOf(null)
            )

            val opponentPokemon = gameState.opponentState.getFaceUpPokemon()
            val validOpponentPokemonChoices = getPokemonWithHPGreaterThan(opponentPokemon, 1)
            val opponentPoke = UI.askUserToPickFromCards(
                "Select one of your opponent's Pokemon",
                validOpponentPokemonChoices,
                mutableStateOf(null)
            )

        }

        private fun getPokemonWithDamage(pokemon: List<CardState>): List<CardState> {
            val validPokemon = mutableListOf<CardState>()
            for (poke in pokemon) {
                if (poke.healthLeft() != poke.card.maxHP) {
                    validPokemon.add(poke)
                }
            }
            return validPokemon
        }

        private fun getPokemonWithHPGreaterThan(pokemon: List<CardState>, hp: Int): List<CardState> {
            val validPokemon = mutableListOf<CardState>()
            for (poke in pokemon) {
                if (poke.healthLeft() > hp) {
                    validPokemon.add(poke)
                }
            }
            return validPokemon
        }

    }

}
