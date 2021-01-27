using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AACalculator
{
    public class BattleResult
    {
        public ImmutableList<RoundResult> Rounds { get; }
        public BattleWinner Winner { get; }

        public Army RemainingArmy => Winner == BattleWinner.Defender ? Rounds.Last().Defender : Rounds.Last().Attacker;

        public BattleResult(IEnumerable<RoundResult> rounds, BattleWinner winner)
        {
            Rounds = rounds.ToImmutableList();
            Winner = winner;
        }
    }
}