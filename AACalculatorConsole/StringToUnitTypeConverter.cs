using System;
using System.Collections.Immutable;
using System.Linq;
using AACalculator;
using Asker.Conversion;
using JLCommons;

namespace AACalculatorConsole
{
    public class StringToUnitTypeConverter : IConverter<string, UnitType>
    {
        public string ErrorMessage { get; set; } = "That is not a valid unit type!";

        public ImmutableList<string> NoneValues = new string[]
        {
            "none",
            "nothing",
            "no",
            "n",
            "null",
            "ineffective",
            "impossible",
            "miss"
        }.ToImmutableList();
        
        public Result<UnitType> Convert(string from)
        {
            if (NoneValues.Contains(from, StringComparer.OrdinalIgnoreCase))
                return Result<UnitType>.Success(null);
            
            var type = UnitType.Find(from);
            
            if (type == null)
                return Result<UnitType>.Failure(ErrorMessage);
            
            return Result<UnitType>.Success(type);
        }
    }
}