using System.Runtime.InteropServices;
using AACalculator;
using CommandLine;
using CommandLine.Text;
using CsvHelper.Configuration.Attributes;

namespace AACalculatorConsole
{
    public static class EntryPoint
    {
        public class Options
        {
            [Option('a', "attacker", Required = true, HelpText = "The attacking army.")]
            public string Attacker { get; set; }
            
            [Option('d', "defender", Required = true, HelpText = "The defending army.")]
            public string Defender { get; set; }
            
            [Option('r', "show-rounds", HelpText = "Whether to display each round result after the calculation.", Default = false)]
            public bool ShowRounds { get; set; }
            
            [Option('h', "hit-method", HelpText = "The method to use when removing casualties.", Default = "score")]
            public string HitMethod { get; set; }
        }
        
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(Launch);
        }

        private static void Launch(Options options)
        {
            var attackingArmy = Army.Parse(options.Attacker);
            var defendingArmy = Army.Parse(options.Defender);
            IHitSelector hitSelector = options.HitMethod == "score" ? new HitSelectorByScore() : new ManualHitSelector();
            var aa = new AACalculatorConsole(attackingArmy, defendingArmy, options.ShowRounds, hitSelector);
            aa.Launch();
        }
    }
}