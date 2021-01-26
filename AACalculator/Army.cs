using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AACalculator
{
    public class Army
    {
        public IReadOnlyDictionary<UnitType, decimal> Units => new ReadOnlyDictionary<UnitType, decimal>(units);
        public IReadOnlyDictionary<UnitType, decimal> ExtraLives => new ReadOnlyDictionary<UnitType, decimal>(extraLives);
        
        private Dictionary<UnitType, decimal> units;
        private Dictionary<UnitType, decimal> extraLives;

        public Army(Dictionary<UnitType, decimal> units)
        {
            this.units = units;
            extraLives = units.ToDictionary(p => p.Key, p => (decimal) p.Key.ExtraLives);
        }

        public decimal Hit(UnitType type, decimal amt)
        {
            if (!units.ContainsKey(type))
                return amt;

            if (extraLives[type] >= amt)
            {
                extraLives[type] -= amt;
            }
            else
            {
                var remainder = amt - extraLives[type];
                extraLives[type] = 0;
                
                return Hit(type, remainder);
            }

            if (units[type] >= amt)
            {
                units[type] -= amt;
                return 0;
            }
            else
            {
                var remainder = amt - units[type];
                units[type] = 0;
                
                return remainder;
            }
        }
    }
}