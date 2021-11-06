using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary
{
    public readonly struct AdvanceResultStruct
    {
        private readonly static Dictionary<char, string> rules = new Dictionary<char, string>()
        {
            {'0',"シ最"},
            {'1',"シ強"},
            {'2',"シ普"},
            {'3',"シ弱"},
            {'4',"ダ最"},
            {'5',"ダ強"},
            {'6',"ダ普"},
            {'7',"ダ弱"},
        };

        public uint Seed { get; }

        private readonly string _code;
        public string GetProcedure() => string.Join("→", _code.Select(_ => rules[_]));

        public AdvanceResultStruct(uint seed, string code)
            => (Seed, _code) = (seed, code);
    }

    public static class BattleNowAdvance
    {
        public static (int Count, uint Seed) AdvanceRoughly(uint currentSeed, uint targetStep, uint range)
        {
            var count = 0;
            var seed = currentSeed;
            while (targetStep - seed.GetIndex(currentSeed) > range)
            {
                seed = BattleNow.SingleBattle.Ultimate.AdvanceSeed(seed);
                count++;
            }

            return (count, seed);
        }

        private readonly static RentalPartyRank[] rules = new RentalPartyRank[]
        {
            BattleNow.SingleBattle.Ultimate,
            BattleNow.SingleBattle.Hard,
            BattleNow.SingleBattle.Normal,
            BattleNow.SingleBattle.Easy,
            BattleNow.DoubleBattle.Ultimate,
            BattleNow.DoubleBattle.Hard,
            BattleNow.DoubleBattle.Normal,
            BattleNow.DoubleBattle.Easy
        };

        /// <summary>
        /// targetStepは0x100000以下の値を指定してください.
        /// </summary>
        /// <param name="currentSeed"></param>
        /// <param name="targetSeed"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static IEnumerable<AdvanceResultStruct> Advance(uint currentSeed, uint targetStep, uint range)
        {
            if (targetStep > 0x100000) throw new ArgumentException("targetStepが大きすぎます. 0x100000消費以内にしてください");

            var queue = new Queue<(uint seed, string code)>();
            var hasAppeared = new bool[targetStep + 1];

            queue.Enqueue((currentSeed, ""));
            while (queue.Count > 0)
            {
                var (seed, code) = queue.Dequeue();
                for (int i = 0; i < 8; i++)
                {
                    var nextSeed = rules[i].AdvanceSeed(seed);
                    var nextStep = nextSeed.GetIndex(currentSeed);
                    if (nextStep > targetStep || hasAppeared[nextStep]) continue;

                    hasAppeared[nextStep] = true;
                    var nextCode = code + i.ToString();

                    if (targetStep - nextStep <= range)
                        yield return new AdvanceResultStruct(nextSeed, nextCode);

                    queue.Enqueue((nextSeed, nextCode));
                }
            }
        }

    }
}
