using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using PokemonPRNG.LCG32.GCLCG;
using PokemonCoRNGLibrary.Algorithm;

namespace PokemonCoRNGLibrary
{
    public static class SeedFinder
    {
        private static readonly Lower16Solver lower16Solver = new();
        private static readonly byte[] minBlinkableBlank; // randに対し, 「到達したときに瞬きをするような内部カウンタの値」の最小値.

        static SeedFinder()
        {
            minBlinkableBlank = Enumerable.Range(0, 0x445).Select(_ => (byte)BlinkConst.blinkThresholds_even.UpperBound((uint)_)).ToArray();
        }

        /// <summary>
        /// 指定した個体値の個体を生成するseedを返します. 
        /// </summary>
        public static IEnumerable<uint> FindGeneratingSeed(uint H, uint A, uint B, uint C, uint D, uint S, bool generateEnemyTSV = true)
        {
            var offset = generateEnemyTSV ? 5u : 3u;

            var HAB = H | (A << 5) | (B << 10);
            var SCD = S | (C << 5) | (D << 10);

            var key = (SCD - (0x43FDU * HAB)) & 0x7FFF;

            foreach (var low16 in lower16Solver.Solve(key))
            {
                var seed = ((HAB << 16) | low16).PrevSeed(offset);
                yield return seed;
                yield return seed ^ 0x80000000;
            }
        }

        private const int MAX_BLINK_AGE = 85;

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
            if (blinkInput.Length == 0) yield break;
            if (blinkInput.Length > 32) throw new ArgumentOutOfRangeException(nameof(blinkInput));

            seed.Advance(minIndex);

            var candidateCount = (long)maxIndex - minIndex + 1;
            var observationCount = blinkInput.Length;
            for (var k = 0; k < observationCount; k++)
            {
                var interval = unchecked(blinkInput[k] - (5 + coolTime));
                var minAge = Math.Max(0, unchecked(interval - allowanceLimitOfError));
                var maxAge = Math.Min(MAX_BLINK_AGE, unchecked(interval + allowanceLimitOfError));
                if (minAge > maxAge) yield break;
            }

            FillBlinkMasks(out var masks, blinkInput, allowanceLimitOfError, coolTime);

            var scanLength = candidateCount + (MAX_BLINK_AGE + 1L) * observationCount;
            var finalBit = 1u << (observationCount - 1);
            BlinkStates states = default;
            long nextPendingStart = 0;
            long index = 0;

            // min ~ maxの区間の尺取り
            while (index < candidateCount)
            {
                var localSeed = seed;
                var localIndex = index;
                var localStart = nextPendingStart;
                ref var localMasks = ref masks;
                ref var localStates = ref states;
                var table = minBlinkableBlank;
                var matched = false;

                while (localIndex < candidateCount)
                {
                    var rand = localSeed.GetRand();
                    var ageLimit = rand >= 0x445 ? MAX_BLINK_AGE : table[(int)rand];
                    var end = localIndex - ageLimit;

                    uint active = 0;
                    while (localStart <= end)
                    {
                        var prev = localStates[(int)(localStart & 0x7F)];
                        active |= ((prev << 1) | 1u) & localMasks[(int)(localIndex - localStart)];
                        localStart++;
                    }

                    localStates[(int)((localIndex + 1) & 0x7F)] = active;
                    localIndex++;

                    if ((active & finalBit) != 0)
                    {
                        matched = true;
                        break;
                    }
                }

                seed = localSeed;
                index = localIndex;
                nextPendingStart = localStart;
                if (matched) yield return seed;
            }

            // maxを超えたぶんの尺取り
            while (index < scanLength)
            {
                var localSeed = seed;
                var localIndex = index;
                var localStart = nextPendingStart;
                ref var localMasks = ref masks;
                ref var localStates = ref states;
                var table = minBlinkableBlank;
                var matched = false;

                while (localIndex < scanLength)
                {
                    var rand = localSeed.GetRand();
                    var ageLimit = rand >= 0x445 ? MAX_BLINK_AGE : table[(int)rand];
                    var end = localIndex - ageLimit;

                    uint active = 0;
                    while (localStart <= end)
                    {
                        var prev = localStates[(int)(localStart & 0x7F)];
                        active |= ((prev << 1) | (localStart < candidateCount ? 1u : 0u)) & localMasks[(int)(localIndex - localStart)];
                        localStart++;
                    }

                    localStates[(int)((localIndex + 1) & 0x7F)] = active;
                    localIndex++;

                    if ((active & finalBit) != 0)
                    {
                        matched = true;
                        break;
                    }
                }

                seed = localSeed;
                index = localIndex;
                nextPendingStart = localStart;
                if (matched) yield return seed;
            }
        }

        private static void FillBlinkMasks(out BlinkMasks masks, int[] blinkInput, int allowanceLimitOfError, int coolTime)
        {
            masks = default;
            for (var k = 0; k < blinkInput.Length; k++)
            {
                var interval = unchecked(blinkInput[k] - (5 + coolTime));
                var minAge = Math.Max(0, unchecked(interval - allowanceLimitOfError));
                var maxAge = Math.Min(MAX_BLINK_AGE, unchecked(interval + allowanceLimitOfError));

                for (var age = minAge; age <= maxAge; age++)
                {
                    masks[age] |= 1u << k;
                }
            }
        }

        [InlineArray(MAX_BLINK_AGE + 1)]
        private struct BlinkMasks
        {
            private uint _element0;
        }

        [InlineArray(128)]
        private struct BlinkStates
        {
            private uint _element0;
        }

        /// <summary>
        /// 戦闘終了時に瞬きから現在seedを検索し, 条件に一致するものを返します.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="minIndex"></param>
        /// <param name="maxIndex"></param>
        /// <param name="blinkInput"></param>
        /// <param name="allowanceLimitOfError"></param>
        /// <param name="coolTime"></param>
        /// <returns></returns>
        public static IEnumerable<uint> FindCurrentSeedByBlinkInBattle(uint seed, uint maxFrame, int[] blinkInput, IActionSequenceEnumeratorHandler handler, int allowanceLimitOfError = 20)
        {
            var source = seed.EnumerateActionSequence(handler).TakeWhile(_ => _.Frame <= maxFrame);
            var timeline = source.Select(_ => _.Interval).ToArray();
            var seeds = source.Select(_ => _.Seed).ToArray();

            return blinkInput.SearchSubArrayWithError(timeline, allowanceLimitOfError).Distinct().Select(_ => seeds[_ - 1]);
        }

        /// <summary>
        /// 瞬きから現在seedを検索し, 条件に一致するものを返します.
        /// 省メモリかつ高速ですが, 瞬きを数回捨てる必要があります.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="minIndex"></param>
        /// <param name="maxIndex"></param>
        /// <param name="blinkInput"></param>
        /// <param name="allowanceLimitOfError"></param>
        /// <param name="coolTime"></param>
        /// <returns></returns>
        public static IEnumerable<uint> FindCurrentSeedByBlinkFaster(uint seed, uint minIndex, uint maxIndex, int[] blinkInput, int allowanceLimitOfError = 20, int coolTime = 4)
        {
            seed.Advance(minIndex);

            var n = (ulong)maxIndex - minIndex + 1;

            var firstBlink = true;
            ulong lcgIndex = 0;
            uint currentSeed = 0;
            var blinkCache = new (int, ulong)[256]; // 瞬き間隔をキャッシュしておく配列.

            for (int i = 0; i < blinkInput.Length; i++)
            {
                var interval = AdvanceToNextBlink(ref seed, ref lcgIndex, coolTime, ref firstBlink);
                currentSeed = seed;
                blinkCache[i] = (interval, lcgIndex);
            }

            bool check(int k)
            {
                for (int i = 0; i < blinkInput.Length; i++)
                {
                    var b = blinkCache[(k + i) & 0xFF].Item1;
                    if (blinkInput[i] + allowanceLimitOfError < b || b < blinkInput[i] - allowanceLimitOfError) return false;
                }

                return true;
            }

            int head = 0, tail = blinkInput.Length;
            do
            {
                if (check(head++)) yield return currentSeed;
                var interval = AdvanceToNextBlink(ref seed, ref lcgIndex, coolTime, ref firstBlink);
                currentSeed = seed;
                blinkCache[tail++ & 0xFF] = (interval, lcgIndex);
            }
            while (blinkCache[head & 0xFF].Item2 <= n);
        }

        private static int AdvanceToNextBlink(ref uint seed, ref ulong index, int coolTime, ref bool firstBlink)
        {
            var thresholds = BlinkConst.blinkThresholds;
            var thresholdIndex = firstBlink ? 1 : 0;
            while (seed.GetRand() >= thresholds[thresholdIndex]) thresholdIndex += 2;

            var consumed = thresholdIndex / 2 + 1;
            index += (ulong)consumed;
            var interval = unchecked(consumed + 4 + (firstBlink ? 0 : coolTime));
            firstBlink = false;
            return interval;
        }

        /// <summary>
        /// 全範囲から検索します.
        /// Fasterと同様に瞬きを数回捨てる必要があります. 使う意味は薄いです.
        /// </summary>
        /// <param name="blinkInput"></param>
        /// <param name="allowanceLimitOfError"></param>
        /// <param name="coolTime"></param>
        /// <returns></returns>
        public static IEnumerable<uint> FindCurrentSeedByBlink(int[] blinkInput, int allowanceLimitOfError = 20, int coolTime = 4)
        {
            uint seed = 0;
            ulong lcgIndex = 0;
            var firstBlink = true;
            var blinkCache = new int[256]; // 瞬き間隔をキャッシュしておく配列.

            for (int i = 0; i < blinkInput.Length; i++)
            {
                blinkCache[i] = AdvanceToNextBlink(ref seed, ref lcgIndex, coolTime, ref firstBlink);
            }

            bool check(int k)
            {
                for (int i = 0; i < blinkInput.Length; i++)
                {
                    var b = blinkCache[(k + i) & 0xFF];
                    if (blinkInput[i] + allowanceLimitOfError < b || b < blinkInput[i] - allowanceLimitOfError) return false;
                }

                return true;
            }

            int head = 0, tail = blinkInput.Length;
            while (true)
            {
                var interval = AdvanceToNextBlink(ref seed, ref lcgIndex, coolTime, ref firstBlink);
                if (check(head++)) yield return seed;
                blinkCache[tail++ & 0xFF] = interval;
            }
        }
    }

    sealed class Lower16Solver
    {
        private const uint A = 0x343FD;
        private const uint B = 0x269EC3;

        public Lower16Solver()
        {
            // GCLCGの定数では再帰は高々4回で終わることがわかっている
            var steps = new Step[3];
            var (a, m) = (A, 0x80000000);
            for (var i = 0; i < steps.Length; i++)
            {
                steps[i] = new Step(a, m);
                (a, m) = ((a - m % a) % a, a);
            }
            _steps = steps;
        }

        public IEnumerable<uint> Solve(uint key)
        {
            // ((A*x+B) >> 16) & 0x7FFF == key を満たす2^16未満のxをすべて列挙する。

            // このbを使うと ((A*x+B) >> 16) & 0x7FFF == key は
            // (A*x+b) mod 2^31 < 2^16 と変形できる 
            var b = (B + (0x80000000 - (key << 16))) & 0x7FFFFFFF;

            const uint STEP_LEFT = 30103;  // (-A * t) mod 2^31 < 2^16 となる最小のt
            const uint STEP_RIGHT = 20069; // ( A * t) mod 2^31 < 2^16 となる最小のt

            const uint DELTA_LEFT = 17605;  // (-A * Step) mod 2^31; つまり、(-Aの倍数 % 2^31)で2^16未満のもの
            const uint DELTA_RIGHT = 59601; // ( A * Step) mod 2^31; つまり、( Aの倍数 % 2^31)で2^16未満のもの

            // (A*x+b) mod 2^31 < 2^16 を満たす最小のxを計算する
            // rは (A*x + b) % 2^31 を計算した値
            var (x, r) = F(b);
            while (x < 0x10000)
            {
                yield return x;

                // xとrの関係を保ちながら、別のxを計算で求める

                // 0..<5935
                if (r < 0x10000 - DELTA_RIGHT)
                {
                    x += STEP_RIGHT;
                    r += DELTA_RIGHT;
                }
                // 5935..<17605
                else if (r < DELTA_LEFT)
                {
                    x += STEP_RIGHT + STEP_LEFT;
                    r += DELTA_RIGHT - DELTA_LEFT;
                }
                // 17605..<65536
                else
                {
                    x += STEP_LEFT;
                    r -= DELTA_LEFT;
                }
            }
        }

        private readonly Step[] _steps;
        // (a_i*x+b) mod m_i < 2^16を満たす最小の非負整数xと、そのときの(a_i*x+b mod m_i)の組を返す
        // a_iとm_iは_step[i]の値を参照し、a_0=A, m_0=2^31
        private (uint Value, uint Residue) F(uint b, int i = 0)
        {
            if (b < 0x10000) return (0, b);

            // GCLCGの定数では再帰は高々4回で終わることがわかっている
            Debug.Assert(i < 3);

            ref readonly var step = ref _steps[i];
            var d = step.M - b;
            var rem = step.Rem(d);

            var b2 = rem != 0 ? step.A - rem : 0;
            var (x, r) = F(b2, i + 1);

            return (step.ReconstructX(x, d, r), r);
        }

        private readonly struct Step
        {
            public readonly uint A;
            public readonly uint M;

            public Step(uint a, uint m)
            {
                A = a;
                M = m;

                {
                    // Aを2^kと奇数の積に分解する
                    var k = 0;
                    var odd = (ulong)a;
                    while ((odd & 1) == 0)
                    {
                        odd >>= 1;
                        k++;
                    }

                    // oddの逆数を求める
                    // see: https://rsk0315.hatenablog.com/entry/2022/11/27/060616
                    var inverse = odd;
                    for (var i = 0; i < 5; i++) inverse *= 2 - odd * inverse;

                    Debug.Assert((inverse * a) == 1);

                    _exactDivA = (inverse, k);
                }

                {
                    // 2^k >= A*Mとなるkを求める
                    var k = 0;
                    while ((1UL << k) < (ulong)a * m) k++;

                    // ceil(2^k / A) = (2^k + A - 1) / A
                    var l = ((1UL << k) + a - 1) / a;

                    // NOTE: このlについて、
                    // d*l / 2^k
                    // = d * ceil(2^k / A) / 2^k
                    // ≈ d / A
                    // であり、2^k >= A*Mより、M未満のdに対しては切り上げによる誤差が1/A未満になるため、
                    // d/Aと一致する。

                    _intDivA = (l, k);
                }
            }

            // d % A (= d - d/A * A)
            public uint Rem(uint d) => d - (uint)((d * _intDivA.m) >> _intDivA.shift) * A;
            private readonly (ulong m, int shift) _intDivA;

            // x_{i} = (x_{i+1} * M + d + r) / A
            public uint ReconstructX(uint x, uint d, uint r) => (uint)((((ulong)x * M + d + r) >> _exactDivA.shift) * _exactDivA.m);
            private readonly (ulong m, int shift) _exactDivA;
        }
    }
}
