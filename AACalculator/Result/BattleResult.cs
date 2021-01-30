using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AACalculator.Result
{
    /// <summary>
    /// A representation of the result of a single battle between two armies.
    /// </summary>
    public class BattleResult
    {
        /// <summary>
        /// An immutable list containing the results of each combat round in the battle.
        /// </summary>
        public ImmutableList<RoundResult> Rounds { get; }

        /// <summary>
        /// The winner of the battle.
        /// </summary>
        public BattleWinner Winner { get; }

        /// <summary>
        /// The final attacking army left over after the battle.
        /// </summary>
        public Army FinalAttacker { get; }

        /// <summary>
        /// The final defending army left over after the battle.
        /// </summary>
        public Army FinalDefender { get; }

        /// <summary>
        /// The final remaining army in a battle with an actual winner. If the battle was a tie, or if no winner was declared, null is returned.
        /// </summary>
        public Army RemainingArmy => Winner == BattleWinner.Attacker ? FinalAttacker :
                                     Winner == BattleWinner.Defender ? FinalDefender : null;

        /// <summary>
        /// Constructs a new <see cref="BattleResult"/> with the given list of <see cref="RoundResult"/>s, winner, final attacker, and final
        /// defender.
        /// </summary>
        /// <param name="rounds">The list of round results. Note that it will be converted to an <see cref="ImmutableList"/>.</param>
        /// <param name="winner">The winner of the battle.</param>
        /// <param name="finalAttacker">The final attacking army left over after the battle; may be empty.</param>
        /// <param name="finalDefender">The final defending army left over after the battle; may be empty.</param>
        public BattleResult(IEnumerable<RoundResult> rounds, BattleWinner winner, Army finalAttacker, Army finalDefender)
        {
            Rounds = rounds.ToImmutableList();
            Winner = winner;
            FinalAttacker = finalAttacker;
            FinalDefender = finalDefender;
        }
    }
}