using System;
using System.Collections.Generic;

using PokemonPRNG.LCG32.GCLCG;
using PokemonCoRNGLibrary.AdvanceSource;

namespace PokemonCoRNGLibrary
{
    public interface ISeedEnumeratorHandler
    {
        uint Initialize(uint seed);
        uint SelectCurrent(uint seed);
        uint Advance(uint seed);
    }
    public interface ISeedEnumeratorHandlerWithSelector<T>
    {
        uint Initialize(uint seed);
        uint SelectCurrent(uint seed, Func<uint, T, uint> selector);
        uint Advance(uint seed);
    }
    class AppliedSeedEnumeratorHandler<T> : ISeedEnumeratorHandler
    {
        private Func<uint, T, uint> _selector;
        private ISeedEnumeratorHandlerWithSelector<T> _handler;

        public uint Initialize(uint seed) => _handler.Initialize(seed);
        public uint SelectCurrent(uint seed) => _handler.SelectCurrent(seed, _selector);
        public uint Advance(uint seed) => _handler.Advance(seed);

        public AppliedSeedEnumeratorHandler(ISeedEnumeratorHandlerWithSelector<T> handler, Func<uint, T, uint> selector)
        {
            _handler = handler;
            _selector = selector;
        }
    }
    public static class ISeedEnumeratorHandlerExtension
    {
        public static ISeedEnumeratorHandler Apply<T>(this ISeedEnumeratorHandlerWithSelector<T> handler, Func<uint, T, uint> selector)
            => new AppliedSeedEnumeratorHandler<T>(handler, selector);
    }

    public interface IActionSequenceEnumeratorHandler
    {
        uint Initialize(uint seed);
        uint SelectCurrent(uint seed);
        bool RollForAction(ref uint seed);
    }

    public static class SeedEnumeratorExtension
    {
        private static IEnumerable<uint> EnumerateSeedNotNull(this uint seed, ISeedEnumeratorHandler handler)
        {
            seed = handler.Initialize(seed);
            while (true)
            {
                yield return handler.SelectCurrent(seed);
                seed = handler.Advance(seed);
            }
        }
        public static IEnumerable<uint> EnumerateSeed(this uint seed, ISeedEnumeratorHandler handler)
            => handler is null ? seed.EnumerateSeed() : seed.EnumerateSeedNotNull(handler);

        public static IEnumerable<(uint Seed, int Frame, int Interval)> EnumerateActionSequence(this uint seed, IActionSequenceEnumeratorHandler handler)
        {
            var lastBlinkedFrame = 0;
            var currentFrame = 0;

            seed = handler.Initialize(seed);
            while (true)
            {
                currentFrame++;
                if (handler.RollForAction(ref seed))
                {
                    yield return (handler.SelectCurrent(seed), currentFrame, currentFrame - lastBlinkedFrame);
                    lastBlinkedFrame = currentFrame;
                }
            }
        }

        /// <summary>
        /// 瞬きを行うseedと前回の瞬きからの間隔と消費数のTupleを返し続けます. 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="coolTime"></param>
        /// <returns></returns>
        public static IEnumerable<(uint seed, int interval, int frame, uint lcgIndex)> EnumerateBlinkingSeed(this uint seed, int coolTime = 4)
        {
            var lastBlinkedFrame = 0;
            var obj = new BlinkObject(coolTime, 1);
            uint index = 0;
            for (var currentFrame = 1; true; currentFrame++)
            {
                if (obj.CountUp(ref seed, ref index))
                {
                    yield return (seed, currentFrame - lastBlinkedFrame, currentFrame, index);
                    lastBlinkedFrame = currentFrame;
                }
            }
        }
        public static IEnumerable<(uint seed, int interval, int frame, uint lcgIndex)> EnumerateBlinkingSeedInBattle(this uint seed, int coolTime = 4, bool enemyBlinking = false)
        {
            var lastBlinkedFrame = 0;
            var player = new BlinkObject(10);
            var enemy = new BlinkObject(10);
            var obj = new BlinkObject(coolTime);
            uint index = 0;
            for (var currentFrame = 1; true; currentFrame++)
            {
                player.CountUp(ref seed, ref index);
                if (enemyBlinking) enemy.CountUp(ref seed, ref index);
                if (obj.CountUp(ref seed, ref index))
                {
                    yield return (seed, currentFrame - lastBlinkedFrame, currentFrame, index);
                    lastBlinkedFrame = currentFrame;
                }
            }
        }

        [Obsolete] public static IEnumerable<uint> EnumerateSeedInBattle(this uint seed, int coolTime = 4, bool enemyBlinking = false)
        {
            var player = new BlinkObject(10, 10);
            var enemy = new BlinkObject(10, 10);
            var obj = new BlinkObject(coolTime, 10);
            var index = 0u;

            yield return seed;
            for (var currentFrame = 1; true; currentFrame++)
            {
                player.CountUp(ref seed, ref index);
                if (enemyBlinking) enemy.CountUp(ref seed, ref index);
                obj.CountUp(ref seed, ref index);
                yield return seed;
            }
        }

        /// <summary>
        /// 名前入力画面での消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        [Obsolete] public static IEnumerable<uint> EnumerateSeedAtNamingScreen(this uint seed)
        {
            while (true)
            {
                yield return seed;
                if (seed.GetRand() < 0x199A) seed.Advance(4); // 実際はfloatに変換して0.1fと比較している.
            }
        }

        /// <summary>
        /// スナッチリストを開いたときに発生する消費をシミュレートし, 無限にseedを返し続けます.
        /// PID再計算の発生に対応しています.
        /// SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        [Obsolete] public static IEnumerable<uint> EnumerateSnatchListAdvance(this uint seed)
        {
            while (true)
            {
                yield return seed;
                seed.Advance(5);
                // ID = 0x00000000による色回避判定が入る
                // GCのLCGパラメータでは、最大で2回の再計算が発生する
                var psv = seed.GetRand() ^ seed.GetRand();
                if (psv < 8)
                {
                    psv = seed.GetRand() ^ seed.GetRand();
                    if (psv < 8) seed.Advance(2);
                }
            }
        }

        /// <summary>
        /// 名前入力画面での不定消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<(int frameIndex, uint lcgIndex, uint seed)> EnumerateSeedAtNamingScreenWithIndex(this uint seed)
        {
            uint index = 0;
            for (int frame = 0; true; )
            {
                yield return (frame++, index++, seed);
                if (seed.GetRand() < 0x199A) { seed.Advance(4); index += 4; }
            }
        }

    }
}
