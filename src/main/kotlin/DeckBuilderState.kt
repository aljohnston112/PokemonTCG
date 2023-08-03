import pokemon_tcg_api.APIHelper
import pokemon_tcg_api.Card

private val DECK_SIZE = 60

class DeckBuilderState(
    val imagesFolder: String,
    val cards: Map<Card, Int>,
    val pokemonCards: List<PokemonCard>,
    val trainerCards: List<TrainerCard>,
    val energyCards: List<EnergyCard>
) {

    fun getCardsLeft(): Int {
        return DECK_SIZE - pokemonCards.size - trainerCards.size - energyCards.size
    }

}
