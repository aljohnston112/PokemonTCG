using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using PokemonTCG.Utilities;

namespace PokemonTCG.Models
{

    /// <summary>
    /// Part of the <c>GameState</c>
    /// </summary>
    internal class PlayerState
    {
        private static readonly int MAX_BENCH_SIZE = 5;

        internal readonly ImmutableList<PokemonCard> Deck;
        internal readonly ImmutableList<PokemonCard> Hand;
        internal readonly PokemonCardState Active;
        internal readonly ImmutableList<PokemonCardState> Bench;
        internal readonly ImmutableList<PokemonCard> Prizes;
        internal readonly ImmutableList<PokemonCard> DiscardPile;
        internal readonly ImmutableList<PokemonCard> LostZone;

        internal PlayerState(
            ImmutableList<PokemonCard> deck,
            ImmutableList<PokemonCard> hand,
            PokemonCardState active,
            ImmutableList<PokemonCardState> bench,
            ImmutableList<PokemonCard> prizes,
            ImmutableList<PokemonCard> discardPile,
            ImmutableList<PokemonCard> lostZone
        )
        {
            Deck = deck;
            Hand = hand;
            Active = active;
            Bench = bench;
            Prizes = prizes;
            DiscardPile = discardPile;
            LostZone = lostZone;
        }

        internal PlayerState(
            ImmutableList<PokemonCard> deck,
            ImmutableList<PokemonCard> hand,
            PokemonCard active,
            ImmutableList<PokemonCard> bench,
            ImmutableList<PokemonCard> prizes,
            ImmutableList<PokemonCard> discardPile,
            ImmutableList<PokemonCard> lostZone
        ) : this(
            deck,
            hand,
            new PokemonCardState(active),
            bench.Select(card => new PokemonCardState(card)).ToImmutableList(),
            prizes,
            discardPile,
            lostZone
            )
        { }

        internal bool HandHasBasicPokemon()
        {
            bool hasBasic = false;
            foreach (PokemonCard card in Hand)
            {
                if (PokemonCard.IsBasicPokemon(card))
                {
                    hasBasic = true;
                }
            }
            return hasBasic;
        }

        internal PlayerState MoveFromHandToActive(PokemonCard active)
        {
            Debug.Assert(Hand.Contains(active));
            return new PlayerState(Deck, Hand.Remove(active), new PokemonCardState(active), Bench, Prizes, DiscardPile, LostZone);
        }

        internal PlayerState MoveFromHandToBench(IList<PokemonCard> benchable)
        {
            // TODO All 4 cards of a V-UNION can be played only from the discard pile and take up one bench spot.
            Debug.Assert(benchable.Count < (MAX_BENCH_SIZE - Bench.Count));
            ImmutableList<PokemonCard> newHand = Hand;
            foreach (PokemonCard card in benchable)
            {
                Debug.Assert(Hand.Contains(card));
                newHand = newHand.Remove(card);
            }
            return new PlayerState(Deck, newHand, Active, Bench.AddRange(benchable.Select(card => new PokemonCardState(card))), Prizes, DiscardPile, LostZone);
        }

        internal PlayerState SetUpPrizes(bool isSuddenDeath = false)
        {
            int numberOfPrizes = 6;
            if (isSuddenDeath)
            {
                numberOfPrizes = 1;
            }
            (ImmutableList<PokemonCard> deck, ImmutableList<PokemonCard> prizes) = DeckUtil.DrawCards(Deck, numberOfPrizes);
            return new PlayerState(deck, Hand, Active, Bench, prizes, DiscardPile, LostZone);
        }

        internal PlayerState DrawCards(int numberOfDraws)
        {
            (ImmutableList<PokemonCard> deck, ImmutableList<PokemonCard> drawn) = DeckUtil.DrawCards(Deck, numberOfDraws);
            return new PlayerState(deck, Hand.AddRange(drawn), Active, Bench, Prizes, DiscardPile, LostZone);
        }

        private static int IndexOfCard(ImmutableList<PokemonCardState> cardStates, string benchedCardId)
        {
            bool found = false;
            int i = -1;
            while (i < cardStates.Count && !found)
            {
                i++;
                PokemonCard card = cardStates[i].PokemonCard;
                if (card.Id == benchedCardId)
                {
                    found = true;
                }
            }
            return i;
        }

        private static int IndexOfCard(ImmutableList<PokemonCard> cards, string benchedCardId)
        {
            bool found = false;
            int i = -1;
            while (i < cards.Count && !found)
            {
                i++;
                PokemonCard card = cards[i];
                if (card.Id == benchedCardId)
                {
                    found = true;
                }
            }
            return i;
        }

        internal PlayerState EvolveActivePokemon(string evolvedCardId)
        {
            // TODO Remove attack effects and status condition.
            int i = IndexOfCard(Hand, evolvedCardId);
            if (i == -1)
            {
                throw new ArgumentException($"The hand did not contain the specified id: {evolvedCardId}");
            }

            PokemonCard card = Hand[i];
            string lowerStageName = Active.PokemonCard.Name;
            if (lowerStageName != card.EvolvesFrom)
            {
                throw new ArgumentException($"Card with id: {evolvedCardId} does not evolve from {lowerStageName}");
            }
            return new PlayerState(Deck, Hand.Remove(card), Active.EvolveTo(card), Bench, Prizes, DiscardPile, LostZone);
        }

        internal PlayerState EvolveBenchedPokemon(string benchedCardId, string evolvedCardId)
        {
            // TODO Remove attack effects and status condition.
            int iForBench = IndexOfCard(Bench, benchedCardId);
            if (iForBench == -1)
            {
                throw new ArgumentException($"Bench did not contain a card with id: {benchedCardId}");
            }

            int iForHand = IndexOfCard(Hand, evolvedCardId);
            if (iForHand == -1)
            {
                throw new ArgumentException($"The hand did not contain the specified id: {evolvedCardId}");
            }

            PokemonCard card = Hand[iForHand];
            List<PokemonCardState> mutableBench = Bench.ToList();
            mutableBench[iForBench] = Bench[iForBench].EvolveTo(card);
            return new PlayerState(Deck, Hand.Remove(card), Active, mutableBench.ToImmutableList(), Prizes, DiscardPile, LostZone);
        }

        internal PlayerState AttachEnergyToActiveFromHand(string energyCardId)
        {
            int i = IndexOfCard(Hand, energyCardId);
            if (i == -1)
            {
                throw new ArgumentException($"Energy card with id {energyCardId} was not found in the hand");
            }

            PokemonCard card = Hand[i];
            return new PlayerState(Deck, Hand.Remove(card), Active.AttachEnergy(card), Bench, Prizes, DiscardPile, LostZone);
        }

        internal PlayerState AttachEnergyToBenchedFromHand(string benchedCardId, string energyCardId)
        {
            int iForBenched = IndexOfCard(Bench, benchedCardId);
            if (iForBenched == -1)
            {
                throw new ArgumentException($"Card with id {benchedCardId} was not found in the bench");
            }

            int iForHand = IndexOfCard(Hand, energyCardId);
            if (iForHand == -1)
            {
                throw new ArgumentException($"Energy card with id {energyCardId} was not found in the hand");
            }

            PokemonCard energyCard = Hand[iForHand];
            List<PokemonCardState> mutableBench = Bench.ToList();
            mutableBench[iForBenched] = Bench[iForBenched].AttachEnergy(energyCard);
            return new PlayerState(Deck, Hand.Remove(energyCard), Active, mutableBench.ToImmutableList(), Prizes, DiscardPile, LostZone);
        }

    }

}

/*
 * 1. Draw a card. If you have no cards to draw you lose.
 * 2. Do any of the following in any order.
 *      a. Put any number of basic Pokemon on the bench. 
 *              All 4 cards of a V-UNION can be played only from the discard pile and take up one bench spot.
 *      b. Evolve a Pokemon who has a name that matches the "evolves from" of another card. Cannot be done on a Pokemon's first turn in play.
 *                  Level is not part of a card name. 
 *                  Symbols are except δ (Delta Species). 
 *                  Owner is part of the name. Form is part of the name.
 *                  "-Ex" is not the same as "ex".
 *                  Turn ends if the evolution is a mega evolution or a primal reversion Pokemon.
 *              i. Remove attack effects and status condition.
 *              ii. Evolve
 *      c. Attach one energy to any of your in play Pokemon.
 *      d. Play any number of trainers, but only one Supporter and one Stadium. 
 *              Supporters cannot be played on the first player's first turn.
 *              Stadium cards replace any stadium cards in play. You can't play a stadium that is already active.
 *      e. Retreat the active Pokemon once. Remove energy for retreat cost. Asleep or paralyzed Pokemon cannot retreat.
 *              Attack effects and status conditions are cleared from the retreating Pokemon.
 *      f. Use any amount of abilities. 
 *              The active and all benched Pokemon abilities are fair game. 
 *              A player can only use one VSTAR per game.
 *      g. Use "any amount"? of ancient traits. Ancient traits do not count as abilities.
 * 
 * 3. Attack; Attacks that say to put damage tokens on another Pokemon do not have any damage calulations. 
 *         "Up to" mean between 1 and N. 
 *         "Any" mean 0 or more.
 *         The first player cannot attack on the first turn.
 *         Makes sure energy requirments are met.
 *         Sleep, confusion and paralysis replace one another.
 *         A player can only use one GX attack per game
 *      a. Apply your effects and any trainer cards that alter the attack; Attack effects go away when any active Pokemon goes to the bench.
 *      b. Check if confusion inflicted. Flip a coin to see if you take 30 damage. Card is upside down.
 *      c. Take weakness into account, except for benched Pokemon.
 *      d. Take resistance into account, except for benched Pokemon.
 *      e. Perform attack choices.
 *      f. Do what the attack says.
 *      g. Apply opponent effects and any trainer that alters damage.
 *      h. Apply all other effects.
 *      i. Do damage
 * 
 * 4. If a opponent's Pokemon is knocked out, take a prize. 
 *          Take another if the Pokemon was 
 *              a Pokemon ex,
 *              a tera Pokemon ex,
 *              a Pokemon VSTAR,
 *              a Pokemon V,
 *              a Pokemon GX,
 *              a Pokemon-EX,
 *              a Mega evolution Pokemon,
 *              a primal reversion Pokemon
 *          or Take 2 more if the Pokemon was 
 *              a Pokemon V-UNION,
 *              a Pokemon VMAX,
 *              a TAG TEAM
 * 5. Pokemon checkup
 *      a. Poison. 10 damage.
 *      b. Burn. 20 damage. Flip a coin to see if healed.
 *      c. Sleep. Flip a coin to see if awakened. Card top facing left.
 *      d. Paralysis. Heals after owner's next turn. Card top facing right.
 *      e. Abilitiy and trainer effects.
 *      f. Other Pokemon checkup and between turn effects.
 *      a-d and e-f can be switched, so e-f then a-d.
 *      g. Take prizes for any knocked out Pokemon.
 *      
 * Special rules.
 *      Tera Pokemon ex do not take any damamge while benched.
 *      VSTAR are considered V Pokemon.
 *      V-UNION cards are not basic, stage 1, stage 2, or evolution Pokemon
 *      Individual V-UNION cards do not any attributes other than types, catergory, and name.
 *      V-UNION cards count as Pokemon V.
 *      Prism cards go to the loast zone instead of the dicard pile.
 *      Restored Pokemon are not basic Pokemon, but are considered unevolved.
 */
