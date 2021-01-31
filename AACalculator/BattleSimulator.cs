using System;
using System.Collections.Generic;
using AACalculator.Result;

namespace AACalculator
{
    /// <summary>
    /// A class that is capable of simulating an Axis & Allies battle.
    /// </summary>
    public class BattleSimulator
    {
        /// <summary>
        /// The attacking army.
        /// </summary>
        private Army Attacker { get; }

        /// <summary>
        /// The defending army.
        /// </summary>
        private Army Defender { get; }

        /// <summary>
        /// The <see cref="IHitSelector"/> used to take causualties.
        /// </summary>
        private IHitSelector HitSelector { get; }
        
        /// <summary>
        /// Constructs a new <see cref="BattleSimulator"/> with the given attacking/defending armies and hit selector.
        /// </summary>
        /// <param name="attacker">The attacking army.</param>
        /// <param name="defender">The defending army.</param>
        /// <param name="hitSelector">The <see cref="IHitSelector"/> to use when taking causualties.</param>
        public BattleSimulator(Army attacker, Army defender, IHitSelector hitSelector)
        {
            Attacker = attacker;
            Defender = defender;
            HitSelector = hitSelector;
        }

        /// <summary>
        /// Simulates the battle between the attacking army and the defending army.
        /// </summary>
        /// <returns>A <see cref="BattleResult"/> representing the result of the battle.</returns>
        public BattleResult Simulate()
        {
            // Instantiate the list of round results.
            var rounds = new List<RoundResult>();
            
            // Simulate battle rounds until:
            //   1) A winner has been declared (attacker, defender, or tie), or
            //   2) Neither side is able to hit the other, at which point the simulation is stopped with no winner declared.
            while (Winner() == BattleWinner.None)
            {
                // Simulate a battle round.
                var result = DoRound();
                rounds.Add(result);

                // Check if neither side was able to hit the other. If so, stop the simulation.
                if (result.AttackerResult.TotalEffectiveHits == 0 &&
                    result.DefenderResult.TotalEffectiveHits == 0 &&
                    FireResult.Safe(result.AttackerSurpriseResult).TotalEffectiveHits == 0 &&
                    FireResult.Safe(result.DefenderSurpriseResult).TotalEffectiveHits == 0) break;
            }
            
            // Return an appropriate battle result.
            return new BattleResult(rounds, Winner(), Attacker.Clone(), Defender.Clone());
        }

        /// <summary>
        /// Determines the winner of the simulation.
        /// </summary>
        /// <returns><see cref="BattleWinner.Tie"/> if both sides have been destroyed, <see cref="BattleWinner.Attacker"/> if the attacking
        /// army remains while the defending does not, <see cref="BattleWinner.Defender"/> if the defending army remains while the attacking
        /// does not, or <see cref="BattleWinner.None"/> if no winner exists.</returns>
        private BattleWinner Winner()
        {
            if (Attacker.Empty && Defender.Empty)
                return BattleWinner.Tie;
            if (Defender.Empty)
                return BattleWinner.Attacker;
            if (Attacker.Empty)
                return BattleWinner.Defender;

            return BattleWinner.None;
        }

        /// <summary>
        /// Simulates a battle round.
        /// </summary>
        /// <returns>A <see cref="RoundResult"/> representing the result of the round.</returns>
        private RoundResult DoRound()
        {
            // Clone and store the armies for this round.
            var roundAttacker = Attacker.Clone();
            var roundDefender = Defender.Clone();

            // Clone the defending army so that it can make hits before causualties are taken.
            var tempDefender = Defender.Clone();
            
            // Simulate the surprise strikes, capturing the results.
            var attackerSurpriseResult = SurpriseStrike(Attacker, Defender, true);
            var defenderSurpriseResult = SurpriseStrike(tempDefender, Attacker, false);

            // Clone the defending army so that it can make hits before causualties are taken.
            tempDefender = Defender.Clone();
            
            // Simulate the general firing rounds, capturing the results.
            // Note that whether or not to fire the submarines depends upon whether or not the surprise strikes were executed.
            var attackerResult = Fire(Attacker, Defender, true, attackerSurpriseResult == null);
            var defenderResult = Fire(tempDefender, Attacker, false, defenderSurpriseResult == null);

            return new RoundResult(roundAttacker, roundDefender, attackerSurpriseResult, defenderSurpriseResult,
                attackerResult, defenderResult);
        }

        /// <summary>
        /// Simulates a submarine surprise strike.
        /// </summary>
        /// <param name="striker">The striking army.</param>
        /// <param name="sustainer">The army being struck.</param>
        /// <param name="attacker">Whether the submarine(s) are in the attacking army (as opposed to the defending army).</param>
        /// <returns></returns>
        private FireResult SurpriseStrike(Army striker, Army sustainer, bool attacker)
        {
            // If a surprise strike is not allowed between these armies, return null.
            if (!CanSurpriseStrike(striker, sustainer)) return null;

            // Simulate the strike using FireUnitGroup() capturing the hit results.
            var hits = FireUnitGroup(sustainer, UnitType.Submarine, striker, attacker);

            // Return an appropriate firing round result with one entry from UnitType.Submarine to the captured hits.
            return new FireResult(new Dictionary<UnitType, IEnumerable<HitResult>> { [UnitType.Submarine] = hits });
        }

        /// <summary>
        /// Simulates a general firing round between the given armies.
        /// </summary>
        /// <param name="firer">The firing army.</param>
        /// <param name="sustainer">The army being fired upon.</param>
        /// <param name="attacker">Whether the firing army is the attacking army (as opposed to the defending army).</param>
        /// <param name="subs">Whether to fire submarines.</param>
        /// <returns></returns>
        private FireResult Fire(Army firer, Army sustainer, bool attacker, bool subs)
        {
            // Instantiate the hit result list.
            var hits = new Dictionary<UnitType, IEnumerable<HitResult>>();
            
            // Iterate over each unit type in the firing army.
            foreach (var type in firer.Units.Keys)
                // Ensure that submarines are only fired when requested.
                if (subs || type != UnitType.Submarine)
                    // Simulate the firing round by using FireUnitGroup(), adding the result to the hit results list.
                    hits.Add(type, FireUnitGroup(sustainer, type, firer, attacker));

            return new FireResult(hits);
        }

        /// <summary>
        /// Fires the given unit type from the given firing army into the given target army.
        /// </summary>
        /// <param name="army">The army upon which to fire.</param>
        /// <param name="firer">The unit type to fire.</param>
        /// <param name="firingArmy">The firing army.</param>
        /// <param name="attacker">Whether the firing army is the attacking army (as opposed to the defending army).</param>
        /// <returns>A list of HitResults, each one representing a unit upon which the firer fired.</returns>
        private IEnumerable<HitResult> FireUnitGroup(Army army, UnitType firer, Army firingArmy, bool attacker)
        {
            // Calculate the necessary amount of causualties to impose on the sustaining army by:
            //   1) Getting the appropriate "score", depeneding on whether the firer is the attacker or not,
            //   2) Dividing by 6 since there are 6 sides to a die, and
            //   3) Multiplying the causualties-per-unit score by the number of units.
            var hits = firingArmy.Units[firer] * (firer.Score(attacker) / 6);

            // Remove the calculated causualties and return the result.
            return Hitter.Hit(army, firer, firingArmy, hits, attacker, HitSelector);
        }

        /// <summary>
        /// Determines whether the the given army can surprise strike another army.
        /// </summary>
        /// <param name="striker">The striking army.</param>
        /// <param name="sustainer">The army being struck.</param>
        /// <returns>True if the surprise strike is valid; false otherwise.</returns>
        private static bool CanSurpriseStrike(Army striker, Army sustainer)
        {
            // To be valid:
            //   1) The striking army must of course contain a submarine, and
            //   2) The sustaining army must not contain a destroyer, which counters surprise strikes.
            return striker.Contains(UnitType.Submarine) && !sustainer.Contains(UnitType.Destroyer);
        }
    }
}