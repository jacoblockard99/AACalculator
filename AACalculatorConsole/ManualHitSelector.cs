using System;
using System.Collections.Generic;
using System.Linq;
using AACalculator;
using AACalculator.Result;
using Asker;
using Asker.Conversion.Converters;
using Asker.Validation;
using Asker.Validation.Validators;

namespace AACalculatorConsole
{
    public class ManualHitSelector : IHitSelector
    {
        public UnitType Select(Army army, UnitType firer, Army firingArmy, decimal amt, bool attacker)
        {
            var asker = new GeneralAsker<UnitType>(new ValidatedConverter<string, UnitType>(
                new PresenceValidator(),
                new StringToUnitTypeConverter(),
                new OptionValidator<UnitType>(army.Units.Keys.Append(null).ToArray())
            ));

            Console.WriteLine($"Firing Army: {firingArmy}");
            Console.WriteLine($"Firing Unit Type: {firer}");
            Console.WriteLine($"Sustaining Army: {army}");
            
            var type = asker.Ask("From which unit type do you wish to take casualties?");
            
            Console.WriteLine();
            
            return type;
        }
    }
}