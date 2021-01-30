using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AACalculator.Result;
using CsvHelper;

namespace AACalculator
{
    /// <summary>
    /// Contains methods to export the results of a battle to CSV files.
    /// </summary>
    public class CSVExporter
    {
        /// <summary>
        /// Exports the given <see cref="BattleResult"/> to the given file path.
        /// </summary>
        /// <param name="battleResult">The result of the battle to export.</param>
        /// <param name="path">The path of the file to which to export.</param>
        public static void Export(BattleResult battleResult, string path)
        {
            // Initialize/use the writer.
            using var writer = new StreamWriter(path);
            // Initialize/use the CsvWriter.
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            
            // Write the headers.
            csv.WriteField("Attacker Units");
            csv.WriteField("Defender Units");
            csv.WriteField("Attacker Hits");
            csv.WriteField("Defender Hits");

            csv.NextRecord();
            
            // Iterate over each RoundResult in the battle result.
            foreach (var r in battleResult.Rounds)
            {
                // Write the fields:
                //   1) The number of units in the attacking army,
                //   2) The number of units in the defending army,
                //   3) The number of effective hits made by the attacker, and
                //   4) The number of effective hits made by the defender.
                csv.WriteField(r.Attacker.UnitCount);
                csv.WriteField(r.Defender.UnitCount);
                csv.WriteField(r.AttackerResult.TotalEffectiveHits);
                csv.WriteField(r.DefenderResult.TotalEffectiveHits);

                csv.NextRecord();
            }
            
            // Write the final result, leaving the firing scores as "Not Applicable."
            csv.WriteField(battleResult.FinalAttacker.UnitCount);
            csv.WriteField(battleResult.FinalDefender.UnitCount);
            csv.WriteField("N/A");
            csv.WriteField("N/A");
            
            csv.NextRecord();
        }
    }
}