using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AACalculator
{
    public class Army
    {
        public IReadOnlyDictionary<UnitType, decimal> Units => new ReadOnlyDictionary<UnitType, decimal>(units);
        public IReadOnlyDictionary<UnitType, decimal> ExtraLives => new ReadOnlyDictionary<UnitType, decimal>(extraLives);

        public bool Empty => Units.Count == 0 || Units.All(p => p.Value == 0);
        
        private Dictionary<UnitType, decimal> units;
        private Dictionary<UnitType, decimal> extraLives;

        public Army(Dictionary<UnitType, decimal> units)
        {
            this.units = units;
            extraLives = units.ToDictionary(p => p.Key, p => (decimal) p.Key.ExtraLives);
        }

        private Army(Dictionary<UnitType, decimal> units, Dictionary<UnitType, decimal> extraLives)
        {
            this.units = units;
            this.extraLives = extraLives;
        }

        public Army Clone()
        {
            return new Army(new Dictionary<UnitType, decimal>(units), new Dictionary<UnitType, decimal>(extraLives));
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
                units.Remove(type);
                
                return remainder;
            }
        }
    }
}