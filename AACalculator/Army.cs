using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Humanizer;

namespace AACalculator
{
    public class Army
    {
        public const decimal UnitMinimum = 0.000001M;
        
        public IReadOnlyDictionary<UnitType, decimal> Units => new ReadOnlyDictionary<UnitType, decimal>(units);
        public IReadOnlyDictionary<UnitType, decimal> ExtraLives => new ReadOnlyDictionary<UnitType, decimal>(extraLives);

        public bool Empty => Units.Count == 0 || Units.All(p => p.Value == 0);
        public decimal UnitCount => Units.Sum(p => p.Value);
        
        private Dictionary<UnitType, decimal> units;
        private Dictionary<UnitType, decimal> extraLives;

        public Army(Dictionary<UnitType, decimal> units)
        {
            this.units = units;
            extraLives = units.ToDictionary(p => p.Key, p => p.Key.ExtraLives * p.Value);
        }

        private Army(Dictionary<UnitType, decimal> units, Dictionary<UnitType, decimal> extraLives)
        {
            this.units = units;
            this.extraLives = extraLives;
        }

        public Army Clone()
        {
            return new(new Dictionary<UnitType, decimal>(units), new Dictionary<UnitType, decimal>(extraLives));
        }

        public bool Contains(UnitType type)
        {
            return Units.ContainsKey(type);
        }

        public decimal Hit(UnitType type, decimal amt)
        {
            if (!units.ContainsKey(type))
                return amt;
            
            if (extraLives[type] >= amt)
            {
                extraLives[type] -= amt;
                return 0;
            }
            else
            {
                var remainder = amt - extraLives[type];
                extraLives[type] = 0;
                amt = remainder;
            }

            if (units[type] >= amt)
            {
                units[type] -= amt;
                
                if (units[type] <= UnitMinimum)
                    units.Remove(type);
                
                return 0;
            }
            else
            {
                var remainder = amt - units[type];
                units.Remove(type);
                
                return remainder;
            }
        }

        public override string ToString()
        {
            return string.Join(", ", units.Select(p => $"{p.Value:0.####} {ProperName(p.Key, p.Value)}"));
        }

        private static string ProperName(UnitType type, decimal amt)
        {
            return (amt == 1 ? type.Name : type.PluralName).ToLower();
        }
    }
}