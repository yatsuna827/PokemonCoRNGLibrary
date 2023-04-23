using System;
using System.Collections;
using System.Collections.Generic;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary.IrregularAdvance
{
    public class NamingScreen : ISeedEnumeratorHandler
    {
        public uint SelectCurrent(uint seed) => seed;

        public void MoveNext(ref uint seed)
        {
            if (seed.GetRand() < 0x199A) seed.Advance(4);
        }

        public uint Reset(uint initialSeed) => initialSeed;
    }
}
