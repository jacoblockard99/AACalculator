using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AACalculator.Result
{
    public class BattleResult
    {
        public ImmutableList<RoundResult> Rounds { get; }
        public BattleWinner Winner { get; }
        public Army FinalAttacker { get; }
        public Army FinalDefender { get; }
        public Army RemainingArmy => Winner == BattleWinner.Attacker ? FinalAttacker : FinalDefender;

        public BattleResult(IEnumerable<RoundResult> rounds, BattleWinner winner, Army finalAttacker, Army finalDefender)
        {
            Rounds = rounds.ToImmutableList();
            Winner = winner;
            FinalAttacker = finalAttacker;
            FinalDefender = finalDefender;
        }
    }
}