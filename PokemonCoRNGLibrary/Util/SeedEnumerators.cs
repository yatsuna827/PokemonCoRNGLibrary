using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32.GCLCG;
using PokemonCoRNGLibrary.IrregularAdvance;

namespace PokemonCoRNGLibrary
{
    public static class SeedEnumerator
    {
        /// <summary>
        /// 瞬きを行うseedと前回の瞬きからの間隔と消費数のTupleを返し続けます. 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="coolTime"></param>
        /// <returns></returns>
        public static IEnumerable<(uint seed, int interval, int frame, uint lcgIndex)> EnumerateBlinkingSeed(this uint seed, int coolTime = 4)
        {
            var lastBlinkedFrame = 0;
            var obj = new BlinkObject(coolTime);
            uint index = 0;
            for (var currentFrame = 1; true; currentFrame++)
            {
                if (index == 0xFFFFFFFF) yield break;
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

        /// <summary>
        /// 名前入力画面での消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<uint> EnumerateSeedAtNamingScreen(this uint seed)
        {
            while (true)
            {
                yield return seed;
                if (seed.GetRand() < 0x199A) seed.Advance(4); // 実際はfloatに変換して0.1fと比較している.
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

        /// <summary>
        /// パイラの洞窟の不定消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// consider4Framesは戦闘突入時の4消費を考慮するかどうかです. この4消費の後に不定消費が1フレームだけ入ります.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<uint> EnumerateSeedAtPyriteCave(this uint seed, bool consider4Frames = true) => new PyriteCaveEnumerator(seed, consider4Frames).EnumerateSeed();

        /// <summary>
        /// パイラの洞窟の不定消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// consider4Framesは戦闘突入時の4消費を考慮するかどうかです. この4消費の後に不定消費が1フレームだけ入ります.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<(int index, uint seed)> EnumerateSeedAtPyriteCaveWithIndex(this uint seed, bool consider4Frames = true) => new PyriteCaveEnumerator(seed, consider4Frames).EnumerateSeedWithIndex();

        /// <summary>
        /// ダークポケモン研究所B1Fの不定消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<uint> EnumerateSeedAtCipherLabB1F(this uint seed) => new VibravaEnumerator(seed).EnumerateSeed();

        /// <summary>
        /// ダークポケモン研究所B1Fの不定消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<(int index, uint seed)> EnumerateSeedAtCipherLabB1FWithIndex(this uint seed) => new VibravaEnumerator(seed).EnumerateSeedWithIndex();

        /// <summary>
        /// ダークポケモン研究所B2Fの不定消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<uint> EnumerateSeedAtCipherLabB2F(this uint seed) => new CipherLabEnumerator(seed).EnumerateSeed();

        /// <summary>
        /// ダークポケモン研究所B2Fの不定消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<(int index, uint seed)> EnumerateSeedAtCipherLabB2FWithIndex(this uint seed) => new CipherLabEnumerator(seed).EnumerateSeedWithIndex();

        /// <summary>
        /// 町外れのスタンド屋外の不定消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<uint> EnumerateSeedAtOutskirtStand(this uint seed) => new OutskirtStandEnumerator(seed).EnumerateSeed();

        /// <summary>
        /// 町外れのスタンド屋外の不定消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<(int index, uint seed)> EnumerateSeedAtOutskirtStandWithIndex(this uint seed) => new OutskirtStandEnumerator(seed).EnumerateSeedWithIndex();

    }
}
