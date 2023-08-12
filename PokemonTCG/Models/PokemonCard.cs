using PokemonTCG.Enums;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PokemonTCG.Models
{

    /// <summary>
    /// Contains card data read from the json files.
    /// </summary>
    internal class PokemonCard
    {
        internal readonly string Id;
        internal readonly string Name;
        internal readonly CardSupertype Supertype;
        internal readonly ImmutableList<CardSubtype> Subtypes;
        internal readonly int Level;
        internal readonly int Hp;
        internal readonly ImmutableList<PokemonType> Types;
        internal readonly string EvolvesFrom;
        internal readonly ImmutableList<Ability> Abilities;
        internal readonly ImmutableList<Attack> Attacks;
        internal readonly ImmutableDictionary<PokemonType, Modifier> Weaknesses;
        internal readonly ImmutableDictionary<PokemonType, Modifier> Resistances;
        internal readonly ImmutableDictionary<PokemonType, int> RetreatCost;
        internal readonly int ConvertedRetreatCost;
        internal readonly ImmutableDictionary<ImageSize, string> ImagePaths;
        internal readonly string SetId;
        internal readonly string SetName;
        internal readonly string SetSeries;
        internal readonly int Number;
        internal readonly string Artist;
        internal readonly Rarity Rarity;
        internal readonly string FlavorText;
        internal readonly IDictionary<LegalFormat, Legality> Legalities;

        internal PokemonCard(
            string id,
            string name,
            CardSupertype supertype,
            List<CardSubtype> subtypes,
            int level,
            int hp,
            List<PokemonType> types,
            string evolvesFrom,
            List<Ability> abilities,
            List<Attack> attacks,
            Dictionary<PokemonType, Modifier> weaknesses,
            Dictionary<PokemonType, Modifier> resistances,
            Dictionary<PokemonType, int> retreatCost,
            int convertedRetreatCost,
            Dictionary<ImageSize, string> imagePaths,
            string setId,
            string setName,
            string setSeries,
            int number,
            string artist,
            Rarity rarity,
            string flavorText,
            IDictionary<LegalFormat, Legality> legalities
        )
        {
            Id = id;
            Name = name;
            Supertype = supertype;
            Subtypes = subtypes.ToImmutableList();
            Level = level;
            Hp = hp;
            Types = types.ToImmutableList();
            EvolvesFrom = evolvesFrom;
            Abilities = abilities.ToImmutableList();
            Attacks = attacks.ToImmutableList();
            Weaknesses = weaknesses.ToImmutableDictionary();
            Resistances = resistances.ToImmutableDictionary();
            RetreatCost = retreatCost.ToImmutableDictionary();
            ConvertedRetreatCost = convertedRetreatCost;
            ImagePaths = imagePaths.ToImmutableDictionary();
            SetId = setId;
            SetName = setName;
            SetSeries = setSeries;
            Number = number;
            Artist = artist;
            Rarity = rarity;
            FlavorText = flavorText;
            Legalities = legalities;
        }

    }

}
