using System.Linq;

namespace AACalculator
{
    public class HitSelectorByScore : IHitSelector
    {
        public void Hit(Army army, UnitType firer, decimal amt, bool attacker)
        {
            if (army.Empty) return;
            
            var hitType = army.Units.Keys.First();

            foreach (var type in army.Units.Keys.Skip(1))
            {
                var score = type.Score(attacker);
                var hitScore = hitType.Score(attacker);

                if (score < hitScore || score == hitScore && type.Cost < hitType.Cost)
                    hitType = type;
            }

            var remainder = army.Hit(hitType, amt);

            if (remainder > 0)
                Hit(army, firer, remainder, attacker);
        }
    }
}