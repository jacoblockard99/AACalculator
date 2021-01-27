using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AACalculator;
using Humanizer;

namespace AACalculatorConsole
{
    class BasicHitSelector : IHitSelector
    {
        public void Hit(Army army, UnitType firer, decimal amt)
        {
            if (army.Empty) return;
            
            var remainder = army.Hit(army.Units.Keys.First(), amt);
            
            if (remainder > 0)
                Hit(army, firer, remainder);
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var attacker = new Army(new Dictionary<UnitType, decimal>
            {
                [UnitType.Infantry] = 1
            });
            var defender = new Army(new Dictionary<UnitType, decimal>
            {
                [UnitType.Infantry] = 1
            });
            var result = BattleCalculator.Calculate(attacker, defender, new BasicHitSelector());

            for (var i = 0; i < result.Rounds.Count; i++)
            {
                var r = result.Rounds[i];

                var opening = $"===== Round {i + 1} =====";
                
                Console.WriteLine(opening);
                Console.WriteLine($"Attacker: {r.Attacker}");
                Console.WriteLine($"Defender: {r.Defender}");
                
                if (r.AttackerSurpriseHits >= 0)
                    Console.WriteLine($"Attacker Surprise Hits: {Format(r.AttackerSurpriseHits)}");
                
                if (r.DefenderSurpriseHits >= 0)
                    Console.WriteLine($"Defender Surprise Hits: {Format(r.DefenderSurpriseHits)}");
                
                Console.WriteLine($"Attacker Hits: {Format(r.AttackerHits)}");
                Console.WriteLine($"Defender Hits: {Format(r.DefenderHits)}");
                
                Console.WriteLine(new string('=', opening.Length));
                Console.WriteLine();
                Console.WriteLine();
            }
            
            if (result.Winner == BattleWinner.Tie)
                Console.WriteLine("The battle was a tie!");
            else
                Console.WriteLine("The " + result.Winner.ToString().ToLower() + " won, with " + result.RemainingArmy + " left!");
        }

        private static string Format(decimal d)
        {
            return $"{d:0.##}";
        }
    }
}