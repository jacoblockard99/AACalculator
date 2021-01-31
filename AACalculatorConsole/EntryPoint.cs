using System.Runtime.InteropServices;
using AACalculator;
using CommandLine;
using CommandLine.Text;

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
        }
        
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(Launch);
        }

        private static void Launch(Options options)
        {
            var aa = new AACalculatorConsole(Army.Parse(options.Attacker), Army.Parse(options.Defender), options.ShowRounds);
            aa.Launch();
        }
    }
}