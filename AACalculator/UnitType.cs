using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Humanizer;
using static AACalculator.UnitCategory;

namespace AACalculator
{
    
    /// <summary>
    /// An immutable representation of the type of a unit.
    /// </summary>
    public class UnitType
    {
        public static readonly UnitType Infantry = new("Infantry", "Infantry", Land, 1, 2, 3, "i", "inf");
        public static readonly UnitType Tank = new("Tank", Land, 3, 3, 6, "t", "tnk");
        public static readonly UnitType Fighter = new("Fighter", Air, 3, 4, 10, "f", "ftr");
        public static readonly UnitType Bomber = new("Bomber", Air, 4, 1, 12, "b", "bmr");
        public static readonly UnitType Submarine = new("Submarine", Naval, 2, 1, 6, "s", "sub");
        public static readonly UnitType Destroyer = new("Destroyer", Naval, 2, 2, 8, "d", "dst");
        public static readonly UnitType AircraftCarrier = new("Aircraft Carrier", Naval, 1, 2, 12, "Carrier", "c", "car");
        public static readonly UnitType Battleship = new("Battleship", Naval, 4, 4, 16, 1, "bt", "btl");
        
        /// <summary>
        /// A readonly list containing all constructed <see cref="UnitType"/>s.
        /// </summary>
        public static IEnumerable<UnitType> Values => values.AsReadOnly();

        /// <summary>
        /// The private mutable member behind <see cref="Values"/>.
        /// </summary>
        private static List<UnitType> values;
        
        /// <summary>
        /// The full name of the unit type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The plural form of the full name of the unit type.
        /// </summary>
        public string PluralName { get; }

        /// <summary>
        /// The category to which this unit type belongs.
        /// </summary>
        public UnitCategory Category { get; }

        /// <summary>
        /// An immutable list of the aliases by which this unit type can be referenced.
        /// </summary>
        public ImmutableList<string> Aliases { get; }

        /// <summary>
        /// The attack power of this unit type.
        /// </summary>
        public int Attack { get; }

        /// <summary>
        /// The defense power of this unit type.
        /// </summary>
        public int Defense { get; }

        /// <summary>
        /// The cost (in Industrial Production Credits [IPC]) of a single unit of this type.
        /// </summary>
        public int Cost { get; }

        /// <summary>
        /// The number of extra lives that units of this type possess.
        /// </summary>
        public int ExtraLives { get; }

        /// <summary>
        /// Constructs a new <see cref="UnitType"/> with the given name, plural name, category, attack power, defense power, cost, no extra lives,
        /// and aliases. Also adds the new <see cref="UnitType"/> to <see cref="Values"/>.
        /// </summary>
        /// <param name="name">The full name of the unit type.</param>
        /// <param name="category">The category to which the unit type belongs.</param>
        /// <param name="attack">The "attack power" of the unit type.</param>
        /// <param name="defense">The "defense power" of the unit type.</param>
        /// <param name="cost">The cost (in IPC) of a single unit of this type.</param>
        /// <param name="aliases">The aliases by which this unit type can be referenced.</param>
        private UnitType(string name, string pluralName, UnitCategory category, int attack, int defense, int cost, params string[] aliases) : this(name, pluralName, category, attack, defense, cost, 0, aliases)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="UnitType"/> with the given name, the default plural name, the given category, attack power, defense power, cost, no extra lives,
        /// and aliases. Also adds the new <see cref="UnitType"/> to <see cref="Values"/>.
        /// </summary>
        /// <param name="name">The full name of the unit type.</param>
        /// <param name="category">The category to which the unit type belongs.</param>
        /// <param name="attack">The "attack power" of the unit type.</param>
        /// <param name="defense">The "defense power" of the unit type.</param>
        /// <param name="cost">The cost (in IPC) of a single unit of this type.</param>
        /// <param name="aliases">The aliases by which this unit type can be referenced.</param>
        private UnitType(string name, UnitCategory category, int attack, int defense, int cost, params string[] aliases) : this(name, category, attack, defense, cost, 0, aliases)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="UnitType"/> with the given name, the default plural name, the given category, attack power, defense power, cost, extra lives,
        /// and aliases. Also adds the new <see cref="UnitType"/> to <see cref="Values"/>.
        /// </summary>
        /// <param name="name">The full name of the unit type.</param>
        /// <param name="category">The category to which the unit type belongs.</param>
        /// <param name="attack">The "attack power" of the unit type.</param>
        /// <param name="defense">The "defense power" of the unit type.</param>
        /// <param name="cost">The cost (in IPC) of a single unit of this type.</param>
        /// <param name="extraLives">The extra lives of a unit of this type.</param>
        /// <param name="aliases">The aliases by which this unit type can be referenced.</param>
        private UnitType(string name, UnitCategory category, int attack, int defense, int cost, int extraLives, params string[] aliases) : this(name, name.Pluralize(), category, attack, defense, cost, extraLives, aliases)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="UnitType"/> with the given name, plural name, category, attack power, defense power, cost, extra lives,
        /// and aliases. Also adds the new <see cref="UnitType"/> to <see cref="Values"/>.
        /// </summary>
        /// <param name="name">The full name of the unit type.</param>
        /// <param name="pluralName">The plural form of the full name of the unit type.</param>
        /// <param name="category">The category to which the unit type belongs.</param>
        /// <param name="attack">The "attack power" of the unit type.</param>
        /// <param name="defense">The "defense power" of the unit type.</param>
        /// <param name="cost">The cost (in IPC) of a single unit of this type.</param>
        /// <param name="extraLives">The extra lives of a unit of this type.</param>
        /// <param name="aliases">The aliases by which this unit type can be referenced.</param>
        private UnitType(string name, string pluralName, UnitCategory category, int attack, int defense, int cost, int extraLives, params string[] aliases)
        {
            // Instantiation
            Name = name;
            PluralName = pluralName;
            Category = category;
            Attack = attack;
            Defense = defense;
            Cost = cost;
            ExtraLives = extraLives;
            Aliases = aliases.ToImmutableList();

            // Initialize the values variable, if necessary.
            values ??= new List<UnitType>();

            // Add the newly created UnitType to the values list.
            values.Add(this);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Gets the appropriate firing score (i.e. either the "attack power" or the "defense power") for the given boolean.
        /// </summary>
        /// <param name="attacker">Whether to get the "attack" score.</param>
        /// <returns>The "attack power" if attacker is true; the "defense power" otherwise.</returns>
        public decimal Score(bool attacker)
        {
            return attacker ? Attack : Defense;
        }

        /// <summary>
        /// Determines whether this unit type matches the given input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>True if this unit type matches; false otherwise</returns>
        public bool Matches(string input)
        {
            return input.Equals(Name, StringComparison.OrdinalIgnoreCase) ||
                   input.Equals(PluralName, StringComparison.OrdinalIgnoreCase) ||
                   Aliases.Contains(input, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Finds the first unit type matching the given input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The <see cref="UnitType"/> matched by the given input string, or null if no matches were found.</returns>
        public static UnitType Find(string input)
        {
            return Values.FirstOrDefault(u => u.Matches(input));
        }
    }
}