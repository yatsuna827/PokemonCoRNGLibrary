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

        public CoStarterResult Generate(uint seed)
        {
            var head = seed;
            seed.Advance1000();
            uint TID = seed.GetRand();
            uint SID = seed.GetRand();
            var u = UMBREON.Generate(ref seed, TID ^ SID);
            var e = ESPEON.Generate(ref seed, TID ^ SID);

            return new CoStarterResult(head, TID, SID, e, u);
        }

        // なんでもワンライナーにすればいいってもんじゃない.
        public static IEnumerable<uint> CalcBack(uint tid, uint sid)
            => LOWER[(sid - (0x43FDU * tid)) & 0xFFFF]
                .Select(_ => ((tid << 16) | _)
                .PrevSeed(1001))
                .Where(_ => _.IsAccessibleInNameScreen());

        public static IEnumerable<uint> CalcBack(uint tid, uint pid, ShinyType shinyType)
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

    }
    public readonly struct CoStarterResult
    {
        public uint HeadSeed { get; }
        public readonly uint TID;
        public readonly uint SID;
        public readonly GCIndividual Espeon, Umbreon;

        public static readonly CoStarterResult Empty = new CoStarterResult(0, 0x10000, 0x10000, GCIndividual.Empty, GCIndividual.Empty);

        internal CoStarterResult(uint head, uint tid, uint sid, GCIndividual espeon, GCIndividual umbreon)
        {
            HeadSeed = head;
            TID = tid;
            SID = sid;
            Espeon = espeon;
            Umbreon = umbreon;
        }
    }
}
