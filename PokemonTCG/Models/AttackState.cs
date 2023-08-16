using System;

namespace PokemonTCG.Models
{

    internal class AttackState
    {

        internal readonly Func<int> DamageModifier;
        internal readonly Func<int> AfterWeaknessAndResistanceDamageModifier;

    }

}

/*
 * 3. Attack; Attacks that say to put damage tokens on another Pokemon do not have any damage calulations. 
 *         "Up to" mean between 1 and N. 
 *         "Any" mean 0 or more.
 *         The first player cannot attack on the first turn.
 *         Makes sure energy requirments are met.
 *         Sleep, confusion and paralysis replace one another.
 *         A player can only use one GX attack per game
 *      a. Apply your effects and any trainer cards that alter the attack; 
 *         Attack effects go away when any active Pokemon goes to the bench.
 *      b. Check if confusion inflicted. Flip a coin to see if you take 30 damage. Card is upside down.
 *      c. Take weakness into account, except for benched Pokemon.
 *      d. Take resistance into account, except for benched Pokemon.
 *      e. Perform attack choices.
 *      f. Do what the attack says.
 *      g. Apply opponent effects and any trainer that alters damage.
 *      h. Apply all other effects.
 *      i. Do damage
 */