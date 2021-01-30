using System.Collections.Generic;
using AACalculator.Result;

namespace AACalculator
{
    public interface IHitSelector
    {
        IEnumerable<HitResult> Hit(Army army, UnitType firer, Army firingArmy, decimal amt, bool attacker);
    }
}