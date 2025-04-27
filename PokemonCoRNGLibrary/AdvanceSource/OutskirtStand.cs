using System;
using System.Collections;
using System.Collections.Generic;

using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary.AdvanceSource
{
    /// <summary>
    /// 町外れのスタンド 屋外の不定消費をエミュレートするクラス. 不定消費の用途は知らない.
    /// </summary>
    class OutskirtStandCounter : ISourceCounter
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
        public uint SimulateNextFrame(uint seed)
        {
            var next = value + seed.GetRand_f() * 0.8f;
            if (next >= 1.0f)
            {
                seed.Advance();
            }
            return seed;
        }
    }

    public class OutskirtStand : ISeedEnumeratorHandler, ISeedEnumeratorHandlerWithSelector<ISourceCounter>
    {
        private OutskirtStandCounter _counter;

        public uint Initialize(uint seed)
        {
            _counter = new OutskirtStandCounter(ref seed);
            return seed;
        }

        public uint SelectCurrent(uint seed) => _counter.SimulateNextFrame(seed.NextSeed(4));
        public uint SelectCurrent(uint seed, Func<uint, ISourceCounter, uint> selector) => selector(seed, _counter);

        public uint Advance(uint seed)
        {
            _counter.CountUp(ref seed);
            return seed;
        }
    }
}
