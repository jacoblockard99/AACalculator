namespace AACalculator.Result
{
    public class HitResult
    {
        public UnitType Type { get; }
        public decimal Amount { get; }
        public bool Ineffective => !Effective;
        public bool Effective => Type != null;
        
        public HitResult(UnitType type, decimal amount)
        {
            Type = type;
            Amount = amount;
        }

        public HitResult(decimal amount) : this(null, amount)
        {
        }
    }
}