using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary.IrregularAdvance
{
    /// <summary>
    /// ボルグのフロアにある4台の泡発生マシンによる不定消費をエミュレートするクラス.
    /// </summary>
    class CipherLabCounter
    {
        readonly MainCounter[] mainCounters;
        public CipherLabCounter()
        {
            mainCounters = new MainCounter[4]
            {
                new MainCounter(),
                new MainCounter(),
                new MainCounter(),
                new MainCounter()
            };
        }
        public void CountUp(ref uint seed)
        {
            var adv = (uint)mainCounters.Sum(_ => _.HeaderAdvance());
            seed.Advance(adv);

            foreach (var c in mainCounters) c.CountUp(ref seed);
        }


        class MainCounter
        {
            private float value;
            private uint nextHeader;
            private readonly SubCounter subCounter;
            private const float INITIAL_VALUE = 0.9999999f;

            public MainCounter(float init = INITIAL_VALUE)
            {
                value = init;
                subCounter = new SubCounter();
            }

            public void CountUp(ref uint seed)
            {
                value += seed.GetRand_f() * 0.003f;
                if (value >= 1.0f)
                {
                    value -= 1.0f;
                    seed.Advance();
                    subCounter.Initialize();
                }

                nextHeader = subCounter.CountUp(ref seed) ? 1u : 0u;
            }

            public uint HeaderAdvance() => nextHeader + subCounter.LazyAdvance();

        }
        class SubCounter
        {
            private float value;
            private int lifetime;
            private readonly Queue<uint> lazyAdvance1, lazyAdvance2;
            private const float INITIAL_VALUE = 0.9999999f;
            public SubCounter()
            {
                value = INITIAL_VALUE;
                lazyAdvance1 = new Queue<uint>(Enumerable.Repeat(0, 31).Select(_ => (uint)_));
                lazyAdvance2 = new Queue<uint>(Enumerable.Repeat(0, 41).Select(_ => (uint)_));
            }

            private int counter;
            public bool CountUp(ref uint seed)
            {
                if (lifetime == 0)
                {
                    lazyAdvance1.Enqueue(0);
                    lazyAdvance2.Enqueue(0);

                    return false;
                }

                value += seed.GetRand_f() * (counter++ < 100 ? 0.5f : 0f);
                if (value >= 1.0f)
                {
                    value -= 1.0f;
                    lifetime = 100;
                    seed.Advance(6);
                    lazyAdvance1.Enqueue(3);
                    lazyAdvance2.Enqueue(1);
                    return true;
                }

                lifetime--;
                lazyAdvance1.Enqueue(0);
                lazyAdvance2.Enqueue(0);

                return false;
            }

            public uint LazyAdvance() => lazyAdvance1.Dequeue() + lazyAdvance2.Dequeue();

            public void Initialize()
            {
                lifetime = 100;
                value = INITIAL_VALUE;
                counter = 0;
            }
        }
    }
    
    /// <summary>
    /// seedの列挙をサポートするクラス.
    /// </summary>
    class CipherLabEnumerator : IEnumerator<uint>
    {
        public uint Current => seed;

        object IEnumerator.Current => Current;

        public void Dispose() => counter = null;

        public bool MoveNext()
        {
            counter.CountUp(ref seed);
            return true;
        }

        public void Reset()
        {
            seed = initialSeed;
            counter = new CipherLabCounter();
        }

        private readonly uint initialSeed;
        private uint seed;
        private CipherLabCounter counter;
        public CipherLabEnumerator(uint seed)
        {
            initialSeed = seed;
            Reset();
        }
    }
}