using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.GCLCG;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary
{
    public class CoStarterGenerator : IGeneratable<CoStarterResult>
    {
        private static readonly GCSlot ESPEON = new GCSlot("エーフィ", 25, Gender.Male);
        private static readonly GCSlot UMBREON = new GCSlot("ブラッキー", 25, Gender.Male);
        public static readonly CoStarterGenerator Instance = new CoStarterGenerator();
        private CoStarterGenerator() { }
        public CoStarterResult Generate(uint seed)
        {
            seed.Advance1000();
            uint TID = seed.GetRand();
            uint SID = seed.GetRand();
            var u = UMBREON.Generate(seed, out seed, TID ^ SID);
            var e = ESPEON.Generate(seed, out seed, TID ^ SID);

            return new CoStarterResult(TID, SID, e, u);
        }

        // なんでもワンライナーにすればいいってもんじゃない.
        public IEnumerable<uint> CalcBack(uint tid, uint sid)
            => LOWER[(sid - (0x43FDU * tid)) & 0xFFFF].Select(_ => ((tid << 16) | _).PrevSeed(1001)).Where(_ => IsAccessible(_));

        public IEnumerable<uint> CalcBack(uint tid, uint pid, ShinyType shinyType)
        {
            // 死ね.
            if (shinyType == ShinyType.NotShiny) yield break;

            var psv = (pid >> 16) ^ (pid & 0xFFFF);

            // 菱形になるseedを返す.
            if ((shinyType & ShinyType.Square) != 0)
                foreach (var seed in CalcBack(tid, tid ^ psv)) yield return seed;
            
            // 星型になるseedを返す.
            if((shinyType & ShinyType.Star) != 0)
                for(uint bit =1; bit < 8; bit++)
                    foreach (var seed in CalcBack(tid, tid ^ psv ^ bit)) yield return seed;
        }

        private static readonly uint[][] LOWER;
        static CoStarterGenerator()
        {
            var lower = Enumerable.Range(0, 0x10000).Select(_ => new List<uint>()).ToArray();
            for (uint y = 0; y < 0x10000; y++) lower[(y.NextSeed() >> 16)].Add(y);

            LOWER = lower.Select(_ => _.ToArray()).ToArray();
        }

        // 名前画面の不定消費を考慮して実現できるかどうか. 
        // ID決定までに最低でも数百消費は入るのでコーナーケースは無視してよい.
        // 再帰でBack()の呼び出し/条件判定に若干の無駄はあるものの、どうせそんなに回数呼ばれないので誤差だと思っていい.
        // https://twitter.com/sub_827/status/1339884393494532096
        private static bool IsAccessible(uint seed)
        {
            var prev1 = seed > 0x1999FFFF; // a[i-1]
            var prev2 = seed.Back() > 0x1999FFFF; // a[i-2]
            var prev3 = seed.Back() > 0x1999FFFF; // a[i-3]
            var prev4 = seed.Back() > 0x1999FFFF; // a[i-4]

            if (prev1 && prev2 && prev3 && prev4) return true; // 直前の4つがスキップ条件満たさないなら安全.

            if (seed.Back() < 0x199A0000 && IsAccessible(seed.PrevSeed())) return true;
            if (seed.Back() < 0x199A0000 && prev1 && IsAccessible(seed.PrevSeed())) return true;
            if (seed.Back() < 0x199A0000 && prev1 && prev2 && IsAccessible(seed.PrevSeed())) return true;
            if (seed.Back() < 0x199A0000 && prev1 && prev2 && prev3 && IsAccessible(seed.PrevSeed())) return true;

            return false;
        }
    }
    public class CoStarterResult
    {
        public readonly uint TID;
        public readonly uint SID;
        public readonly GCIndividual Espeon, Umbreon;

        public static readonly CoStarterResult Empty = new CoStarterResult(0x10000, 0x10000, GCIndividual.Empty, GCIndividual.Empty);

        internal CoStarterResult(uint tid, uint sid, GCIndividual espeon, GCIndividual umbreon)
        {
            TID = tid;
            SID = sid;
            Espeon = espeon;
            Umbreon = umbreon;
        }
    }
}
