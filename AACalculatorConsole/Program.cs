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
                [UnitType.Submarine] = 5
            });
            var defender = new Army(new Dictionary<UnitType, decimal>
            {
                [UnitType.Fighter] = 1,
                [UnitType.Destroyer] = 2,
                [UnitType.Submarine] = 1
            });
            var result = BattleCalculator.Calculate(attacker, defender, new HitSelectorByScore());

            for (var i = 0; i < result.Rounds.Count; i++)
            {
                var r = result.Rounds[i];

                var opening = $"===== Round {i + 1} =====";
                
                Console.WriteLine(opening);
                Console.WriteLine($"Attacker: {r.Attacker}");
                Console.WriteLine($"Defender: {r.Defender}");
                
                if (r.AttackerSurpriseResult != null)
                    Console.WriteLine($"Attacker Surprise Hits: {r.AttackerSurpriseResult}");
                
                if (r.DefenderSurpriseResult != null)
                    Console.WriteLine($"Defender Surprise Hits: {r.DefenderSurpriseResult}");
                
                Console.WriteLine($"Attacker Hits: {r.AttackerResult}");
                Console.WriteLine($"Defender Hits: {r.DefenderResult}");
                
                Console.WriteLine(new string('=', opening.Length));
                Console.WriteLine();
                Console.WriteLine();
            }
            
            if (result.Winner == BattleWinner.Tie)
                Console.WriteLine("The battle was a tie!");
            else if (result.Winner == BattleWinner.None)
                Console.WriteLine($"No one won the battle! The attacker was left with {result.FinalAttacker}, and the defender was left with {result.FinalDefender}.");
            else
                Console.WriteLine($"The {result.Winner.ToString().ToLower()} won in {result.Rounds.Count} rounds, with {result.RemainingArmy} left!");
            
            CSVExporter.ExportScores(result, "/home/jacob/scores.csv");
            Console.WriteLine("Scores exported.");
        }
    }
}