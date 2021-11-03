using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary
{
    public static class LCGExtensions
    {
        /// <summary>
        /// 名前入力画面の不定消費で到達可能なseedか判定します.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static bool IsAccessibleInNameScreen(this uint seed)
        {
            var prev1 = seed > 0x1999FFFF; // a[i-1]
            var prev2 = seed.Back() > 0x1999FFFF; // a[i-2]
            var prev3 = seed.Back() > 0x1999FFFF; // a[i-3]
            var prev4 = seed.Back() > 0x1999FFFF; // a[i-4]

            if (prev1 && prev2 && prev3 && prev4) return true; // 直前の4つがスキップ条件満たさないなら安全.

            if (seed.Back() < 0x199A0000 && IsAccessibleInNameScreen(seed.PrevSeed())) return true;
            if (seed.Back() < 0x199A0000 && prev1 && IsAccessibleInNameScreen(seed.PrevSeed())) return true;
            if (seed.Back() < 0x199A0000 && prev1 && prev2 && IsAccessibleInNameScreen(seed.PrevSeed())) return true;
            if (seed.Back() < 0x199A0000 && prev1 && prev2 && prev3 && IsAccessibleInNameScreen(seed.PrevSeed())) return true;

            return false;
        }

        public static uint BackInNamingScreen(this uint seed, uint n)
        {
            if (!seed.IsAccessibleInNameScreen()) throw new ArgumentException("到達不可能なseedが渡されました.");

            // 前準備.
            // 5つ前のseedから得られる乱数値 = 4つ前のseedの上位16bit.
            var prev4 = seed.PrevSeed(4);

            while (n > 0)
            {
                n--;
                if (prev4 > 0x1999FFFF)
                {
                    seed.Back();
                    prev4.Back();
                }
                else
                {
                    seed.Back(5);
                    prev4.Back(5);
                }
            }

            return seed;
        }
    }
}
