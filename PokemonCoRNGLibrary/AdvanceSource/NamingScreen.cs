using System;
using System.Collections;
using System.Collections.Generic;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary.AdvanceSource
{
    public class NamingScreen : ISeedEnumeratorHandler
    {
        public uint Initialize(uint initialSeed) => initialSeed;

        public uint SelectCurrent(uint seed) => seed;

        public uint Advance(uint seed)
        {
            if (seed.GetRand() < 0x199A) seed.Advance(4);
            return seed;
        }
    }
}
