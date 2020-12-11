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
        /// 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<uint> EnumerateSeed(this uint seed)
        {
            yield return seed;
            while (true) yield return seed.Advance();
        }

        /// <summary>
        /// 無限にseedと消費数のTupleを返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<(int index, uint seed)> EnumerateSeedWithIndex(this uint seed)
        {
            int i = 0;
            yield return (i++, seed);
            while (true) yield return (i++, seed.Advance());
        }

        /// <summary>
        /// 無限に乱数値を返し続けます. SkipやTakeと組み合わせてください.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static IEnumerable<uint> EnumerateRand(this uint seed)
        {
            while (true) yield return seed.GetRand();
        }

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
        /// 名前入力画面での消費をシミュレートし, 無限にseedを返し続けます. SkipやTakeと組み合わせてください.
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

        public static IEnumerable<uint> EnumerateSeedAtCipherLab(this uint seed) => EnumerateSeed(new CipherLabEnumerator(seed));
        public static IEnumerable<(int index, uint seed)> EnumerateSeedAtCipherLabWithIndex(this uint seed) => EnumerateSeedWithIndex(new CipherLabEnumerator(seed));

        public static IEnumerable<uint> EnumerateSeedAtOutskirtStand(this uint seed) => EnumerateSeed(new OutskirtStandEnumerator(seed));
        public static IEnumerable<(int index, uint seed)> EnumerateSeedAtOutskirtStandWithIndex(this uint seed) => EnumerateSeedWithIndex(new OutskirtStandEnumerator(seed));

        public static IEnumerable<(int index, uint seed, GCIndividual individual)> EnumerateGeneration(this uint seed, CoDarkPokemon darkPokemon)
        {
            int idx = 0;
            while (true)
            {
                yield return (idx++, seed, darkPokemon.Generate(seed));
                seed.Advance();
            }
        }
        public static IEnumerable<(int index, uint seed, GCIndividual individual)> EnumerateGeneration(this IEnumerable<uint> seedEnumerator, CoDarkPokemon darkPokemon)
        {
            int idx = 0;
            foreach(var seed in seedEnumerator)
            {
                yield return (idx++, seed, darkPokemon.Generate(seed));
            }
        }


        public static IEnumerable<(int index, uint seed, CoStarterResult result)> EnumerateGenerateStarter(this uint seed)
        {
            int idx = 0;
            while (true)
            {
                yield return (idx++, seed, CoStarter.GenerateStarter(seed));
                seed.Advance();
            }
        }
        public static IEnumerable<(int index, uint seed, CoStarterResult result)> EnumerateGenerateStarter(this IEnumerable<uint> seedEnumerator)
        {
            int idx = 0;
            foreach (var seed in seedEnumerator)
            {
                yield return (idx++, seed, CoStarter.GenerateStarter(seed));
            }
        }


        private static IEnumerable<uint> EnumerateSeed(IEnumerator<uint> e)
        {
            do { yield return e.Current; } while (e.MoveNext());
        }
        private static IEnumerable<(int Index, uint seed)> EnumerateSeedWithIndex(IEnumerator<uint> e)
        {
            int i = 0;
            do { yield return (i++, e.Current); } while (e.MoveNext());
        }
    }
}
