using System;
using System.Collections.Generic;
using System.Linq;
using AACalculator.Result;

namespace AACalculator
{
    public class HitSelectorByScore : IHitSelector
    {
        public IEnumerable<HitResult> Hit(Army army, UnitType firer, Army firingArmy, decimal amt, bool attacker)
        {
            // If the army is empty, no hits are necessary. Return such.
            if (army.Empty) return NoHits();
            
            UnitType hitType = null;

            // Iterate through each unit type in the sustaining army.
            foreach (var type in army.Units.Keys)
            {
                // Check if the current arrangement would be a valid hit. If not, immediately try the next arrangement.
                if (!HitValidator.ValidHit(firer, type, firingArmy)) continue;

                // Retrieve the score of the firing unit.
                var score = type.Score(attacker);

                // Check if the current arrangement is better than the currently stored one. It is better/less valuable if:
                //   1) The stored hitType is null, meaning that none have even been stored yet,
                //   2) The current firing score is worse than the stored one, or
                //   3) The current firing scores are equal, but the cost of the current one is lower than that of the stored one.
                // If the current arrangement is better, store it.
                if (hitType == null || score < hitType.Score(attacker) || score == hitType.Score(attacker) && type.Cost < hitType.Cost)
                    hitType = type;
            }

            // Check if the hitType is null. If so, no valid hits are possible. Thus, return an ineffective hit result for the appropriate amount.
            if (hitType == null) return OneHit(HitResult.NewIneffective(amt));

            // Take the appropriate number of casualties from the army and capture the result.
            var result = army.Hit(hitType, amt);

            // Check if the resulting hit completed the required amount.
            if (result.Amount == amt)
                // If so, simply return that hit result.
                return OneHit(result);
            else
                // If not, then further hits need to be made. Recursively take away the remaining number of causualties, adding the results
                // to the current one.
                return Hit(army, firer, firingArmy, amt - result.Amount, attacker).Prepend(result);
        }

        /// <summary>
        /// Returns an appropriate <see cref="IEnumerable{T}"/> to represent no hits occurring.
        /// </summary>
        /// <returns>An appropriate <see cref="IEnumerable{T}"/> to represent no hits occurring.</returns>
        private static IEnumerable<HitResult> NoHits()
        {
            return new List<HitResult>();
        }

        /// <summary>
        /// Returns an appropriate <see cref="IEnumerable{T}"/> to represent only one hit--the given one--occurring.
        /// </summary>
        /// <param name="result">The given hit result.</param>
        /// <returns>An appropriate <see cref="IEnumerable{T}"/> to represents only the given hit occurring.</returns>
        private static IEnumerable<HitResult> OneHit(HitResult result)
        {
            return new List<HitResult> { result };
        }
    }
}