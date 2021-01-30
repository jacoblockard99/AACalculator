using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AACalculator;
using Humanizer;

namespace AACalculatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var attacker = new Army(new Dictionary<UnitType, decimal>
            {
                [UnitType.Infantry] = 10,
                [UnitType.Tank] = 3,
                [UnitType.Fighter] = 1
            });
            var defender = new Army(new Dictionary<UnitType, decimal>
            {
                [UnitType.Infantry] = 10,
                [UnitType.Fighter] = 2
            });
            var result = BattleCalculator.Calculate(attacker, defender, new HitSelectorByScore());

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
                Console.WriteLine($"The {result.Winner.ToString().ToLower()} won in {result.Rounds.Count} rounds, with {result.RemainingArmy} left!");
            
            CSVExporter.ExportScores(result, "/home/jacob/scores.csv");
            Console.WriteLine("Scores exported.");
        }

        private static string Format(decimal d)
        {
            return $"{d:0.##}";
        }
    }
}