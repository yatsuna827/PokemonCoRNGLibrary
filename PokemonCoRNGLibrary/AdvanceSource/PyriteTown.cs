using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary.AdvanceSource
{
    /// <summary>
    /// パイラタウンにある3つの噴気孔の煙による不定消費をエミュレートするクラス.
    /// </summary>
    public class PyriteTownCounter
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
        public uint CalcSeedBeforeEntryingBattle(uint seed)
        {
            // 戦闘突入時のエフェクトで4消費. 諸悪の根源.
            seed.Advance(4);

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

    /// <summary>
    /// seedの列挙をサポートするクラス.
    /// </summary>
    class PyriteTownEnumerator : IEnumerator<uint>
    {
        public uint Current => consider4frames ? _counter.CalcSeedBeforeEntryingBattle(seed) : seed;

        object IEnumerator.Current => Current;

        public void Dispose() => _counter = null;

        public bool MoveNext()
        {
            _counter.CountUp(ref seed);
            return true;
        }

        public void Reset()
        {
            seed = initialSeed;
            _counter = new PyriteTownCounter();
        }

        private readonly uint initialSeed;
        private uint seed;
        private PyriteTownCounter _counter;
        private readonly bool consider4frames;
        public PyriteTownEnumerator(uint seed, bool consider4frames = false)
        {
            initialSeed = seed;
            this.consider4frames = consider4frames;
            Reset();
        }
    }

    public class PyriteTown : ISeedEnumeratorHandler
    {
        private PyriteTownCounter _counter;

        public uint Initialize(uint seed)
        {
            _counter = new PyriteTownCounter();
            return seed;
        }

        public uint SelectCurrent(uint seed) => _counter.CalcSeedBeforeEntryingBattle(seed);

        public uint Advance(uint seed)
        {
            _counter.CountUp(ref seed);
            return seed;
        }
    }
}
