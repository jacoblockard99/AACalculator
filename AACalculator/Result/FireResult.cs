using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AACalculator.Result
{
    public class FireResult
    {
        public ImmutableDictionary<UnitType, ImmutableList<HitResult>> Hits { get; }

        public decimal TotalHits => _totalHits.Value;
        public decimal TotalEffectiveHits => _totalEffectiveHits.Value;
        public decimal TotalIneffectiveHits => _totalIneffectiveHits.Value;

        private readonly Lazy<decimal> _totalHits;
        private readonly Lazy<decimal> _totalEffectiveHits;
        private readonly Lazy<decimal> _totalIneffectiveHits;

        public FireResult(IDictionary<UnitType, IEnumerable<HitResult>> hits)
        {
            Hits = hits.ToImmutableDictionary(p => p.Key, p => p.Value.ToImmutableList());
            _totalHits =
                new Lazy<decimal>(() => SumHits(FlattenedHits()));
            _totalEffectiveHits =
                new Lazy<decimal>(() => SumHits(FlattenedHits().Where(h => h.Effective)));
            _totalIneffectiveHits =
                new Lazy<decimal>(() => SumHits(FlattenedHits().Where(h => h.Ineffective)));
        }

        public override string ToString()
        {
            var ineffective = TotalIneffectiveHits > 0 ? $" ({TotalIneffectiveHits:0.###} ineffective)" : "";
            return $"{TotalEffectiveHits:0.###}" + ineffective;
        }

        private IEnumerable<HitResult> FlattenedHits()
        {
            return Hits.SelectMany(p => p.Value);
        }

        private static decimal SumHits(IEnumerable<HitResult> hits)
        {
            return hits.Sum(h => h.Amount);
        }
    }
}