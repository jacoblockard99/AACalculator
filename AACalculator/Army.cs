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
        /// An immutable dictionary containing the amount of each type of unit in the army.
        /// </summary>
        public ImmutableDictionary<UnitType, decimal> Units => _units.ToImmutableDictionary();

        /// <summary>
        /// An immutable dictionary containing the amount of extra lives of each type of unit in the army.
        /// </summary>
        public ImmutableDictionary<UnitType, decimal> ExtraLives => _extraLives.ToImmutableDictionary();

        /// <summary>
        /// Whether the army is empty.
        /// </summary>
        public bool Empty => Units.Count == 0 || Units.All(p => p.Value == 0);

        /// <summary>
        /// The total number of units in this army.
        /// </summary>
        public decimal UnitCount => Units.Sum(p => p.Value);
        
        /// <summary>
        /// A private, mutable property behind <see cref="Units"/>.
        /// </summary>
        private readonly Dictionary<UnitType, decimal> _units;

        /// <summary>
        /// A private, mutable property behind <see cref="ExtraLives"/>.
        /// </summary>
        private readonly Dictionary<UnitType, decimal> _extraLives;

        public Army(Dictionary<UnitType, decimal> units)
        {
            _units = units;
            _extraLives = units.ToDictionary(p => p.Key, p => p.Key.ExtraLives * p.Value);
        }

        /// <summary>
        /// Constructs a new <see cref="Army"/> with the given units and extra lives dictionaries.
        /// </summary>
        /// <param name="units">The units dictionary, containing the amounts of each unit type in the army.</param>
        /// <param name="extraLives">The extra lives dictionary, containing the extra lives of each unit type in the army.</param>
        private Army(Dictionary<UnitType, decimal> units, Dictionary<UnitType, decimal> extraLives)
        {
            _units = units;
            _extraLives = extraLives;
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="Army"/>.
        /// </summary>
        /// <returns>An <see cref="Army"/> object that is a copy of this one.</returns>
        public Army Clone()
        {
            // Create new, copied dictionaries using the Dictionary constructor.
            return new(new Dictionary<UnitType, decimal>(_units), new Dictionary<UnitType, decimal>(_extraLives));
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
            if (_extraLives[type] >= amt)
            {
                // If so, remove the appropriate amount of lives from the given type and immediately return an appropriate result.
                _extraLives[type] -= amt;
                return new HitResult(type, amt);
            }
            else
            {
                // If not, calculate the remainder and take what lives do exist (by setting it equal to zero).
                var remainder = amt - _extraLives[type];
                _extraLives[type] = 0;

                // Continue hitting with the remainder.
                amt = remainder;
            }

            // Check if the given unit type can completely absorb the hit.
            if (_units[type] >= amt)
            {
                // If it can, remove the necessary number of causualties.
                _units[type] -= amt;
                
                // Check if the number of units is now at the minimum. If so, remove it.
                if (_units[type] <= UnitMinimum)
                    _units.Remove(type);
                
                return new HitResult(type, amt);
            }
            else
            {
                // If not, store the number of units that *will* be removed and remove the units.
                var removed = _units[type];
                _units.Remove(type);
                
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
            return string.Join(", ", _units.Select(p => $"{p.Value:0.####} {ProperName(p.Key, p.Value)}"));
        }

        /// <summary>
        /// Gets the proper name (with the proper pluralization) for the given unit type and amount.
        /// </summary>
        /// <param name="type">The unit type.</param>
        /// <param name="amt">The amount.</param>
        /// <returns>The proper name.</returns>
        private static string ProperName(UnitType type, decimal amt)
        {
            return (amt == 1 ? type.Name : type.PluralName).ToLower();
        }
    }
}