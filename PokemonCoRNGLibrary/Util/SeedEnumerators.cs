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
        public static IEnumerable<(uint seed, int interval, uint lcgIndex)> EnumerateBlinkingSeed(this uint seed, int coolTime = 4)
        {
            var blinkCounter = 0;
            var lastBlinkedFrame = 0;
            var index = 0u;
            for (var currentFrame = 1; true; currentFrame++)
            {
                blinkCounter += 2;
                if (blinkCounter < 10) continue;

                index++;
                if (seed.GetRand_f() <= BlinkConst.blinkThresholds[blinkCounter - 10])
                {
                    yield return (seed, currentFrame - lastBlinkedFrame, index);

                    blinkCounter = 0;
                    lastBlinkedFrame = currentFrame;
                    currentFrame += coolTime;
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
                if (seed.Advance() < 0x199A) seed.Advance(4); // 実際はfloatに変換して0.1fと比較している.
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
                if (seed.Advance() < 0x199A) { seed.Advance(4); index += 4; }
            }
        }

        /// <summary>
        /// ダークポケモン研究所B2Fの不定消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<uint> EnumerateSeedAtCipherLab(this uint seed) => new CipherLabEnumerator(seed).EnumerateSeed();
        /// <summary>
        /// ダークポケモン研究所B2Fの不定消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<(int index, uint seed)> EnumerateSeedAtCipherLabWithIndex(this uint seed) => new CipherLabEnumerator(seed).EnumerateSeedWithIndex();

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
