using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AACalculator
{
    public class HitValidator
    {
        public static bool ValidHit(UnitType firer, UnitType sustainer, Army firingArmy)
        {
            return CheckSub(firer, sustainer) && CheckAir(firer, sustainer, firingArmy);
        }

        private static bool CheckSub(UnitType firer, UnitType sustainer)
        {
            return firer != UnitType.Submarine || sustainer.Category != UnitCategory.Air;
        }

        private static bool CheckAir(UnitType firer, UnitType sustainer, Army firingArmy)
        {
            return firer.Category != UnitCategory.Air || sustainer != UnitType.Submarine || firingArmy.Contains(UnitType.Destroyer);
        }
    }
}
