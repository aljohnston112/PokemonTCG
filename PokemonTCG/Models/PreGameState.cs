using PokemonTCG.DataSources;
using PokemonTCG.Enums;
using PokemonTCG.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace PokemonTCG.Models
{

    internal class PreGameState
    {

        private readonly bool PlayerGoesFirst;
        private readonly int PlayerDraws = 0;
        private readonly int OpponentDraws = 0;

        internal readonly GameFieldState GameFieldState;

        private PreGameState(
            GameFieldState gameFieldState, 
            bool playerGoesFirst, 
            int playerDraws, 
            int opponentDraws
            )
        {
            GameFieldState = gameFieldState;
            PlayerGoesFirst = playerGoesFirst;
            PlayerDraws = playerDraws;
            OpponentDraws = opponentDraws;
        }

        internal static PreGameState StartPreGame(
            PokemonDeck playerDeck,
            PokemonDeck opponentDeck
            )
        {
            bool playerHasBasic = false;
            bool opponentHasBasic = false;

            int playerDraws = 0;
            int opponentDraws = 0;

            // Shuffle and draw until at least one player has a basic Pokemon
            PlayerState potentialPlayerState = null;
            PlayerState potentialOpponentState = null;
            while (!playerHasBasic && !opponentHasBasic)
            {
                potentialPlayerState = ShuffleAndDraw7Cards(playerDeck);
                potentialOpponentState = ShuffleAndDraw7Cards(opponentDeck);
                playerHasBasic = potentialPlayerState.HandHasBasicPokemon();
                opponentHasBasic = potentialOpponentState.HandHasBasicPokemon();
                opponentDraws++;
                playerDraws++;
            }

            // Shuffle and draw until both players have a basic Pokemon
            if (!playerHasBasic)
            {
                while (!playerHasBasic)
                {
                    potentialPlayerState = ShuffleAndDraw7Cards(playerDeck);
                    playerHasBasic = potentialPlayerState.HandHasBasicPokemon();
                    opponentDraws++;
                }
            }

            if (!opponentHasBasic)
            {
                while (!opponentHasBasic)
                {
                    potentialOpponentState = ShuffleAndDraw7Cards(opponentDeck);
                    opponentHasBasic = potentialOpponentState.HandHasBasicPokemon();
                    playerDraws++;
                }
            }
            if (playerDraws > opponentDraws)
            {
                opponentDraws = 0;
            }
            else
            {
                playerDraws = 0;
            }
            Debug.Assert(potentialOpponentState != null && potentialPlayerState != null);
            bool playerGoesFirst = CoinUtil.FlipCoin();
            GameFieldState gameFieldState = new(potentialPlayerState, potentialOpponentState);
            return new PreGameState(
                gameFieldState: gameFieldState, 
                playerGoesFirst: playerGoesFirst, 
                playerDraws: playerDraws, 
                opponentDraws: opponentDraws
                );
        }

        private static PlayerState ShuffleAndDraw7Cards(PokemonDeck deck)
        {
            IImmutableList<PokemonCard> deckState =
                DeckUtil.ShuffleDeck(deck)
                .Select(id => CardDataSource.GetCardById(id))
                .ToImmutableList();
            (IImmutableList<PokemonCard> deckOfCards, IImmutableList<PokemonCard> hand) =
                DeckUtil.DrawCards(deckState, 7);
            return new PlayerState(
                deck: deckOfCards,
                hand: hand,
                active: null,
                bench: ImmutableList<PokemonCardState>.Empty,
                prizes: ImmutableList<PokemonCard>.Empty,
                discardPile: ImmutableList<PokemonCard>.Empty,
                lostZone: ImmutableList<PokemonCard>.Empty
                );
        }

        internal static GameState SetUpOpponent(PreGameState preGameState)
        {
            IImmutableList<PokemonCard> basicPokemon = preGameState.GameFieldState.OpponentState.Hand
                .Where(card => CardUtil.IsBasicPokemon(card)
            ).ToImmutableList();

            PlayerState newOpponentState = preGameState.GameFieldState.OpponentState;
            PokemonCard active;
            if (basicPokemon.Count == 1)
            {
                active = basicPokemon[0];
            }
            else
            {
                // TODO maybe max damage and evolution
                IDictionary<PokemonCard, int> rank = RankBasicPokemonByHowManyAttacksAreCoveredByEnergy(
                    preGameState.GameFieldState.OpponentState
                    );
                int maxRank = rank.Max(rank => rank.Value);
                IImmutableList<PokemonCard> potentialPokemon = rank
                    .Where(rank => rank.Value == maxRank)
                    .Select(rank => rank.Key)
                    .ToImmutableList();

                IDictionary<PokemonCard, int> efficientAttackers = GetFastestEfficientAttackers(potentialPokemon);
                maxRank = efficientAttackers.Max(rank => rank.Value);
                active = efficientAttackers
                    .Where(rank => rank.Value == maxRank)
                    .OrderByDescending(rank => rank.Value).First().Key;

                rank.Remove(active);
                newOpponentState = newOpponentState.MoveFromHandToBench(
                    rank
                    .OrderByDescending(rank => rank.Value)
                    .Select(kv => kv.Key)
                    .Take(5)
                    .ToImmutableList()
                    );
            }
            newOpponentState = newOpponentState.MoveFromHandToActive(active);
            newOpponentState = newOpponentState.SetUpPrizes();
            newOpponentState = newOpponentState.DrawCards(preGameState.OpponentDraws);
            PlayerState newPlayerState = preGameState.GameFieldState.PlayerState.DrawCards(preGameState.PlayerDraws);
            return new GameState(preGameState.PlayerGoesFirst, new GameFieldState(newPlayerState, newOpponentState));
        }

        private static IDictionary<PokemonCard, int> RankBasicPokemonByHowManyAttacksAreCoveredByEnergy(
            PlayerState opponentState
            )
        {
            Dictionary<PokemonCard, int> rank = new();

            ImmutableList<PokemonCard> basicPokemon = opponentState.Hand.Where(
               card => CardUtil.IsBasicPokemon(card)
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
                    bool enoughEnergyForAttack = HasEnoughEnergyForAttack(opponentState, attack);
                    if (enoughEnergyForAttack)
                    {
                        rank[pokemon]++;
                    }
                }
            }
            return rank;
        }

        private static bool HasEnoughEnergyForAttack(PlayerState opponentState, Attack attack)
        {
            bool enoughEnergyForAttack = true;

            // Count energy cards from hand
            IImmutableDictionary<PokemonType, int> numberOfEveryEnergy = opponentState.Hand
                .Where(card => card.Supertype == CardSupertype.ENERGY)
                .GroupBy(card => CardUtil.GetEnergyType(card))
                .ToImmutableDictionary(group => group.Key, group => group.Count());
            int numberOfEnergies = opponentState.Hand
                .Where(card => card.Supertype == CardSupertype.ENERGY)
                .Count();

            foreach ((PokemonType type, int count) in attack.EnergyCost)
            {
                if ((!numberOfEveryEnergy.ContainsKey(type) || (numberOfEveryEnergy[type] < count)) ||
                    (type == PokemonType.Colorless && numberOfEnergies < count))
                {
                    enoughEnergyForAttack = false;
                }
            }
            return enoughEnergyForAttack;
        }

        private static IDictionary<PokemonCard, int> GetFastestEfficientAttackers(
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
