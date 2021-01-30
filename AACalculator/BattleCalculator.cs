using System;
using System.Collections.Generic;
using AACalculator.Result;

namespace AACalculator
{
    public class BattleCalculator
    {
        private Army Attacker { get; }
        private Army Defender { get; }
        private IHitSelector HitSelector { get; }
        
        public BattleCalculator(Army attacker, Army defender, IHitSelector hitSelector)
        {
            Attacker = attacker;
            Defender = defender;
            HitSelector = hitSelector;
        }

        public static BattleResult Calculate(Army attacker, Army defender, IHitSelector hitSelector)
        {
            return new BattleCalculator(attacker, defender, hitSelector).Simulate();
        }

        private BattleResult Simulate()
        {
            var rounds = new List<RoundResult>();
            
            while (Winner() == BattleWinner.None)
            {
                var result = DoRound();
                rounds.Add(result);
                if (result.AttackerResult.TotalEffectiveHits == 0 && result.DefenderResult.TotalEffectiveHits == 0)
                    break;
            }
            
            return new BattleResult(rounds, Winner(), Attacker.Clone(), Defender.Clone());
        }

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

        private RoundResult DoRound()
        {
            var roundAttacker = Attacker.Clone();
            var roundDefender = Defender.Clone();

            var tempDefender = Defender.Clone();
            
            var attackerSurpriseResult = SurpriseStrike(Attacker, Defender, Side.Attacker);
            var defenderSurpriseResult = SurpriseStrike(tempDefender, Attacker, Side.Defender);

            tempDefender = Defender.Clone();
            
            var attackerResult = Fire(Attacker, Defender, Side.Attacker, attackerSurpriseResult == null);
            var defenderResult = Fire(tempDefender, Attacker, Side.Defender, defenderSurpriseResult == null);

            return new RoundResult(roundAttacker, roundDefender, attackerSurpriseResult, defenderSurpriseResult,
                attackerResult, defenderResult);
        }

        private FireResult SurpriseStrike(Army striker, Army sustainer, Side side)
        {
            if (!CanSurpriseStrike(striker, sustainer))
                return null;

            var hits = FireUnitGroup(sustainer, UnitType.Submarine, striker, striker.Units[UnitType.Submarine], side);
            return new FireResult(new Dictionary<UnitType, IEnumerable<HitResult>> { [UnitType.Submarine] = hits });
        }

        private FireResult Fire(Army firer, Army sustainer, Side side, bool subs)
        {
            var hits = new Dictionary<UnitType, IEnumerable<HitResult>>();
            
            foreach (var (type, amt) in firer.Units)
                if (subs || type != UnitType.Submarine)
                    hits.Add(type, FireUnitGroup(sustainer, type, firer, amt, side));

            return new FireResult(hits);
        }

        private IEnumerable<HitResult> FireUnitGroup(Army army, UnitType firer, Army firingArmy, decimal units, Side side)
        {
            var hits = units * (Score(firer, side) / 6);
            return HitSelector.Hit(army, firer, firingArmy, hits, side != Side.Attacker);
        }

        private static bool CanSurpriseStrike(Army striker, Army sustainer)
        {
            return striker.Units.ContainsKey(UnitType.Submarine) && !sustainer.Units.ContainsKey(UnitType.Destroyer);
        }

        private static decimal Score(UnitType type, Side side)
        {
            return side == Side.Attacker ? type.Attack : type.Defense;
        }
    }
}