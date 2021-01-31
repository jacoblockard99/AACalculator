using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using AACalculator.Result;
using Humanizer;

namespace AACalculator
{
    /// <summary>
    /// Represents an army of units.
    /// </summary>
    public class Army
    {
        /// <summary>
        /// The number of units at which it is considered zero.
        /// </summary>
        public const decimal UnitMinimum = 0.000001M;
        
        /// <summary>
        /// A private, mutable property behind <see cref="Units"/>.
        /// </summary>
        public Dictionary<UnitType, decimal> Units { get; }

        /// <summary>
        /// A private, mutable property behind <see cref="ExtraLives"/>.
        /// </summary>
        public Dictionary<UnitType, decimal> ExtraLives { get; }

        /// <summary>
        /// Whether the army is empty.
        /// </summary>
        public bool Empty => Units.Count == 0 || Units.All(p => p.Value == 0);

        /// <summary>
        /// The total number of units in this army.
        /// </summary>
        public decimal UnitCount => Units.Sum(p => p.Value);

        public Army() : this(new Dictionary<UnitType, decimal>())
        {
        }

        public Army(Dictionary<UnitType, decimal> units)
        {
            Units = units;
            ExtraLives = units.ToDictionary(p => p.Key, p => p.Key.ExtraLives * p.Value);
        }

        /// <summary>
        /// Constructs a new <see cref="Army"/> with the given units and extra lives dictionaries.
        /// </summary>
        /// <param name="units">The units dictionary, containing the amounts of each unit type in the army.</param>
        /// <param name="extraLives">The extra lives dictionary, containing the extra lives of each unit type in the army.</param>
        private Army(Dictionary<UnitType, decimal> units, Dictionary<UnitType, decimal> extraLives)
        {
            Units = units;
            ExtraLives = extraLives;
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="Army"/>.
        /// </summary>
        /// <returns>An <see cref="Army"/> object that is a copy of this one.</returns>
        public Army Clone()
        {
            // Create new, copied dictionaries using the Dictionary constructor.
            return new(new Dictionary<UnitType, decimal>(Units), new Dictionary<UnitType, decimal>(ExtraLives));
        }

        /// <summary>
        /// Determines whether this <see cref="Army"/> contains any units of the given type.
        /// </summary>
        /// <param name="type">The type of unit to check for.</param>
        /// <returns>True if this <see cref="Army"/> contains any units of the given type; false otherwise.</returns>
        public bool Contains(UnitType type)
        {
            return Units.ContainsKey(type);
        }

        /// <summary>
        /// Takes the given number of causualties from the given type of units.
        /// </summary>
        /// <param name="type">The type of unit from which to take causualties.</param>
        /// <param name="amt">The number of causualties to remove.</param>
        /// <returns>A <see cref="HitResult"/> object representing the result of the hit.</returns>
        public HitResult Hit(UnitType type, decimal amt)
        {
            // Ensure that some units of the given type exist in this army.
            if (!Contains(type)) throw new KeyNotFoundException("No units of the given type exist in this Army!");
            
            // Check if the given unit type has enough lives to completely absorb the hit.
            if (ExtraLives[type] >= amt)
            {
                // If so, remove the appropriate amount of lives from the given type and immediately return an appropriate result.
                ExtraLives[type] -= amt;
                return new HitResult(type, amt);
            }
            else
            {
                // If not, calculate the remainder and take what lives do exist (by setting it equal to zero).
                var remainder = amt - ExtraLives[type];
                ExtraLives[type] = 0;

                // Continue hitting with the remainder.
                amt = remainder;
            }

            // Check if the given unit type can completely absorb the hit.
            if (Units[type] >= amt)
            {
                // If it can, remove the necessary number of causualties.
                Units[type] -= amt;
                
                // Check if the number of units is now at the minimum. If so, remove it.
                if (Units[type] <= UnitMinimum)
                    Units.Remove(type);
                
                return new HitResult(type, amt);
            }
            else
            {
                // If not, store the number of units that *will* be removed and remove the units.
                var removed = Units[type];
                Units.Remove(type);
                
                // Return a new hit result with the number of units that *were* removed.
                return new HitResult(type, removed);
            }
        }

        /// <summary>
        /// Gets a string representation of this army by generating a comma-seperated list of the unit types/numbers in this <see cref="Army"/>.
        /// </summary>
        /// <returns>The generated string representation.</returns>
        public override string ToString()
        {
            return string.Join(", ", Units.Select(p => UnitString(p.Key)));
        }

        /// <summary>
        /// Parses the given string into an <see cref="Army"/>.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The parsed <see cref="Army"/>.</returns>
        public static Army Parse(string input)
        {
            var units = new Dictionary<UnitType, decimal>();
            var types = input.Split(",");

            foreach (var t in types)
            {
                var trimmed = t.Trim();
                var split = trimmed.Split(' ', 2);
                var amt = decimal.Parse(split[0].Trim());
                var name = split[1].Trim();
                var type = UnitType.Find(name);
                
                units.Add(type, amt);
            }

            return new Army(units);
        }

        /// <summary>
        /// Gets a formatted string for the given unit type containing its name, amount, and remaining lives.
        /// </summary>
        /// <param name="type">The unit type for which to format.</param>
        /// <returns>The formatted unit string.</returns>
        private string UnitString(UnitType type)
        {
            var livesString = ExtraLives[type] == 1 ? "life" : "lives";
            var lives = type.ExtraLives == 0 ? "" : $" ({ExtraLives[type]:0.###} extra {livesString})";
            return $"{Units[type]:0.###} {ProperName(type)}{lives}";
        }

        /// <summary>
        /// Gets the proper name (with the proper pluralization) for the given unit type.
        /// </summary>
        /// <param name="type">The unit type.</param>
        /// <returns>The proper name.</returns>
        private string ProperName(UnitType type)
        {
            return (Units[type] == 1 ? type.Name : type.PluralName).ToLower();
        }
    }
}