import pokemon_tcg_api.APIHelper
import pokemon_tcg_api.Card

private val DECK_SIZE = 60

class DeckBuilderState(
    val cards: List<Card>,
    val pokemonCards: List<PokemonCard>,
    val trainerCards: List<TrainerCard>,
    val energyCards: List<EnergyCard>
) {

    fun getCardsLeft(): Int {
        return DECK_SIZE - pokemonCards.size - trainerCards.size - energyCards.size
    }

}
