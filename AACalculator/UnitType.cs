using System;
using System.Collections.Generic;
using System.Linq;

namespace AACalculator
{
    public class UnitType
    {
        public static readonly UnitType Infantry = new("Infantry", "i", 1, 2, 3, "inf");
        public static readonly UnitType Tank = new("Tank", "t", 3, 3, 6, "tnk");
        public static readonly UnitType Fighter = new("Fighter", "f", 3, 4, 10, "ftr");
        public static readonly UnitType Bomber = new("Bomber", "b", 4, 1, 12, "bmr");
        public static readonly UnitType Submarine = new("Submarine", "s", 2, 1, 6, "sub");
        public static readonly UnitType Destroyer = new("Destroyer", "d", 2, 2, 8, "dst");
        public static readonly UnitType AircraftCarrier = new("Aircraft Carrier", "c", 1, 2, 12, "Carrier", "car");
        public static readonly UnitType Battleship = new("Battleship", "bt", 4, 4, 16, 1, "btl");
        
        public static IEnumerable<UnitType> Values => values.AsReadOnly();
        private static List<UnitType> values;
        
        public string Name { get; }
        public string ShortName { get; }
        public IReadOnlyCollection<string> Aliases { get; }
        public int Attack { get; }
        public int Defense { get; }
        public int Cost { get; }
        public int ExtraLives { get; }

        private UnitType(string name, string shortName, int attack, int defense, int cost, params string[] aliases) : this(name,
            shortName, attack, defense, cost, 0, aliases)
        {
        }

        private UnitType(string name, string shortName, int attack, int defense, int cost, int extraLives, params string[] aliases)
        {
            Name = name;
            ShortName = shortName;
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