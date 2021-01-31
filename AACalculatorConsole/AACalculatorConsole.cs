using System;
using System.Collections.Generic;
using AACalculator;
using AACalculator.Result;
using CsvHelper;

namespace AACalculatorConsole
{
    public class AACalculatorConsole
    {
        private Army Attacker { get; }
        
        private Army Defender { get; }

        private bool ShowRounds { get; }
        
        public AACalculatorConsole(Army attacker, Army defender, bool showRounds)
        {
            Attacker = attacker;
            Defender = defender;
            ShowRounds = showRounds;
        }

        public void Launch()
        {
            var result = BattleCalculator.Calculate(Attacker, Defender, new HitSelectorByScore());
            
            if (ShowRounds) PrintRoundResults(result.Rounds);
            
            Console.WriteLine(WinnerMessage(result));
        }

        private static void PrintRoundResults(IList<RoundResult> rounds)
        {
            for (var i = 0; i < rounds.Count; i++)
            {
                var bar = Bar(6);
                var header = $"{bar} Round {i + 1} {bar}";
                Console.WriteLine(header);
                
                PrintRoundResult(rounds[i]);
                
                Console.WriteLine(Bar(header.Length));

                Console.WriteLine("\n");
            }
        }

        private static void PrintRoundResult(RoundResult r)
        {
            Console.WriteLine("Attacker: " + r.Attacker);
            Console.WriteLine("Defender: " + r.Defender);
            
            PrintSurpriseStrikeResult(r.AttackerSurpriseResult, "Attacker");
            PrintSurpriseStrikeResult(r.DefenderSurpriseResult, "Defender");
            
            Console.WriteLine("Attacker Hits: " + r.AttackerResult);
            Console.WriteLine("Defender Hits: " + r.DefenderResult);
        }

        private static void PrintSurpriseStrikeResult(FireResult result, string side)
        {
            if (result != null) Console.WriteLine($"{side} Surprise Hits: ${result}");
        }

        private static string WinnerMessage(BattleResult result)
        {
            return result.Winner switch
            {
                BattleWinner.None =>
                    $"No one won the battle! The attacker was left with {result.FinalAttacker}, and the" +
                    $"defender was left with {result.FinalDefender}.",
                BattleWinner.Tie => "The battle was a tie! Both teams lost all their troops.",
                _ => $"The {result.Winner.ToString().ToLower()} won, with {result.RemainingArmy} left."
            };
        }

        private static string Bar(int length)
        {
            return new('=', length);
        }
    }
}