namespace AACalculator.Result
{
    public class RoundResult
    {
        public Army Attacker { get; }
        public Army Defender { get; }
        
        public FireResult AttackerSurpriseResult { get; }
        public FireResult DefenderSurpriseResult { get; }
        
        public FireResult AttackerResult { get; }
        public FireResult DefenderResult { get; }
        
        public RoundResult(Army attacker, Army defender, FireResult attackerSurpriseResult,
            FireResult defenderSurpriseResult, FireResult attackerResult, FireResult defenderResult)
        {
            Attacker = attacker;
            Defender = defender;
            AttackerSurpriseResult = attackerSurpriseResult;
            DefenderSurpriseResult = defenderSurpriseResult;
            AttackerResult = attackerResult;
            DefenderResult = defenderResult;
        }
    }
}