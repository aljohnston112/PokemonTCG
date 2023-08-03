import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import pokemon_tcg_api.APIHelper

class BaseSet {

    companion object {

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
                if (poke.healthLeft() != poke.card.hp) {
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
