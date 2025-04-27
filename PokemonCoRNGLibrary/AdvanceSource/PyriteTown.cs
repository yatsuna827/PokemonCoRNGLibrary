using System;

using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary.AdvanceSource
{
    /// <summary>
    /// パイラタウンにある3つの噴気孔の煙による不定消費をエミュレートするクラス.
    /// </summary>
    public class PyriteTownCounter : ISourceCounter
    {
        private readonly MainCounter[] mainCounters;
        public PyriteTownCounter()
        {
            mainCounters = new MainCounter[3]
            {
                new MainCounter(),
                new MainCounter(),
                new MainCounter(),
            };
        }

        public void CountUp(ref uint seed)
        {
            foreach (var counter in mainCounters)
                counter.CountUp(ref seed);
        }
        public uint SimulateNextFrame(uint seed)
        {
            foreach (var counter in mainCounters)
                counter.SimulateCountUp(ref seed);

            return seed;
        }
        
        class MainCounter
        {
            public float Value { get; protected set; }

            private const float INITIAL_VALUE = 0.9999999f;
            public MainCounter(float init = INITIAL_VALUE)
            {
                this.Value = init;
            }

            /// <summary>
            /// カウンタの加算処理を呼びます.
            /// そのまま子カウンタの加算処理も呼び出します.
            /// </summary>
            /// <param name="seed"></param>
            public void CountUp(ref uint seed)
            {
                Value += seed.GetRand_f() * 0.5f;
                if (Value >= 1.0f)
                {
                    Value -= 1.0f;
                    seed.Advance(8);
                }
            }

            public void SimulateCountUp(ref uint seed)
            {
                var v = Value + seed.GetRand_f() * 0.5f;
                if (v >= 1.0f)
                {
                    seed.Advance(8);
                }
            }

        }

    }

    public class PyriteTown : ISeedEnumeratorHandler, ISeedEnumeratorHandlerWithSelector<ISourceCounter>
    {
        private PyriteTownCounter _counter;

        public uint Initialize(uint seed)
        {
            _counter = new PyriteTownCounter();
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

    public class PyriteTownWithNPC : ISeedEnumeratorHandler, ISeedEnumeratorHandlerWithSelector<ISourceCounter>
    {
        private readonly int _framesNpcMove;
        private int _frames;
        private PyriteTownCounter _counter;

        public uint Initialize(uint seed)
        {
            _frames = 0;
            _counter = new PyriteTownCounter();
            return seed;
        }

        public uint SelectCurrent(uint seed) =>  seed;
        public uint SelectCurrent(uint seed, Func<uint, ISourceCounter, uint> selector) => selector(seed, _counter);

        public uint Advance(uint seed)
        {
            _counter.CountUp(ref seed);

            if (++_frames == _framesNpcMove)
                seed.Advance(12);

            return seed;
        }

        /// <summary>
        /// </summary>
        /// <param name="framesNpcMove">パイラビル/パイラコロシアムから出た場合は49F、それ以外の場合は48Fとされている</param>
        public PyriteTownWithNPC(int framesNpcMove)
        {
            _framesNpcMove = framesNpcMove;
        }
    }

}
