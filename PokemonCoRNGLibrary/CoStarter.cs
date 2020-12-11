using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32.GCLCG;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary
{
    public class CoStarter
    {
        public static readonly GCSlot ESPEON = new GCSlot("エーフィ", 25);
        public static readonly GCSlot UMBREON = new GCSlot("ブラッキー", 25);
        public static CoStarterResult GenerateStarter(uint seed)
        {
            seed.Advance1000();
            uint TID = seed.GetRand();
            uint SID = seed.GetRand();
            var e = ESPEON.Generate(seed, out seed, TID ^ SID);
            var u = UMBREON.Generate(seed, out seed, TID ^ SID);

            return new CoStarterResult(TID, SID, e, u);
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
