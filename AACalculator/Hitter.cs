using System.Collections.Generic;
using System.Linq;
using AACalculator.Result;

namespace AACalculator
{
    public class Hitter
    {
        public static IEnumerable<HitResult> Hit(Army army, UnitType firer, Army firingArmy, decimal amt, bool attacker, IHitSelector selector)
        {
            // If the army is empty, no hits are necessary. Return such.
            if (army.Empty) return NoHits();

            var type = selector.Select(army, firer, firingArmy, amt, attacker);

            // Check if the hitType is null. If so, no valid hits are possible. Thus, return an ineffective hit result for the appropriate amount.
            if (type == null) return OneHit(HitResult.NewIneffective(amt));

            // Take the appropriate number of casualties from the army and capture the result.
            var result = army.Hit(type, amt);

            // Check if the resulting hit completed the required amount.
            if (result.Amount == amt)
                // If so, simply return that hit result.
                return OneHit(result);
            else
                // If not, then further hits need to be made. Recursively take away the remaining number of causualties, adding the results
                // to the current one.
                return Hit(army, firer, firingArmy, amt - result.Amount, attacker, selector).Prepend(result);
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