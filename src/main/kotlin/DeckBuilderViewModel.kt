import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pokemon_tcg_api.APIHelper

class DeckBuilderViewModel {

    private val scope = CoroutineScope(Dispatchers.IO)

    private val _state = MutableStateFlow<DeckBuilderState?>(null)
    val state = _state.asStateFlow()

    init {
        scope.launch {
            val cards = APIHelper.getCardsFromSet("base1").map {
                Pair(it, 0)
            }.toMap()

            _state.emit(
                DeckBuilderState(
                    "sets/base1",
                    cards,
                    listOf(),
                    listOf(),
                    listOf()
                )
            )
        }
    }

}