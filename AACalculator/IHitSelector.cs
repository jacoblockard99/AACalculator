using System.Collections.Generic;
using AACalculator.Result;

namespace AACalculator
{
    /// <summary>
    /// Defines a class that is capable of selecting appropriate causualties from an army.
    /// </summary>
    public interface IHitSelector
    {
        /// <summary>
        /// Takes the given number of causualties from an army, given the type of the firing unit and the army to which it belongs.
        /// </summary>
        /// <param name="army">The army from which to take causualties.</param>
        /// <param name="firer">The type of the firing unit.</param>
        /// <param name="firingArmy">The army to which the firing unit belongs.</param>
        /// <param name="amt">The amount of causualties.</param>
        /// <param name="attacker">Whether or not the firing unit is attacking (i.e. not defending).</param>
        /// <returns></returns>
        UnitType Select(Army army, UnitType firer, Army firingArmy, decimal amt, bool attacker);
    }
}