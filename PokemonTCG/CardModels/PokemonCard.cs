using PokemonTCG.Enums;
using PokemonTCG.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Xml.Linq;

namespace PokemonTCG.CardModels
{

    /// <summary>
    /// Contains card data read from the json files.
    /// </summary>
    internal class PokemonCard
    {
        internal readonly string Id;
        internal readonly string Name;
        internal readonly CardSupertype Supertype;
        internal readonly IImmutableList<CardSubtype> Subtypes;
        internal readonly int Level;
        internal readonly int Hp;
        internal readonly IImmutableList<PokemonType> Types;
        internal readonly string EvolvesFrom;
        internal readonly IImmutableList<Ability> Abilities;
        internal readonly IImmutableList<Attack> Attacks;
        internal readonly IImmutableDictionary<PokemonType, Modifier> Weaknesses;
        internal readonly IImmutableDictionary<PokemonType, Modifier> Resistances;
        internal readonly IImmutableDictionary<PokemonType, int> RetreatCost;
        internal readonly int ConvertedRetreatCost;
        internal readonly string ImagePath;
        internal readonly string SetId;
        internal readonly string SetName;
        internal readonly string SetSeries;
        internal readonly int Number;
        internal readonly string Artist;
        internal readonly Rarity Rarity;
        internal readonly string FlavorText;
        internal readonly IImmutableDictionary<LegalFormat, Legality> Legalities;

        internal PokemonCard(
            string id,
            string name,
            CardSupertype supertype,
            IImmutableList<CardSubtype> subtypes,
            int level,
            int hp,
            IImmutableList<PokemonType> types,
            string evolvesFrom,
            IImmutableList<Ability> abilities,
            IImmutableList<Attack> attacks,
            IImmutableDictionary<PokemonType, Modifier> weaknesses,
            IImmutableDictionary<PokemonType, Modifier> resistances,
            IImmutableDictionary<PokemonType, int> retreatCost,
            int convertedRetreatCost,
            string imagePath,
            string setId,
            string setName,
            string setSeries,
            int number,
            string artist,
            Rarity rarity,
            string flavorText,
            IImmutableDictionary<LegalFormat, Legality> legalities
        )
        {
            Id = id;
            Name = name;
            Supertype = supertype;
            Subtypes = subtypes;
            Level = level;
            Hp = hp;
            Types = types;
            EvolvesFrom = evolvesFrom;
            Abilities = abilities;
            Attacks = attacks;
            Weaknesses = weaknesses;
            Resistances = resistances;
            RetreatCost = retreatCost;
            ConvertedRetreatCost = convertedRetreatCost;
            ImagePath = imagePath;
            SetId = setId;
            SetName = setName;
            SetSeries = setSeries;
            Number = number;
            Artist = artist;
            Rarity = rarity;
            FlavorText = flavorText;
            Legalities = legalities;
        }

        internal PokemonCard WithoutAttack(Attack attack)
        {
            return new PokemonCard(
                id: Id,
                name: Name,
                supertype: Supertype,
                subtypes: Subtypes,
                level: Level,
                hp: Hp,
                types: Types,
                evolvesFrom: EvolvesFrom,
                abilities: Abilities,
                attacks: Attacks.Remove(attack),
                weaknesses: Weaknesses,
                resistances: Resistances,
                retreatCost: RetreatCost,
                convertedRetreatCost: ConvertedRetreatCost,
                imagePath: ImagePath,
                setId: SetId,
                setName: SetName,
                setSeries: SetSeries,
                number: Number,
                artist: Artist,
                rarity: Rarity,
                flavorText: FlavorText,
                legalities: Legalities
                );
        }

    }

}
