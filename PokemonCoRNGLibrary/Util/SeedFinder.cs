using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary
{
    public static class SeedFinder
    {
        private static readonly uint[][] LOWER;

        static SeedFinder()
        {
            var lower = Enumerable.Range(0, 0x8000).Select(_ => new List<uint>()).ToArray();
            for (uint y = 0; y < 0x10000; y++) lower[(y.NextSeed() >> 16) & 0x7FFF].Add(y);

            LOWER = lower.Select(_ => _.ToArray()).ToArray();
        }

        /// <summary>
        /// 指定した個体値の個体を生成するseedを返します. 
        /// </summary>
        public static IEnumerable<uint> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool generateEnemyTSV = true)
        {
            var offset = generateEnemyTSV ? 5u : 3u;

            var HAB = H | (A << 5) | (B << 10);
            var SCD = S | (C << 5) | (D << 10);

            uint key = (SCD - (0x43FDU * HAB)) & 0x7FFF;

            foreach (var low16 in LOWER[key])
            {
                uint seed = ((HAB << 16) | low16).PrevSeed(offset);
                yield return seed;
                yield return seed ^ 0x80000000;
            }
        }

        /// <summary>
        /// 瞬きから現在seedを検索し, 条件に一致するものを返します.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="minIndex"></param>
        /// <param name="maxIndex"></param>
        /// <param name="blinkInput"></param>
        /// <param name="allowanceLimitOfError"></param>
        /// <param name="coolTime"></param>
        /// <returns></returns>
        public static IEnumerable<uint> FindCurrentSeedByBlink(uint seed, uint minIndex, uint maxIndex, int[] blinkInput, int allowanceLimitOfError = 20, int coolTime = 4)
        {
            var res = new List<uint>();

            // 瞬き閾値
            var thresh = BlinkConst.blinkThresholds_even;

            seed.Advance(minIndex);
            var n = maxIndex - minIndex + 1;
            var blinkCount = blinkInput.Length;

            // 境界条件でバグらないように瞬き1回分余計に計算する. 
            var len = (int)n + 85 * (blinkCount + 2);

            // 「到達したときに瞬きをするような内部カウンタの値」の最小値に変換する.
            var countList = seed.EnumerateRand().Take(len + 1).Select(_ => thresh.UpperBound(_)).ToArray();

            // 「その位置で瞬きをした場合の, 次の瞬きまでの間隔」.
            var blankList = Enumerable.Repeat(86, len).ToArray();
            for (int i = len - 1; i >= 0; i--)
                for (int k = countList[i]; k <= Math.Min(i, 85); k++)
                    blankList[i - k] = Math.Min(blankList[i - k], k);

            // 『iフレーム目に1回目の瞬きが行われた』と仮定してシミュレート.
            for (int i = 0; i < n; i++)
            {
                int idx = i;
                int k;
                for (k = 0; k < blinkCount; k++)
                {
                    // 上のblankListの計算では判定を行わない(5+coolTime)フレーム分が考慮されていない. 
                    var blank = blankList[idx] + 5 + coolTime;

                    // 許容誤差を超えているなら次のフレームへ.
                    if ((blinkInput[k] + allowanceLimitOfError) < blank || blank < (blinkInput[k] - allowanceLimitOfError)) break;

                    // 間隔ぶんをindexに加算する.
                    idx += blankList[idx] + 1;
                }

                // 入力と全て一致すればresに入れる.
                if (k == blinkCount) res.Add((uint)idx);
            }

            return res.Distinct().Select(_ => seed.NextSeed(_));
        }

    }
}
