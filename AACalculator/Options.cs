namespace AACalculator
{
    /// <summary>
    /// An immutable representation of the various options that can be set for an Axis & Allies battle simulation.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// The number of decimal places that formatted unit number strings should have.
        /// </summary>
        public int UnitDecimalPlaces { get; }
        
        /// <summary>
        /// The number of decimal places that formatted hit number strings should have.
        /// </summary>
        public int HitDecimalPlaces { get; }
        
        /// <summary>
        /// The minimum number of units that can exist before it is counted as zero units.
        /// </summary>
        public decimal MinimumUnitAmount { get; }
    }
}