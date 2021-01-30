namespace AACalculator
{
    public interface IHitSelector
    {
        bool Hit(Army army, UnitType firer, Army firingArmy, decimal amt, bool attacker);
    }
}