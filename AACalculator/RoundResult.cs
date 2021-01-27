namespace AACalculator
{
    public class RoundResult
    {
        public Army Attacker { get; }
        public Army Defender { get; }
        
        public decimal AttackerSurpriseHits { get; }
        public decimal DefenderSurpriseHits { get; }
        
        public decimal AttackerHits { get; }
        public decimal DefenderHits { get; }

        public decimal AttackerTotalHits => AttackerHits + AttackerSurpriseHits;
        public decimal DefenderTotalHits => DefenderHits + DefenderSurpriseHits;

        public RoundResult(Army attacker, Army defender, decimal attackerHits, decimal defenderHits,
            decimal attackerSurpriseHits = -1, decimal defenderSurpriseHits = -1)
        {
            Attacker = attacker;
            Defender = defender;

            AttackerSurpriseHits = attackerSurpriseHits;
            DefenderSurpriseHits = defenderSurpriseHits;

            AttackerHits = attackerHits;
            DefenderHits = defenderHits;
        }
    }
}