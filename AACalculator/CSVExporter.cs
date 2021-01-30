using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AACalculator.Result;
using CsvHelper;

namespace AACalculator
{
    public class CSVExporter
    {
        public static void ExportScores(BattleResult battleResult, string path)
        {
            using var writer = new StreamWriter(path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            
            csv.WriteField("Attacker Units");
            csv.WriteField("Defender Units");
            csv.WriteField("Attacker Hits");
            csv.WriteField("Defender Hits");
            csv.NextRecord();
            
            foreach (var r in battleResult.Rounds)
            {
                csv.WriteField(r.Attacker.UnitCount);
                csv.WriteField(r.Defender.UnitCount);
                csv.WriteField(r.AttackerResult.TotalEffectiveHits);
                csv.WriteField(r.DefenderResult.TotalEffectiveHits);
                csv.NextRecord();
            }
            
            csv.WriteField(battleResult.FinalAttacker.UnitCount);
            csv.WriteField(battleResult.FinalDefender.UnitCount);
            csv.WriteField("N/A");
            csv.WriteField("N/A");
            
            csv.NextRecord();
                
        }
    }
}