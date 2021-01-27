using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AACalculator
{
    public class BattleResult
    {
        public ImmutableList<RoundResult> Rounds { get; }
        public BattleWinner Winner { get; }
        public Army RemainingArmy { get; }

        public BattleResult(IEnumerable<RoundResult> rounds, BattleWinner winner, Army remainingArmy)
        {
            Rounds = rounds.ToImmutableList();
            Winner = winner;
            RemainingArmy = remainingArmy;
        }
    }
}