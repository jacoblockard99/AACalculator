using System;
using System.Collections.Generic;
using System.Linq;
using AACalculator.Result;

namespace AACalculator
{
    public class HitSelectorByScore : IHitSelector
    {
        public UnitType Select(Army army, UnitType firer, Army firingArmy, decimal amt, bool attacker)
        {
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

            return hitType;
        }
    }
}