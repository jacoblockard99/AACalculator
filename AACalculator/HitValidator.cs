using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AACalculator
{
    /// <summary>
    /// Contains methods to validate a hit.
    /// </summary>
    public class HitValidator
    {
        /// <summary>
        /// Determines whether the given hit from one unit type in the given army on the other given unit type is valid.
        /// </summary>
        /// <param name="firer">The firing unit type.</param>
        /// <param name="sustainer">The unit type being fired upon.</param>
        /// <param name="firingArmy">The army of which the firing unit type is a part.</param>
        /// <returns></returns>
        public static bool ValidHit(UnitType firer, UnitType sustainer, Army firingArmy)
        {
            // Ensure that two conditions are met:
            //   1) A submarine must never fire upon an air unit, and
            //   2) An air unit cannot fire upon a submarine unless a destroyer is present on their side.
            return CheckSub(firer, sustainer) && CheckAir(firer, sustainer, firingArmy);
        }

        /// <summary>
        /// Ensures that a submarine cannot fire upon an air unit.
        /// </summary>
        /// <param name="firer">The firing unit type.</param>
        /// <param name="sustainer">The unit type being fired upon.</param>
        /// <returns>True if the hit is not a submarine-to-air hit; false otherwise.</returns>
        private static bool CheckSub(UnitType firer, UnitType sustainer)
        {
            // For this to not be a submarine-to-air hit, one of two conditions must be met:
            //   1) The firing unit type is not a submarine, or
            //   2) The unit type being fired upon is not an air unit.
            // In either case, the hit would not be a submarine-to-air hit.
            return firer != UnitType.Submarine || sustainer.Category != UnitCategory.Air;
        }

        /// <summary>
        /// Ensures that an air unit cannot fire upon a submarine without a destroyer.
        /// </summary>
        /// <param name="firer">The firing unit type.</param>
        /// <param name="sustainer">The unit type being fired upon.</param>
        /// <param name="firingArmy">The army to which the firing unit belongs.</param>
        /// <returns>True if the hit is not an air-to-submarine hit or if a destroyer is present in the firing army; false otherwise.</returns>
        private static bool CheckAir(UnitType firer, UnitType sustainer, Army firingArmy)
        {
            // One of two conditions must be met for this to not be an air-to-submarine hit at all:
            //   1) The firing unit type is not an air unit, or
            //   2) The unit type being fired upon is not a submarine.
            // Finally, even if this is an air-to-submarine hit, it is still valid if the firing army contains a destroyer. Thus, if even
            // one of these three conditions are met, the hit is valid.
            return firer.Category != UnitCategory.Air || sustainer != UnitType.Submarine || firingArmy.Contains(UnitType.Destroyer);
        }
    }
}
