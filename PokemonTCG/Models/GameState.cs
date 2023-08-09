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
        private PlayerState PlayerState;
        private PlayerState OpponentState;

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

            SetUpOpponent();

        }

        private void SetUpOpponent()
        {
            ImmutableList<PokemonCard> basicPokemon = OpponentState.Hand.Where(
                card => card.Subtypes.Contains(CardSubtype.BASIC)
            ).ToImmutableList();

            PokemonCard active;
            if(basicPokemon.Count == 1)
            {
                active = basicPokemon.First();
            } else
            {
                ImmutableList<PokemonCard> energy = OpponentState.Hand.Where(
                    card => card.Supertype == CardSupertype.Energy
                ).ToImmutableList();

                ImmutableDictionary<PokemonType, int> numberOfEnergies = OpponentState.Hand
                    .GroupBy(card => PokemonCard.GetEnergyType(card))
                    .ToImmutableDictionary(group => group.Key, group => group.Count());

                Dictionary<string, int> rank = new();
                foreach(PokemonCard pokemon in basicPokemon)
                {
                    rank[pokemon.Id] = 0;
                }
                foreach (PokemonCard pokemon in basicPokemon)
                {
                    foreach(Attack attack in pokemon.Attacks)
                    {
                        foreach((PokemonType type, int count) in attack.EnergyCost)
                        {
                            Console.WriteLine();
                        }
                    }
                }

            }

            

        }

        private static PlayerState ShuffleAndDraw7Cards(PokemonDeck deck)
        {
            ImmutableList<string> deckState = deck.Shuffle();
            PlayerState playerState = DeckUtil.DrawCards(deckState, 7);
            return playerState;
        }

    }

}
