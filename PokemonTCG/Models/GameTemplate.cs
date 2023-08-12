using PokemonTCG.DataSources;
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
            Debug.Assert(potentialOpponentState != null && potentialPlayerState != null);
            return new GameState(potentialPlayerState, potentialOpponentState);
        }

        private static PlayerState ShuffleAndDraw7Cards(PokemonDeck deck)
        {
            ImmutableList<PokemonCard> deckState = deck.Shuffle().Select(id => CardDataSource.GetCardById(id)).ToImmutableList();
            (ImmutableList<PokemonCard> deckOfCards, ImmutableList<PokemonCard> hand) = DeckUtil.DrawCards(deckState, 7);
            return new PlayerState(
                deckOfCards,
                hand,
                null,
                ImmutableList<PokemonCard>.Empty,
                ImmutableList<PokemonCard>.Empty,
                ImmutableList<PokemonCard>.Empty,
                ImmutableList<PokemonCard>.Empty
                );
        }

        internal GameState SetUpOpponent(GameState gameState)
        {
            ImmutableList<PokemonCard> basicPokemon = gameState.OpponentState.Hand.Where(
                card => PokemonCard.IsBasicPokemon(card)
            ).ToImmutableList();

            PokemonCard active;
            PlayerState newOpponentState = gameState.OpponentState;
            if (basicPokemon.Count == 1)
            {
                active = basicPokemon.First();
            }
            else
            {
                IDictionary<PokemonCard, int> rank = RankBasicPokemonByHowManyAttacksAreCoveredByEnergy(gameState);
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
            newOpponentState = newOpponentState.DrawCards(OpponentDraws);
            return new GameState(gameState.PlayerState, newOpponentState);
            // TODO maybe max damage and evolution
        }

        private static Dictionary<PokemonCard, int> GetDamagePerEnergy(ImmutableList<PokemonCard> potentialPokemon)
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

        private static IDictionary<PokemonCard, int> RankBasicPokemonByHowManyAttacksAreCoveredByEnergy(GameState gameState)
        {
            Dictionary<PokemonCard, int> rank = new();

            ImmutableList<PokemonCard> basicPokemon = gameState.OpponentState.Hand.Where(
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
                    bool enoughEnergyForAttack = HasEnoughEnergyForAttack(gameState, attack);
                    if (enoughEnergyForAttack)
                    {
                        rank[pokemon]++;
                    }
                }
            }
            return rank;
        }

        private static bool HasEnoughEnergyForAttack(GameState gameState, Attack attack)
        {
            bool enoughEnergyForAttack = true;

            // Count energy cards from hand
            ImmutableDictionary<PokemonType, int> numberOfEveryEnergy = gameState.OpponentState.Hand
                .Where(card => card.Supertype == CardSupertype.Energy)
                .GroupBy(card => PokemonCard.GetEnergyType(card))
                .ToImmutableDictionary(group => group.Key, group => group.Count());
            int numberOfEnergies = gameState.OpponentState.Hand
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
            } else
            {
                newGameState = OpponentTurnTemplate.NextTurn(gameState);
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
