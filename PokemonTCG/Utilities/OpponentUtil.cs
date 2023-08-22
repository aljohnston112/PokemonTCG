using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.Models;

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
                    int energyNeededToAttack = AttackUtil.GetNumEnergyNeededToAttack(
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

    }

}
