using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.States;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace PokemonTCG.Utilities
{
    internal class OpponentUtil
    {

        internal static int GetNumberOfCardsPokemonCanKO(
            PokemonCard pokemonCard,
            int damageTaken,
            IImmutableList<PokemonCard> hand,
            IList<PokemonCardState> pokemonToBeat
            )
        {
            IImmutableList<PokemonCard> energyInHand = hand.Where(card => card.Supertype == CardSupertype.ENERGY).ToImmutableList();
            int numberOfPokemonKOed = 0;

            foreach (PokemonCardState playerCard in pokemonToBeat)
            {
                Dictionary<Attack, int> energyTillEachAttackForPlayer = new();
                foreach (Attack attack in playerCard.PokemonCard.Attacks)
                {
                    int energyNeededToAttack = AttackUtil.GetEnergyNeededToAttack(
                    attack,
                    playerCard
                    );
                    energyTillEachAttackForPlayer[attack] = energyNeededToAttack;
                }

                Dictionary<Attack, int> energyTillEachAttackForOpponent = new();
                foreach (Attack attack in pokemonCard.Attacks)
                {
                    if (AttackUtil.IsEnoughEnergyForAttack(energyInHand, attack))
                    {
                        energyTillEachAttackForOpponent[attack] = attack.ConvertedEnergyCost;
                    }
                }

                double damagePerTurnPlayer = GetDamagePerTurnForPlayer(
                    energyTillEachAttackForPlayer
                    )
                    .Max(rank => rank.Value);

                double damagePerTurnOpponent = GetDamagePerTurnForPlayer(
                    energyTillEachAttackForOpponent
                    )
                    .Max(rank => rank.Value);

                double playerHealth = playerCard.HealthLeft();
                double opponentHealth = pokemonCard.Hp - damageTaken;
                while (playerHealth > 0 && opponentHealth > 0)
                {
                    playerHealth -= damagePerTurnOpponent;
                    opponentHealth -= damagePerTurnPlayer;
                }
                if (opponentHealth > 0)
                {
                    numberOfPokemonKOed++;
                }
            }
            return numberOfPokemonKOed;
        }

        private static IImmutableDictionary<Attack, double> GetDamagePerTurnForPlayer(
            Dictionary<Attack, int> energyTillEachAttack
            )
        {
            return energyTillEachAttack.ToDictionary(
                kv => kv.Key,
                kv => (double)kv.Key.ConvertedEnergyCost / kv.Value
                ).ToImmutableDictionary();
        }

        internal static IDictionary<PokemonCard, int> RankBasicPokemonByHowManyAttacksAreCoveredByEnergy(
            IImmutableList<PokemonCard> basicPokemon,
            IImmutableList<PokemonCard> hand
            )
        {
            Dictionary<PokemonCard, int> rank = new();

            foreach (PokemonCard pokemon in basicPokemon)
            {
                rank[pokemon] = 0;
            }
            // Check if there is enough energy for each attack
            foreach (PokemonCard pokemon in basicPokemon)
            {
                foreach (Attack attack in pokemon.Attacks)
                {
                    bool enoughEnergyForAttack = AttackUtil.IsEnoughEnergyForAttack(hand, attack);
                    if (enoughEnergyForAttack)
                    {
                        rank[pokemon]++;
                    }
                }
            }
            return rank;
        }

        internal static IDictionary<PokemonCard, int> GetFastestEfficientAttackers(
            IImmutableList<PokemonCard> potentialPokemon
            )
        {
            HashSet<PokemonCard> fastestAttackers = new();
            Dictionary<PokemonCard, int> pokemonToDamage = new();
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
                        pokemonToDamage.Clear();
                        pokemonToDamage[pokemon] = attack.Damage;
                    }
                    else if (energyCount == lowestEnergy)
                    {
                        int damage = attack.Damage;
                        fastestAttackers.Add(pokemon);
                        if (pokemonToDamage.ContainsKey(pokemon))
                        {
                            damage = Math.Max(damage, pokemonToDamage[pokemon]);
                        }
                        pokemonToDamage[pokemon] = damage;
                    }
                }
            }

            Dictionary<PokemonCard, int> efficientAttackers = new();
            foreach (PokemonCard pokemon in fastestAttackers)
            {
                efficientAttackers[pokemon] = pokemonToDamage[pokemon] / lowestEnergy;
            }
            return efficientAttackers;
        }

        internal static PokemonCard BestCardIfShouldAddToBench(GameState gameState)
        {
            List<PokemonCard> bestHandCards = new();
            PokemonCard bestHandCard = null;
            if (gameState.OpponentState.HandHasBasicPokemon())
            {
                IList<PokemonCardState> playerCards = gameState.PlayerState.Bench.ToList();
                playerCards.Add(gameState.PlayerState.Active);


                // Check how much many turns it will take for each bench Pokemon to KO
                //     all player cards on field
                int bestNumberOfKOs = -1;
                foreach (PokemonCardState benchCard in gameState.OpponentState.Bench)
                {
                    int numberOfKOs = GetNumberOfCardsPokemonCanKO(
                        pokemonCard: benchCard.PokemonCard,
                        damageTaken: benchCard.DamageTaken,
                        hand: gameState.OpponentState.Hand,
                        pokemonToBeat: playerCards
                        );
                    bestNumberOfKOs = Math.Max(bestNumberOfKOs, numberOfKOs);
                }

                // Get hand cards that can KO more cards than those in the bench
                foreach (PokemonCard handCard in gameState.OpponentState.Hand.Where(card => CardUtil.IsBasicPokemon(card)))
                {
                    int numberOfKOs = GetNumberOfCardsPokemonCanKO(
                        pokemonCard: handCard,
                        damageTaken: 0,
                        hand: gameState.OpponentState.Hand,
                        pokemonToBeat: playerCards
                        );
                    if (numberOfKOs >= bestNumberOfKOs)
                    {
                        bestNumberOfKOs = numberOfKOs;
                        bestHandCards.Add(handCard);
                    }
                }

                // Rank by energy and damage
                IDictionary<PokemonCard, int> rank = RankBasicPokemonByHowManyAttacksAreCoveredByEnergy(
                    bestHandCards.ToImmutableList(),
                    gameState.OpponentState.Hand
                    );
                int maxRank = rank.Max(rank => rank.Value);
                IImmutableList<PokemonCard> potentialPokemon = rank
                    .Where(rank => rank.Value == maxRank)
                    .Select(rank => rank.Key)
                    .ToImmutableList();
                IDictionary<PokemonCard, int> efficientAttackers = GetFastestEfficientAttackers(potentialPokemon);
                maxRank = efficientAttackers.Max(rank => rank.Value);
                bestHandCard = efficientAttackers
                    .Where(rank => rank.Value == maxRank)
                    .First().Key;
            }
            return bestHandCard;
        }

        internal static PokemonCard PotentialEvolveOf(
            PokemonCardState active,
            IImmutableList<PokemonCard> hand
            )
        {
            PokemonCard evolution = null;
            foreach (PokemonCard handCard in hand)
            {
                if (CardUtil.CardEvolvesFrom(active, handCard))
                {
                    bool canAttack = false;
                    foreach (Attack attack in handCard.Attacks)
                    {
                        if (active.Energy.Count + 1 >= attack.ConvertedEnergyCost)
                        {
                            canAttack = true;
                        }
                    }
                    if (canAttack)
                    {
                        evolution = handCard;
                    }
                }
            }
            return evolution;
        }

    }

}
