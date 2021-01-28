using System.Linq;

namespace AACalculator
{
    public class HitSelectorByScore : IHitSelector
    {
        public bool Hit(Army army, UnitType firer, Army firingArmy, decimal amt, bool attacker)
        {
            if (army.Empty) return true;
            
            UnitType hitType = null;

            foreach (var type in army.Units.Keys)
            {
                var score = type.Score(attacker);

                if (!HitValidator.ValidHit(firer, type, firingArmy))
                    continue;

                if (hitType == null || score < hitType.Score(attacker) || score == hitType.Score(attacker) && type.Cost < hitType.Cost)
                    hitType = type;
            }

            if (hitType == null) return false;

            var remainder = army.Hit(hitType, amt);

            if (remainder > 0)
                return Hit(army, firer, firingArmy, remainder, attacker);

            return true;
        }
    }
}