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
    internal class GameTemplate
    {

        private int PlayerDraws = 0;
        private int OpponentDraws = 0;
        private bool PlayerTurn;

        private readonly TurnTemplate PlayerTurnTemplate;
        private readonly  TurnTemplate OpponentTurnTemplate;

        internal static bool FlipCoin()
        {
            return new Random().Next(2) == 0;
        }

        internal GameState SetUpGame(
            PokemonDeck playerDeck,
            PokemonDeck opponentDeck
            )
        {
            PlayerTurn = FlipCoin();

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
                OpponentDraws++;
                PlayerDraws++;
            }

            // Shuffle and draw until both players have a basic Pokemon
            if (!playerHasBasic)
            {
                while (!playerHasBasic)
                {
                    potentialPlayerState = ShuffleAndDraw7Cards(playerDeck);
                    playerHasBasic = potentialPlayerState.HandHasBasicPokemon();
                    OpponentDraws++;
                }
            }

            if (!opponentHasBasic)
            {
                while (!opponentHasBasic)
                {
                    potentialOpponentState = ShuffleAndDraw7Cards(opponentDeck);
                    opponentHasBasic = potentialOpponentState.HandHasBasicPokemon();
                    PlayerDraws++;
                }
            }
            if (PlayerDraws > OpponentDraws)
            {
                OpponentDraws = 0;
            }
            else {
                PlayerDraws = 0;
            }
            Debug.Assert(potentialOpponentState != null && potentialPlayerState != null);
            return new GameState(potentialPlayerState, potentialOpponentState);
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
                lostZone : ImmutableList<PokemonCard>.Empty
                );
        }

        internal GameState SetUpOpponent(GameState gameState)
        {
            IImmutableList<PokemonCard> basicPokemon = gameState.OpponentState.Hand
                .Where(card => CardUtil.IsBasicPokemon(card)
            ).ToImmutableList();

            PlayerState newOpponentState = gameState.OpponentState;
            PokemonCard active;
            if (basicPokemon.Count == 1)
            {
                active = basicPokemon[0];
            }
            else
            {
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
            newOpponentState = newOpponentState.DrawCards(OpponentDraws);
            return new GameState(gameState.PlayerState, newOpponentState);
            // TODO maybe max damage and evolution
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

        internal GameState OnPlayerSelectedActivePokemon(GameState gameState)
        {
            PlayerState playerState = gameState.PlayerState.DrawCards(PlayerDraws);
            return new GameState(playerState, gameState.OpponentState);
        }

        internal GameState NextTurn(GameState gameState)
        {
            GameState newGameState;
            if (PlayerTurn)
            {
                newGameState = PlayerTurnTemplate.NextTurn(gameState);
                PlayerTurn = false;
            } else
            {
                newGameState = OpponentTurnTemplate.NextTurn(gameState);
                PlayerTurn = true;
            }
            return newGameState;
        }

    }
}

/*
 * 1. Pick who goes first randomly.
 * 2. Shuffle deck.
 * 3. Draw 7 cards.
 * 4. Check for basic Pokemon. Fossils count as basic Pokemon.
 * 5. If no basic Pokemon, go to 2 after opponent reaches step 6.
 * 6. Select an active basic Pokemon.
 * 7. For each time step 2 was repeated after 5, opponent draws 1 card minus any times they had to repeat step 2 after 5.
 * 8. Put up to 5 Pokemon on the bench.
 * 9. Reveal all Pokemon in play.
 * 10. First turn.
 * 11. Second player's turn.
 * 12. First player's turn. Go to 11 unless the game ends.
 *          The game ends when one player takes all their prizes, 
 *          does not have any Pokemon left in play, 
 *          or has no cards left to draw at the beginning of their turn.
 * Sudden death happens if both players win at the same time, unless one meets two of the conditions and the other does not.
 */
