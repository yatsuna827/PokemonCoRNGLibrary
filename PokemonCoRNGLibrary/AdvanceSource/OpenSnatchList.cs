using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary.AdvanceSource
{
    public class OpenSnatchList : ISeedEnumeratorHandler
    {
        public uint Initialize(uint initialValue)
            => initialValue;

        public uint SelectCurrent(uint seed)
            => seed;

        public uint Advance(uint seed)
        {
            seed.Advance(5);
            // ID = 0x00000000による色回避判定が入る
            // GCのLCGパラメータでは、最大で2回の再計算が発生する
            var psv = seed.GetRand() ^ seed.GetRand();
            if (psv < 8)
            {
                psv = seed.GetRand() ^ seed.GetRand();
                if (psv < 8) seed.Advance(2);
            }

            return seed;
        }
    }
}
