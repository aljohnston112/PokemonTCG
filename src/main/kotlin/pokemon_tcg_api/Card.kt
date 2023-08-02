package pokemon_tcg_api

import kotlinx.serialization.Serializable
import kotlinx.serialization.Transient

@Serializable
data class Card(
    val id: String,
    val name: String,
    val supertype: String,
    val subtypes: List<String> = listOf(),
    val level: Int = 0,
    val hp: Int = 0,
    val types: List<String> = listOf(),
    val evolvesFrom: String = "",
    val abilities: List<Ability> = listOf(),
    val attacks: List<Attack> = listOf(),
    val weaknesses: List<Weakness> = listOf(),
    val retreatCost: List<String> = listOf(),
    val convertedRetreatCost: Int = 0,
    val number: Int,
    val artist: String,
    val rarity: String = "",
    val flavorText: String = "",
    val nationalPokedexNumbers: List<Int> = listOf(),
    val legalities: Map<String, String>,
    val images: Map<String, String>
    )

@Serializable
data class Weakness(
    val type: String,
    val value: String
)

@Serializable
data class Attack(
    val name: String,
    val cost: List<String>,
    val convertedEnergyCost: Int,
    @Transient val damage: Int = 0,
    val text: String
)

@Serializable
data class Ability(
    val name: String,
    val text: String,
    val type: String
)
