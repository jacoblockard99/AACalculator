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

        public decimal ActualAttackerSurpriseHits => AttackerSurpriseHits < 0 ? 0 : AttackerSurpriseHits;
        public decimal ActualDefenderSurpriseHits => DefenderSurpriseHits < 0 ? 0 : DefenderSurpriseHits;

        public decimal AttackerTotalHits => AttackerHits + ActualAttackerSurpriseHits;
        public decimal DefenderTotalHits => DefenderHits + ActualDefenderSurpriseHits;

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