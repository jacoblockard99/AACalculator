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
            if (army.Empty) return new List<HitResult>();
            
            UnitType hitType = null;

            foreach (var type in army.Units.Keys)
            {
                var score = type.Score(attacker);

                if (!HitValidator.ValidHit(firer, type, firingArmy))
                    continue;

                if (hitType == null || score < hitType.Score(attacker) || score == hitType.Score(attacker) && type.Cost < hitType.Cost)
                    hitType = type;
            }

            if (hitType == null) return new List<HitResult> { new(amt) };

            var result = army.Hit(hitType, amt);

            if (result.Amount == amt)
                return new List<HitResult> { result };
            else
                return Hit(army, firer, firingArmy, amt - result.Amount, attacker).Prepend(result);
        }
    }
}