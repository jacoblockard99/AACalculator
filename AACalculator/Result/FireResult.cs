using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AACalculator.Result
{
    /// <summary>
    /// An immutable representation of the result of an army firing against another.
    /// </summary>
    public class FireResult
    {
        /// <summary>
        /// An immutable dictionary that contains the hits made by the firing army. The key is the type of unit that fired, and the value is
        /// an immutable list of <see cref="AACalculator.Result.HitResult"/>s containing all the hits made by the unit.
        /// </summary>
        public ImmutableDictionary<UnitType, ImmutableList<HitResult>> Hits { get; }

        /// <summary>
        /// The total number of hits, both effective and ineffective, made by the firing army, lazily evaluated.
        /// </summary>
        public decimal TotalHits => _totalHits.Value;

        /// <summary>
        /// The total number of effective hits made by the firing army, lazily evaluated.
        /// </summary>
        public decimal TotalEffectiveHits => _totalEffectiveHits.Value;

        /// <summary>
        /// The total number of ineffective hits made by the firing army, lazily evaluated.
        /// </summary>
        public decimal TotalIneffectiveHits => _totalIneffectiveHits.Value;

        private readonly Lazy<decimal> _totalHits;
        private readonly Lazy<decimal> _totalEffectiveHits;
        private readonly Lazy<decimal> _totalIneffectiveHits;

        /// <summary>
        /// Constructs a new <see cref="FireResult"/> with the given <see cref="IDictionary{UnitType, IEnumerable{HitResult}}"/> of hits.
        /// </summary>
        /// <param name="hits">The dictionary of hits. Note that it will be converted to an <see cref="ImmutableDictionary"/>.</param>
        public FireResult(IDictionary<UnitType, IEnumerable<HitResult>> hits)
        {
            // Convert the hits parameter to an immutable dictionary.
            Hits = hits.ToImmutableDictionary(p => p.Key, p => p.Value.ToImmutableList());

            // Instantiate the lazy hit count variables.
            _totalHits =            new Lazy<decimal>(() => SumHits(FlattenedHits()));
            _totalEffectiveHits =   new Lazy<decimal>(() => SumHits(FlattenedHits().Where(h => h.Effective)));
            _totalIneffectiveHits = new Lazy<decimal>(() => SumHits(FlattenedHits().Where(h => h.Ineffective)));
        }

        /// <summary>
        /// Gets a string representation of the <see cref="FireResult"/> by using the number of effective hits and placing the number of
        /// ineffective hits in parentheses if applicable.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            var ineffective = TotalIneffectiveHits > 0 ? $" ({TotalIneffectiveHits:0.###} ineffective)" : "";
            return $"{TotalEffectiveHits:0.###}" + ineffective;
        }

        /// <summary>
        /// Returns an empty <see cref="FireResult"/> if the input is null, or simply the input itself otherwise.
        /// </summary>
        /// <param name="input">The input to handle safely.</param>
        /// <returns>An empty <see cref="FireResult"/> if the input is null; the input itself otherwise.</returns>
        public static FireResult Safe(FireResult input)
        {
            return input ?? new FireResult(new Dictionary<UnitType, IEnumerable<HitResult>>());
        }

        /// <summary>
        /// Gets a flattened list of <see cref="AACalculator.Result.HitResult"/>s.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<HitResult> FlattenedHits()
        {
            return Hits.SelectMany(p => p.Value);
        }

        /// <summary>
        /// Calculates the total number of hits represented by a list of <see cref="AACalculator.Result.HitResult"/>s.
        /// </summary>
        /// <param name="hits">The list of <see cref="AACalculator.Result.HitResult"/>s to sum.</param>
        /// <returns>A decimal representing the total hits.</returns>
        private static decimal SumHits(IEnumerable<HitResult> hits)
        {
            return hits.Sum(h => h.Amount);
        }
    }
}