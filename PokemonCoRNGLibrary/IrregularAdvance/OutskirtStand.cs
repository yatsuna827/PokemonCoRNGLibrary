using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary.IrregularAdvance
{
    /// <summary>
    /// 町外れのスタンド 屋外の不定消費をエミュレートするクラス. 不定消費の用途は知らない.
    /// </summary>
    class OutskirtStandCounter
    {
        private float value;
        public OutskirtStandCounter(ref uint seed)
        {
            value = seed.GetRand_f();
        }
        public void CountUp(ref uint seed)
        {
            value += seed.GetRand_f() * 0.8f;
            if (value >= 1.0f)
            {
                value -= 1.0f;
                seed.Advance();
            }
        }
    }

    /// <summary>
    /// seedの列挙をサポートするクラス.
    /// </summary>
    class OutskirtStandEnumerator : IEnumerator<uint>
    {
        public uint Current => seed.NextSeed(4);

        object IEnumerator.Current => Current;

        public void Dispose() => counter = null;

        public bool MoveNext()
        {
            counter.CountUp(ref seed);
            return true;
        }

        public void Reset()
        {
            this.seed = initialSeed;
            counter = new OutskirtStandCounter(ref seed);
        }

        private readonly uint initialSeed;
        private uint seed;
        private OutskirtStandCounter counter;
        public OutskirtStandEnumerator(uint seed)
        {
            initialSeed = seed;
            Reset();
        }
    }
}
