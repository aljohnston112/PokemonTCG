package pokemon_tcg_api

import kotlinx.serialization.Serializable

@Serializable
data class Series(
    val id: String,
    val name: String,
    val series: String,
    val printedTotal: Int,
    val total: Int,
    val legalities: Map<String, String>,
    val ptcgoCode: String? = null,
    val releaseDate: String,
    val updatedAt: String,
    val images: Map<String, String>
)