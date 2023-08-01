
private val DECK_SIZE = 60
class DeckBuilderState {
    fun getCardsLeft(): Int {
        return DECK_SIZE - mPokemonCards.size - mTrainerCards.size - mEnergyCards.size
    }

    private val mPokemonCards = mutableListOf<PokemonCard>()
    val pokemonCards: List<PokemonCard> = mPokemonCards

    private val mTrainerCards = mutableListOf<TrainerCard>()
    val trainerCards: List<TrainerCard> = mTrainerCards

    private val mEnergyCards = mutableListOf<EnergyCard>()
    val energyCards: List<EnergyCard> = mEnergyCards

}
