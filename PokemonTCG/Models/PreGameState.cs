using PokemonTCG.CardModels;
using PokemonTCG.DataSources;
using PokemonTCG.States;
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

        private static int PlayerDraws = 0;
        private static int OpponentDraws = 0;

        internal static GameState StartGame(
            PokemonDeck playerDeck,
            PokemonDeck opponentDeck
            )
        {
            // * 1. Pick who goes first randomly.
            bool playerGoesFirst = CoinUtil.FlipCoin();

            bool playerHasBasic = false;
            bool opponentHasBasic = false;

            int playerDraws = 0;
            int opponentDraws = 0;

            // Shuffle and draw until at least one player has a basic Pokemon
            PlayerState potentialPlayerState = null;
            PlayerState potentialOpponentState = null;

            // Shuffle and draw until both players have a basic Pokemon
            if (!playerHasBasic)
            {
                while (!playerHasBasic)
                {
                    potentialPlayerState = ShuffleAndDraw7Cards(playerDeck);
                    // * 4. Check for basic Pokemon.
                    // * 5. If no basic Pokemon, go to 2 after opponent reaches step 6.
                    // Not sure why step 6 matters here.
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
            PlayerDraws = playerDraws;
            OpponentDraws = opponentDraws;
            return new(
                isPreGame: true,
                playersTurn: playerGoesFirst,
                playerState: potentialPlayerState,
                opponentState: potentialOpponentState,
                stadiumCard: null
                );
        }

        private static PlayerState ShuffleAndDraw7Cards(PokemonDeck deck)
        {
            // * 2. Shuffle deck.
            IImmutableList<PokemonCard> deckState =
                     DeckUtil.ShuffleDeck(deck)
                    .Select(async id => await CardDataSource.GetCardById(id))
                    .Select(task => task.Result)
                    .ToImmutableList();

            // * 3. Draw 7 cards.
            (IImmutableList<PokemonCard> deckOfCards, IImmutableList<PokemonCard> hand) =
                DeckUtil.DrawCards(deckState, 7);

            return new PlayerState(
                deck: deckOfCards,
                hand: hand,
                active: null,
                bench: ImmutableList<PokemonCardState>.Empty,
                prizes: ImmutableList<PokemonCard>.Empty,
                discardPile: ImmutableList<PokemonCard>.Empty,
                lostZone: ImmutableList<PokemonCard>.Empty,
                hasAttachedEnergyThisTurn: false
                );
        }

        internal static GameState SetUpOpponent(GameState gameState)
        {
            // * 6. Select an active basic Pokemon.
            // TODO maybe max damage and evolution
            PokemonCard active;
            IImmutableList<PokemonCard> basicPokemon = gameState.OpponentState.Hand
                .Where(card => CardUtil.IsBasicPokemon(card))
                .ToImmutableList();
            IDictionary<PokemonCard, int> rank = RankBasicPokemonByHowManyAttacksAreCoveredByEnergy(
                    gameState.OpponentState
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
            basicPokemon = basicPokemon.Remove(active);
            rank.Remove(active);

            //  * 7. For each time step 2 was repeated after 5,
            //  opponent draws 1 card minus any times they had to repeat step 2 after 5.
            PlayerState newOpponentState = gameState.OpponentState
                .AfterDrawingCards(OpponentDraws)
                .AfterMovingFromHandToActive(active)
                .AfterSettingUpPrizes();
            PlayerState newPlayerState = gameState.PlayerState
                .AfterDrawingCards(PlayerDraws)
                .AfterSettingUpPrizes();


            if (basicPokemon.Count == 1)
            {
                newOpponentState = newOpponentState.AfterMovingFromHandToBench(basicPokemon[0]);
            }
            else if(rank.Count > 0)
            {
                // * 8. Put up to 5 Pokemon on the bench.
                maxRank = rank.Max(rank => rank.Value);
                potentialPokemon = rank
                    .Where(rank => rank.Value == maxRank)
                    .Select(rank => rank.Key)
                    .ToImmutableList();
                efficientAttackers = GetFastestEfficientAttackers(potentialPokemon);
                newOpponentState = newOpponentState.AfterMovingFromHandToBench(
                    efficientAttackers
                    .OrderByDescending(rank => rank.Value)
                    .Select(kv => kv.Key)
                    .Take(1)
                    .ToImmutableList()
                    );
            }

            return new GameState(
                isPreGame: false,
                playersTurn: gameState.PlayersTurn,
                playerState: newPlayerState,
                opponentState: newOpponentState,
                stadiumCard: gameState.StadiumCard
                );
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
                    bool enoughEnergyForAttack = AttackUtil.IsEnoughEnergyForAttack(opponentState.Hand, attack);
                    if (enoughEnergyForAttack)
                    {
                        rank[pokemon]++;
                    }
                }
            }
            return rank;
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
