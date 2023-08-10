using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonTCG.DataSources;
using PokemonTCG.Utilities;
using PokemonTCG.View;

namespace PokemonTCG.Models
{
    /// <summary>
    /// The state of the game. To be used for the <c>GamePage</c>
    /// </summary>
    public class GameState
    {
        internal readonly PlayerState PlayerState;
        internal readonly PlayerState OpponentState;

        /// <summary>
        /// Creates a new <c>GameState</c>
        /// </summary>
        public GameState(PokemonDeck playerDeck, PokemonDeck opponentDeck)
        {
            Random random = new();
            bool playerFirst = random.Next(2) == 0;

            bool playerHasBasic = false;
            bool opponentHasBasic = false;

            // Shuffle and draw until at least one player has a basic Pokemon
            PlayerState potentialPlayerState = null;
            PlayerState potentialOpponentState = null;
            while (!playerHasBasic && !opponentHasBasic)
            {
                potentialPlayerState = ShuffleAndDraw7Cards(playerDeck);
                potentialOpponentState = ShuffleAndDraw7Cards(opponentDeck);
                playerHasBasic = potentialPlayerState.HandHasBasicPokemon();
                opponentHasBasic = potentialOpponentState.HandHasBasicPokemon();
            }

            // Shuffle and draw until both players have a basic Pokemon
            int opponentDraws = 0;
            if (!playerHasBasic)
            {
                while (!playerHasBasic)
                {
                    potentialPlayerState = ShuffleAndDraw7Cards(playerDeck);
                    playerHasBasic = potentialPlayerState.HandHasBasicPokemon();
                    opponentDraws++;
                }
            }
            int playerDraws = 0;
            if (!opponentHasBasic)
            {
                while (!opponentHasBasic)
                {
                    potentialOpponentState = ShuffleAndDraw7Cards(opponentDeck);
                    opponentHasBasic = potentialOpponentState.HandHasBasicPokemon();
                    playerDraws++;
                }
            }
            Debug.Assert(potentialOpponentState != null && potentialPlayerState != null);
            PlayerState = potentialPlayerState;
            OpponentState = potentialOpponentState;

        }

        internal GameState(PlayerState playerState, PlayerState newOpponentState)
        {
            PlayerState = playerState;
            OpponentState = newOpponentState;
        }

        private void SetUpPlayer()
        {
            
        }

        internal GameState SetUpOpponent()
        {
            ImmutableList<PokemonCard> basicPokemon = OpponentState.Hand.Where(
                card => PokemonCard.IsBasicPokemon(card)
            ).ToImmutableList();

            PokemonCard active;
            PlayerState newOpponentState = OpponentState;
            if (basicPokemon.Count == 1)
            {
                active = basicPokemon.First();
            }
            else
            {
                IDictionary<PokemonCard, int> rank = RankBasicPokemonByHowManyAttacksAreCoveredByEnergy();
                int maxRank = rank.Max(r => r.Value);
                ImmutableList<PokemonCard> potentialPokemon = rank
                    .Where(r => r.Value == maxRank)
                    .ToDictionary(r => r.Key, r => r.Value).Keys
                    .ToImmutableList();

                Dictionary<PokemonCard, int> efficientAttackers = GetDamagePerEnergy(potentialPokemon);
                active = efficientAttackers.First().Key;
                rank.Remove(active);
                newOpponentState = newOpponentState.MoveFromHandToBench(
                rank.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).Take(5).ToImmutableList()
                );
            }
            newOpponentState = newOpponentState.MoveFromHandToActive(active);
            newOpponentState = newOpponentState.SetUpPrizes();
            return new GameState(PlayerState, newOpponentState);
            // TODO maybe max damage and evolution
        }

        private Dictionary<PokemonCard, int> GetDamagePerEnergy(ImmutableList<PokemonCard> potentialPokemon)
        {
            List<PokemonCard> fastestAttackers = new();
            Dictionary<PokemonCard, int> pokemonDamage = new();
            int lowestEnergy = int.MaxValue;

            foreach (PokemonCard pokemon in potentialPokemon)
            {
                foreach (Attack attack in pokemon.Attacks)
                {
                    int energyCount = attack.ConvertedEnergyCost;
                    if (energyCount < lowestEnergy)
                    {
                        lowestEnergy = energyCount;
                        fastestAttackers.Clear();
                        fastestAttackers.Add(pokemon);
                        pokemonDamage.Clear();
                        pokemonDamage[pokemon] = attack.Damage;
                    }
                    else if (energyCount == lowestEnergy)
                    {
                        fastestAttackers.Add(pokemon);
                        pokemonDamage[pokemon] = attack.Damage;
                    }
                }
            }
            Dictionary<PokemonCard, int> efficientAttackers = new();
            foreach (PokemonCard pokemon in fastestAttackers)
            {
                efficientAttackers[pokemon] =
                    pokemonDamage[pokemon] /
                    pokemon.Attacks.Where(attack => attack.ConvertedEnergyCost == lowestEnergy).Min(attack => attack.Damage);
            }
            return efficientAttackers;
        }

        private IDictionary<PokemonCard, int> RankBasicPokemonByHowManyAttacksAreCoveredByEnergy()
        {
            Dictionary<PokemonCard, int> rank = new();

            ImmutableList<PokemonCard> basicPokemon = OpponentState.Hand.Where(
               card => PokemonCard.IsBasicPokemon(card)
           ).ToImmutableList();

            foreach (PokemonCard pokemon in basicPokemon)
            {
                rank[pokemon] = 0;
            }
            // Check if there is enough energy for each attack
            foreach (PokemonCard pokemon in basicPokemon)
            {
                foreach (Attack attack in pokemon.Attacks)
                {
                    bool enoughEnergyForAttack = HasEnoughEnergyForAttack(attack);
                    if (enoughEnergyForAttack)
                    {
                        rank[pokemon]++;
                    }
                }
            }
            return rank;
        }

        private bool HasEnoughEnergyForAttack(Attack attack)
        {
            bool enoughEnergyForAttack = true;

            // Count energy cards from hand
            ImmutableDictionary<PokemonType, int> numberOfEveryEnergy = OpponentState.Hand
                .Where(card => card.Supertype == CardSupertype.Energy)
                .GroupBy(card => PokemonCard.GetEnergyType(card))
                .ToImmutableDictionary(group => group.Key, group => group.Count());
            int numberOfEnergies = OpponentState.Hand
                .Where(card => card.Supertype == CardSupertype.Energy)
                .Count();

            foreach ((PokemonType type, int count) in attack.EnergyCost)
            {
                if ((!numberOfEveryEnergy.ContainsKey(type) ||
                    (numberOfEveryEnergy[type] < count)) ||
                    (type == PokemonType.Colorless && numberOfEnergies < count))
                {
                    enoughEnergyForAttack = false;
                }
            }
            return enoughEnergyForAttack;
        }

        private static PlayerState ShuffleAndDraw7Cards(PokemonDeck deck)
        {
            ImmutableList<PokemonCard> deckState = deck.Shuffle().Select(id => CardDataSource.GetCardById(id)).ToImmutableList();
            (ImmutableList<PokemonCard> deckOfCards, ImmutableList<PokemonCard> hand) = DeckUtil.DrawCards(deckState, 7);
            return new PlayerState(deckOfCards, hand, null, ImmutableList<PokemonCard>.Empty, ImmutableList<PokemonCard>.Empty, ImmutableList<PokemonCard>.Empty);
        }

    }

}
