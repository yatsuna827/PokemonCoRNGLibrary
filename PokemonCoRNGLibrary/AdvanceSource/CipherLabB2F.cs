using System;
using System.Linq;
using System.Collections.Generic;

using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary.AdvanceSource
{
    /// <summary>
    /// ダークポケモン研究所B1Fにある8台の泡発生マシンによる不定消費をエミュレートするクラス.
    /// </summary>
    class CipherLabB2FCounter
    {
        private readonly MapObjectCounter root;
        private readonly Queue<CountDownValue> lazyGenerators;

        public CipherLabB2FCounter(ref uint seed)
        {
            lazyGenerators = new Queue<CountDownValue>();

            void OnCarried()
            {
                lazyGenerators.Enqueue(CountDownValue.CreateValue(1));
                lazyGenerators.Enqueue(CountDownValue.CreateValue(31));
                lazyGenerators.Enqueue(CountDownValue.CreateValue(31));
                lazyGenerators.Enqueue(CountDownValue.CreateValue(31));
                lazyGenerators.Enqueue(CountDownValue.CreateValue(101));
            }
            root = new MapObjectCounter(ref seed, OnCarried);
            for (int i = 1; i < 8; i++) root.InterruptChild(new MapObjectCounter(ref seed, OnCarried));
        }

        public void CountUp(ref uint seed)
        {
            var c = lazyGenerators.Count; // for内でEnqueueするので、外側で代入しておく必要がある.
            for (int i = 0; i < c; i++)
            {
                var value = lazyGenerators.Dequeue().CountDown();
                if (value.IsZero)
                    seed.Advance();
                else
                    lazyGenerators.Enqueue(value);
            }

            root.CountUp(ref seed); // 従属するカウンタのCountUpも再帰的に(?)呼び出す.
        }

        class MapObjectCounter
        {
            public float Value { get; protected set; }
            private readonly Action onCarried;
            private MapObjectCounter child;

            public MapObjectCounter(ref uint seed, Action onCarried)
            {
                this.Value = seed.GetRand_f();
                this.onCarried = onCarried;
            }

            /// <summary>
            /// カウンタの加算処理を呼びます.
            /// そのまま子カウンタの加算処理も呼び出します.
            /// </summary>
            /// <param name="seed"></param>
            public void CountUp(ref uint seed)
            {
                Value += seed.GetRand_f() * 0.3f;
                if (Value >= 1.0f)
                {
                    Value -= 1.0f;
                    seed.Advance(2);
                    onCarried?.Invoke();
                }

                child?.CountUp(ref seed);
            }

            /// <summary>
            /// 子カウンタを挟みこみます.
            /// </summary>
            /// <param name="newChild"></param>
            public void InterruptChild(MapObjectCounter newChild)
            {
                newChild.child = this.child;
                this.child = newChild;
            }
        }
    }

    public class CipherLabB2F : ISeedEnumeratorHandler
    {
        private CipherLabB2FCounter _counter;

        public uint Initialize(uint seed)
        {
            _counter = new CipherLabB2FCounter(ref seed);
            return seed;
        }

        public uint SelectCurrent(uint seed) => seed;

        public uint Advance(uint seed)
        {
            _counter.CountUp(ref seed);
            return seed;
        }
    }
}
