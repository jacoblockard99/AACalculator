using System;
using System.Collections.Generic;
using AACalculator;
using AACalculator.Result;

namespace AACalculatorConsole
{
    public class ManualHitSelector : IHitSelector
    {
        public UnitType Select(Army army, UnitType firer, Army firingArmy, decimal amt, bool attacker)
        {
            var side = attacker ? "Attacker" : "Defender";
            Console.WriteLine($"{side}: [{army}]");
            Console.Write("From which unit type do you wish to take casualties? ");
            var input = Console.ReadLine();
            Console.WriteLine();
            
            return UnitType.Find(input);
        }
    }
}