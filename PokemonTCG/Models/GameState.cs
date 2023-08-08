using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonTCG.DataSources;
using PokemonTCG.View;

namespace PokemonTCG.Models
{
    /// <summary>
    /// The state of the game. To be used for the <c>GamePage</c>
    /// </summary>
    public class GameState
    {
        private PlayerState PlayerState1;
        private PlayerState PlayerState2;

        /// <summary>
        /// Creates a new <c>GameState</c>
        /// </summary>
        public GameState(PokemonDeck playerDeck, PokemonDeck opponentDeck)
        {
            Random random = new();
            bool playerFirst = random.Next(2) == 0;

            bool playerHasBasic = false;
            DeckState playerDeckState;
            IList<PokemonCard> playerDrawnCards;

            bool opponentHasBasic = false;
            DeckState opponentDeckState;
            IList<PokemonCard> opponentDrawnCards;

            // Shuffle and draw until at least one player has a basic Pokemon
            while (!playerHasBasic && !opponentHasBasic)
            {
                (playerHasBasic, playerDeckState, playerDrawnCards) = ShuffleAndDraw7Cards(playerDeck);
                (opponentHasBasic, opponentDeckState, opponentDrawnCards) = ShuffleAndDraw7Cards(opponentDeck);
            }

            // Shuffle and draw until both players have a basic Pokemon
            int opponentDraws = 0;
            if(!playerHasBasic)
            {
                while (!playerHasBasic)
                {
                    (playerHasBasic, playerDeckState, playerDrawnCards) = ShuffleAndDraw7Cards(playerDeck);
                    opponentDraws++;
                }
            }
            int playerDraws = 0;
            if (!opponentHasBasic)
            {
                while (!opponentHasBasic)
                {
                    (opponentHasBasic, opponentDeckState, opponentDrawnCards) = ShuffleAndDraw7Cards(opponentDeck);
                    playerDraws++;
                }
            }

            // Set up player
            // Set up opponent

        }

        private static (bool, DeckState, IList<PokemonCard>) ShuffleAndDraw7Cards(PokemonDeck deck)
        {
            bool hasBasic = false;
            DeckState deckState = new(deck.Shuffle());
            (deckState, IList<string> drawnIds) = deckState.DrawCards(7);
            IList<PokemonCard> drawnCards = new List<PokemonCard>();
            foreach (string cardId in drawnIds)
            {
                PokemonCard card = CardDataSource.GetCardById(cardId);
                if (card.Subtypes.Contains(CardSubtype.BASIC))
                {
                    hasBasic = true;
                }
                drawnCards.Add(card);
            }
            return (hasBasic, deckState, drawnCards);
        }

    }

}
