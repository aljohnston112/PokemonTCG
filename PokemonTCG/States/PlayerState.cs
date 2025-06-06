﻿using PokemonTCG.CardModels;
using PokemonTCG.Enums;
using PokemonTCG.Utilities;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace PokemonTCG.States
{

    /// <summary>
    /// Part of the <c>GameState</c>
    /// </summary>
    internal class PlayerState
    {
        private static readonly int MAX_BENCH_SIZE = 5;

        internal readonly IImmutableList<PokemonCard> Deck;
        internal readonly IImmutableList<PokemonCard> Hand;
        internal readonly PokemonCardState Active;
        internal readonly IImmutableList<PokemonCardState> Bench;
        internal readonly IImmutableList<PokemonCard> Prizes;
        internal readonly IImmutableList<PokemonCard> DiscardPile;
        internal readonly IImmutableList<PokemonCard> LostZone;
        internal readonly OncePerTurnActionsState OncePerTurnActionsState;
        internal readonly bool HasUsedVStarAbility;

        internal PlayerState(
            IImmutableList<PokemonCard> deck,
            IImmutableList<PokemonCard> hand,
            PokemonCardState active,
            IImmutableList<PokemonCardState> bench,
            IImmutableList<PokemonCard> prizes,
            IImmutableList<PokemonCard> discardPile,
            IImmutableList<PokemonCard> lostZone,
            OncePerTurnActionsState oncePerTurnActionsState,
            bool hasUsedVStarAbility
        )
        {
            Deck = deck;
            Hand = hand;
            Active = active;
            Bench = bench;
            Prizes = prizes;
            DiscardPile = discardPile;
            LostZone = lostZone;
            OncePerTurnActionsState = oncePerTurnActionsState;
            HasUsedVStarAbility = hasUsedVStarAbility;
        }

        internal bool HandHasBasicPokemon()
        {
            // TODO Fossils count as basic Pokemon.
            // Make sure they are labeled as basic or otherwise handled.
            bool hasBasic = false;
            foreach (PokemonCard card in Hand)
            {
                if (CardUtil.IsBasicPokemon(card))
                {
                    hasBasic = true;
                }
            }
            return hasBasic;
        }

        internal PlayerState AfterDrawingCards(int numberToDraw)
        {
            (IImmutableList<PokemonCard> newDeck, IImmutableList<PokemonCard> drawncards) = DeckUtil.DrawCards(
                Deck,
                numberToDraw
                );
            return new PlayerState(
                deck: newDeck,
                hand: Hand.AddRange(drawncards),
                active: Active,
                bench: Bench,
                prizes: Prizes,
                discardPile: DiscardPile,
                lostZone: LostZone,
                oncePerTurnActionsState: OncePerTurnActionsState,
                hasUsedVStarAbility: HasUsedVStarAbility
                );
        }

        internal PlayerState AfterMovingFromHandToActive(PokemonCard newActive)
        {
            Debug.Assert(Hand.Contains(newActive));
            return new PlayerState(
                deck: Deck,
                hand: Hand.Remove(newActive),
                active: new PokemonCardState(newActive),
                bench: Bench,
                prizes: Prizes,
                discardPile: DiscardPile,
                lostZone: LostZone,
                oncePerTurnActionsState: OncePerTurnActionsState,
                hasUsedVStarAbility: HasUsedVStarAbility
                );
        }

        internal PlayerState AfterMovingFromHandToBench(IImmutableList<PokemonCard> benchable)
        {
            // TODO All 4 cards of a V-UNION can be played only from the discard pile and take up one bench spot.
            Debug.Assert(benchable.Count <= (MAX_BENCH_SIZE - Bench.Count));
            PlayerState newPlayerState = this;
            if (benchable.Count > 0)
            {
                foreach (PokemonCard card in benchable)
                {
                    newPlayerState = newPlayerState.AfterMovingFromHandToBench(card);
                }
            }
            return newPlayerState;
        }

        internal PlayerState AfterMovingFromHandToBench(PokemonCard card)
        {
            // TODO All 4 cards of a V-UNION can be played only from the discard pile and take up one bench spot.
            Debug.Assert(MAX_BENCH_SIZE > Bench.Count);
            IImmutableList<PokemonCard> newHand = Hand;
            Debug.Assert(Hand.Contains(card));
            newHand = newHand.Remove(card);
            return new PlayerState(
                deck: Deck,
                hand: newHand,
                active: Active,
                bench: Bench.Add(new PokemonCardState(card)),
                prizes: Prizes,
                discardPile: DiscardPile,
                lostZone: LostZone,
                oncePerTurnActionsState: OncePerTurnActionsState,
                hasUsedVStarAbility: HasUsedVStarAbility
                );
        }

        internal PlayerState AfterSettingUpPrizes(bool isSuddenDeath = false)
        {
            int numberOfPrizes = 6;
            if (isSuddenDeath)
            {
                numberOfPrizes = 1;
            }
            (IImmutableList<PokemonCard> newDeck, IImmutableList<PokemonCard> prizes) = DeckUtil.DrawCards(
                Deck,
                numberOfPrizes
                );
            return new PlayerState(
                deck: newDeck,
                hand: Hand,
                active: Active,
                bench: Bench,
                prizes: prizes,
                discardPile: DiscardPile,
                lostZone: LostZone,
                oncePerTurnActionsState: OncePerTurnActionsState,
                hasUsedVStarAbility: HasUsedVStarAbility
                );
        }

        internal PlayerState AfterEvolvingActivePokemon(PokemonCard evolutionCard)
        {
            // TODO Remove attack effects and status condition.
            // TODO Turn ends if the evolution is a mega evolution or a primal reversion Pokemon.

            CardUtil.AssertCardEvolvesFrom(Active, evolutionCard);
            Debug.Assert(Hand.Contains(evolutionCard));
            Debug.Assert(!Active.FirstTurnInPlay);
            return new PlayerState(
                deck: Deck,
                hand: Hand.Remove(evolutionCard),
                active: Active.AfterEvolvingTo(evolutionCard),
                bench: Bench,
                prizes: Prizes,
                discardPile: DiscardPile,
                lostZone: LostZone,
                oncePerTurnActionsState: OncePerTurnActionsState,
                hasUsedVStarAbility: HasUsedVStarAbility
                );
        }

        internal PlayerState AfterEvolvingBenchedPokemon(PokemonCardState benchedCard, PokemonCard evolutionCard)
        {
            // TODO Remove attack effects and status condition.
            CardUtil.AssertCardEvolvesFrom(benchedCard, evolutionCard);
            Debug.Assert(Bench.Contains(benchedCard));
            Debug.Assert(Hand.Contains(evolutionCard));
            Debug.Assert(!benchedCard.FirstTurnInPlay);
            int iForBench = Bench.IndexOf(benchedCard);
            IList<PokemonCardState> mutableBench = Bench.ToList();
            mutableBench[iForBench] = Bench[iForBench].AfterEvolvingTo(evolutionCard);
            return new PlayerState(
                deck: Deck,
                hand: Hand.Remove(evolutionCard),
                active: Active,
                bench: mutableBench.ToImmutableList(),
                prizes: Prizes,
                discardPile: DiscardPile,
                lostZone: LostZone,
                oncePerTurnActionsState: OncePerTurnActionsState,
                hasUsedVStarAbility: HasUsedVStarAbility
                );
        }

        internal PlayerState AfterAttachingEnergyToActiveFromHand(PokemonType energyType)
        {
            int i = CardUtil.IndexOfEnergyCardWithType(Hand, energyType);
            Debug.Assert(i != -1, $"Energy card with type {energyType} was not found in the hand");

            PokemonCard card = Hand[i];
            return new PlayerState(
                deck: Deck,
                hand: Hand.Remove(card),
                active: Active.AfterAttachingEnergy(card),
                bench: Bench,
                prizes: Prizes,
                discardPile: DiscardPile,
                lostZone: LostZone,
                oncePerTurnActionsState: OncePerTurnActionsState.AfterAttachingEnergy(),
                hasUsedVStarAbility: HasUsedVStarAbility
                );
        }

        internal PlayerState AfterAttachingEnergyToBenchedFromHand(PokemonCardState benchedCard, PokemonType type)
        {
            Debug.Assert(Bench.Contains(benchedCard), $"Card with id {benchedCard.PokemonCard.Id} was not found in the bench");
            int iForBenched = Bench.IndexOf(benchedCard);

            int iForHand = CardUtil.IndexOfEnergyCardWithType(Hand, type);
            Debug.Assert(iForHand != -1, $"Energy card with type {type} was not found in the hand");
            PokemonCard energyCard = Hand[iForHand];

            IList<PokemonCardState> mutableBench = Bench.ToList();
            mutableBench[iForBenched] = Bench[iForBenched].AfterAttachingEnergy(energyCard);
            return new PlayerState(
                deck: Deck,
                hand: Hand.Remove(energyCard),
                active: Active,
                bench: mutableBench.ToImmutableList(),
                prizes: Prizes,
                discardPile: DiscardPile,
                lostZone: LostZone,
                oncePerTurnActionsState: OncePerTurnActionsState.AfterAttachingEnergy(),
                hasUsedVStarAbility: HasUsedVStarAbility
                );
        }

        internal PlayerState AfterDiscardingHand()
        {
            return new PlayerState(
                deck: Deck,
                hand: ImmutableList.Create<PokemonCard>(),
                active: Active,
                bench: Bench,
                prizes: Prizes,
                discardPile: DiscardPile.AddRange(Hand),
                lostZone: LostZone,
                oncePerTurnActionsState: OncePerTurnActionsState,
                hasUsedVStarAbility: HasUsedVStarAbility
                );
        }

        internal PlayerState AfterSwappingActiveWithBenched(PokemonCardState benched)
        {
            Debug.Assert(Bench.Contains(benched));
            return new PlayerState(
                deck: Deck,
                hand: Hand,
                active: benched,
                bench: Bench.Remove(benched).Add(Active),
                prizes: Prizes,
                discardPile: DiscardPile,
                lostZone: LostZone,
                oncePerTurnActionsState: OncePerTurnActionsState,
                hasUsedVStarAbility: HasUsedVStarAbility
                );
        }

        internal PlayerState AfterRetreatingActivePokemon(PokemonCardState benched, IImmutableList<PokemonCard> energy)
        {
            Debug.Assert(Bench.Contains(benched));
            Debug.Assert(energy.Count == Active.PokemonCard.ConvertedRetreatCost);
            return new PlayerState(
                deck: Deck,
                hand: Hand,
                active: benched,
                bench: Bench.Remove(benched).Add(Active.AfterRemovingEnergy(energy)),
                prizes: Prizes,
                discardPile: DiscardPile,
                lostZone: LostZone,
                oncePerTurnActionsState: OncePerTurnActionsState,
                hasUsedVStarAbility: HasUsedVStarAbility
                );
        }

        internal IImmutableList<PokemonCard> GetEvolutionCardsForCard(PokemonCardState card)
        {
            return Hand.Where(handCard => CardUtil.CardEvolvesFrom(card, handCard)).ToImmutableList();
        }

        internal int NumberOfEnergyOnAllPokemon()
        {
            int numberOfEnergy = 0;
            numberOfEnergy += Active.Energy.Count;
            foreach (PokemonCardState benched in Bench)
            {
                numberOfEnergy += benched.Energy.Count;
            }
            return numberOfEnergy;
        }

        internal PlayerState AfterPotentialOpponentMoveToBenchAction(GameState gameState)
        {
            PlayerState opponentState = this;
            PokemonCard bestCardToAddToBench = OpponentUtil.BestCardIfShouldAddToBench(gameState);
            if (bestCardToAddToBench != null)
            {
                opponentState = opponentState.AfterMovingFromHandToBench(bestCardToAddToBench);
            }
            return opponentState;
        }

        internal PlayerState AfterPotentialOpponentEvolutions()
        {
            PlayerState opponentState = this;

            IImmutableList<PokemonCard> evolutionCardsForActive = GetEvolutionCardsForCard(Active);
            PokemonCard potentialEvolution = OpponentUtil.PotentialEvolveOf(Active, evolutionCardsForActive);
            if (potentialEvolution != null)
            {
                opponentState = opponentState.AfterEvolvingActivePokemon(potentialEvolution);
            }

            foreach (PokemonCardState benchCard in Bench)
            {
                IImmutableList<PokemonCard> evolutionCardsForBenched = GetEvolutionCardsForCard(benchCard);
                potentialEvolution = OpponentUtil.PotentialEvolveOf(benchCard, evolutionCardsForBenched);
                if (potentialEvolution != null)
                {
                    opponentState = opponentState.AfterEvolvingBenchedPokemon(benchCard, potentialEvolution);
                }
            }
            return opponentState;
        }

        internal PlayerState AfterPotentialOpponentEnergyAttaching()
        {
            IImmutableDictionary<PokemonType, int> energyInHand = CardUtil.GetNumberOfEachEnergy(Hand);

            Dictionary<Attack, int> energyNeededForAttacks = new();
            foreach (Attack attack in Active.PokemonCard.Attacks)
            {
                energyNeededForAttacks[attack] = AttackUtil.GetEnergyNeededToAttack(attack, Active);
            }
            List<KeyValuePair<Attack, int>> sortedAttackEnergyCost = energyNeededForAttacks.ToList();
            sortedAttackEnergyCost.Sort((x, y) => x.Value.CompareTo(y.Value));


            PlayerState opponentState = this;
            bool found = false;
            int attackIndex = 0;
            while (!found && attackIndex < sortedAttackEnergyCost.Count)
            {
                KeyValuePair<Attack, int> attackCost = sortedAttackEnergyCost[attackIndex];
                Dictionary<PokemonType, int> energyNeededToAttack = new(AttackUtil.GetEnergyNeededToAttack(attackCost.Key, Active.Energy));
                int numberOfColorless = 0;
                if (energyNeededToAttack.ContainsKey(PokemonType.Colorless))
                {
                    numberOfColorless = energyNeededToAttack[PokemonType.Colorless];
                }
                energyNeededToAttack.Remove(PokemonType.Colorless);

                bool needToAddEnergy = energyNeededToAttack.Count > 0 || numberOfColorless > 0;
                if (needToAddEnergy)
                {
                    int typeIndex = 0;
                    while (typeIndex < energyNeededToAttack.Keys.Count && !found)
                    {
                        PokemonType type = energyNeededToAttack.Keys.ToList()[typeIndex];
                        if (energyInHand.ContainsKey(type))
                        {
                            found = true;
                            opponentState = opponentState.AfterAttachingEnergyToActiveFromHand(type);
                        }
                        typeIndex++;
                    }
                    if (!found && numberOfColorless > 0)
                    {
                        found = true;
                        int maxEnergy = energyInHand.ToList().Max(kv => kv.Value);
                        PokemonType type = energyInHand.Where(kv => kv.Value == maxEnergy).First().Key;
                        opponentState = opponentState.AfterAttachingEnergyToActiveFromHand(type);
                    }
                }
                attackIndex++;
            }
            return opponentState;
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