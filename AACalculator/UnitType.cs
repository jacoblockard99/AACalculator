using System;
using System.Collections.Generic;
using System.Linq;
using static AACalculator.UnitCategory;

namespace AACalculator
{
    public class UnitType
    {
        public static readonly UnitType Infantry = new("Infantry", "i", Land, 1, 2, 3, "inf");
        public static readonly UnitType Tank = new("Tank", "t", Land, 3, 3, 6, "tnk");
        public static readonly UnitType Fighter = new("Fighter", "f", Air, 3, 4, 10, "ftr");
        public static readonly UnitType Bomber = new("Bomber", "b", Air, 4, 1, 12, "bmr");
        public static readonly UnitType Submarine = new("Submarine", "s", Naval, 2, 1, 6, "sub");
        public static readonly UnitType Destroyer = new("Destroyer", "d", Naval, 2, 2, 8, "dst");
        public static readonly UnitType AircraftCarrier = new("Aircraft Carrier", "c", Naval, 1, 2, 12, "Carrier", "car");
        public static readonly UnitType Battleship = new("Battleship", "bt", Naval, 4, 4, 16, 1, "btl");
        
        public static IEnumerable<UnitType> Values => values.AsReadOnly();
        private static List<UnitType> values;
        
        public string Name { get; }
        public string ShortName { get; }
        public IReadOnlyCollection<string> Aliases { get; }
        public int Attack { get; }
        public int Defense { get; }
        public int Cost { get; }
        public int ExtraLives { get; }
        public UnitCategory Category { get; }

        private UnitType(string name, string shortName, UnitCategory category, int attack, int defense, int cost, params string[] aliases) : this(name,
            shortName, category, attack, defense, cost, 0, aliases)
        {
        }

        private UnitType(string name, string shortName, UnitCategory category, int attack, int defense, int cost, int extraLives, params string[] aliases)
        {
            Name = name;
            ShortName = shortName;
            Category = category;
            Attack = attack;
            Defense = defense;
            Cost = cost;
            Aliases = aliases.ToList().AsReadOnly();
            ExtraLives = extraLives;

            values ??= new List<UnitType>();
            values.Add(this);
        }
    }
}