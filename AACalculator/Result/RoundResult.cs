namespace AACalculator.Result
{
    /// <summary>
    /// A representation of the result of a single combat round.
    /// </summary>
    public class RoundResult
    {
        /// <summary>
        /// The attacking army (at the beginning of the round).
        /// </summary>
        public Army Attacker { get; }

        /// <summary>
        /// The defending army (at the beginning of the round).
        /// </summary>
        public Army Defender { get; }
        
        /// <summary>
        /// The result of the attacking army's surprise strike, if applicable.
        /// </summary>
        public FireResult AttackerSurpriseResult { get; }

        /// <summary>
        /// The result of defending army's surprise strike, if applicable.
        /// </summary>
        public FireResult DefenderSurpriseResult { get; }
        
        /// <summary>
        /// The result of the attacking army's general fire.
        /// </summary>
        public FireResult AttackerResult { get; }

        /// <summary>
        /// The result of the defending army's general fire.
        /// </summary>
        public FireResult DefenderResult { get; }
        
        /// <summary>
        /// Constructs a new <see cref="RoundResult"/> with the given attacking army, defending army, surprise strike results, and general
        /// fire results.
        /// </summary>
        /// <param name="attacker">The attacking army (at the beginning of the round).</param>
        /// <param name="defender">The defending army (at the beginning of the round).</param>
        /// <param name="attackerSurpriseResult">The result of the attacking army's surprise strike, or null if no surprise strike
        /// occurred.</param>
        /// <param name="defenderSurpriseResult">The result of the defending army's surprise strike, or null if no surprise strike
        /// occurred.</param>
        /// <param name="attackerResult">The result of the attacking army's general fire.</param>
        /// <param name="defenderResult">The result of the defending army's general fire.</param>
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