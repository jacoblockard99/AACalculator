using System;
using System.Collections.Generic;

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
                rounds.Add(DoRound());
            
            return new BattleResult(rounds, Winner());
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
            
            var attackerSurpriseHits = SurpriseStrike(Attacker, Defender, Side.Attacker);
            var defenderSurpriseHits = SurpriseStrike(tempDefender, Attacker, Side.Defender);

            tempDefender = Defender.Clone();
            
            var attackerHits = Fire(Attacker, Defender, Side.Attacker, attackerSurpriseHits == -1);
            var defenderHits = Fire(tempDefender, Attacker, Side.Defender, defenderSurpriseHits == -1);

            return new RoundResult(roundAttacker, roundDefender, attackerHits, defenderHits, attackerSurpriseHits,
                defenderSurpriseHits);
        }

        private decimal SurpriseStrike(Army striker, Army sustainer, Side side)
        {
            
            if (!CanSurpriseStrike(striker, sustainer))
                return -1;

            return FireUnitGroup(sustainer, UnitType.Submarine, striker.Units[UnitType.Submarine], side);
        }

        private decimal Fire(Army firer, Army sustainer, Side side, bool subs)
        {
            decimal hits = 0;
            
            foreach (var (type, amt) in firer.Units)
                if (subs || type != UnitType.Submarine)
                    hits += FireUnitGroup(sustainer, type, amt, side);

            return hits;
        }

        private decimal FireUnitGroup(Army army, UnitType firer, decimal units, Side side)
        {
            var hits = units * Score(firer, side);
            HitSelector.Hit(army, firer, hits);

            return hits;
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