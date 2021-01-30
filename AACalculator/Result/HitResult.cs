namespace AACalculator.Result
{
    /// <summary>
    /// An immutable representation of the result of a hit against a unit.
    /// </summary>
    public class HitResult
    {
        /// <summary>
        /// The unit type against which the hit was made.
        /// </summary>
        public UnitType Type { get; }

        /// <summary>
        /// The amount of hits made against the unit.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Whether the hit was ineffective.
        /// </summary>
        public bool Ineffective => !Effective;

        /// <summary>
        /// Whether the hit was effective.
        /// </summary>
        public bool Effective => Type != null;
        
        /// <summary>
        /// Constructs a new <see cref="HitResult"/> for an effective hit against the given unit type with the given amount of hits.
        /// </summary>
        /// <param name="type">The unit type against which the hit was made.</param>
        /// <param name="amount">The amount of hits made against the unit.</param>
        public HitResult(UnitType type, decimal amount)
        {
            Type = type;
            Amount = amount;
        }

        /// <summary>
        /// Constructs a new <see cref="HitResult"/> for an ineffective hit against the given unit type with the given amount of hits.
        /// </summary>
        /// <param name="amount">The amount of (ineffective) hits made.</param>
        public static HitResult NewIneffective(decimal amount)
        {
            return new HitResult(null, amount);
        }
    }
}