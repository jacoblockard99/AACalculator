namespace AACalculator
{
    public interface IHitSelector
    {
        void Hit(Army army, UnitType firer, decimal amt);
    }
}