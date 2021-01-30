using AACalculator.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AACalculator
{
    /// <summary>
    /// Contains convenience methods to calculate Axis & Allies battle odds by using a <see cref="BattleSimulator"/>.
    /// </summary>
    public class BattleCalculator
    {
        /// <summary>
        /// Simulates the battle between the attacking army and the defending army and returns the result.
        /// </summary>
        /// <param name="attacker">The attacking army.</param>
        /// <param name="defender">The defending army.</param>
        /// <param name="hitSelector">The <see cref="IHitSelector"/> to use when taking causualties.</param>
        /// <returns>A <see cref="BattleResult"/> representing the result of the battle.</returns>
        public static BattleResult Calculate(Army attacker, Army defender, IHitSelector hitSelector)
        {
            return new BattleSimulator(attacker, defender, hitSelector).Simulate();
        }
    }
}
