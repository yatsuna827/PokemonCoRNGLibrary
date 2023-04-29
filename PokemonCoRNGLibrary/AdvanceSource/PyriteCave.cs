using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonPRNG.LCG32.GCLCG;
using System.Reflection;

namespace PokemonCoRNGLibrary.AdvanceSource
{
    /// <summary>
    /// パイラの洞窟にある3台のマシンの煙による不定消費をエミュレートするクラス.
    /// </summary>
    public class PyriteCaveCounter
    {
        private readonly MainCounter[] mainCounters;
        private readonly Queue<CountDownValue> lazyGenerators;
        public PyriteCaveCounter()
        {
            lazyGenerators = new Queue<CountDownValue>();
            
            void OnCarriedRight()
            {
                lazyGenerators.Enqueue(CountDownValue.CreateValue(10));
            }
            void OnCarriedLeft()
            {
                lazyGenerators.Enqueue(CountDownValue.CreateValue(20));
                lazyGenerators.Enqueue(CountDownValue.CreateValue(30));
            }
            mainCounters = new MainCounter[6]
            {
                new MainCounter(OnCarriedLeft),
                new MainCounter(OnCarriedRight),
                new MainCounter(OnCarriedLeft),
                new MainCounter(OnCarriedRight),
                new MainCounter(OnCarriedLeft),
                new MainCounter(OnCarriedRight),
            };
        }

        public void CountUp(ref uint seed)
        {
            var c = lazyGenerators.Count; // for内でEnqueueするので、外側で代入しておく必要がある.
            for (int i = 0; i < c; i++)
            {
                var value = lazyGenerators.Dequeue().CountDown();
                if (value.IsZero)
                    mainCounters[0].InterruptChild(new SubCounter(ref seed));
                else
                    lazyGenerators.Enqueue(value);
            }

            foreach(var counter in mainCounters) 
                counter.CountUp(ref seed);
        }
        public uint CalcSeedBeforeEntryingBattle(uint seed)
        {
            // 戦闘突入時のエフェクトで4消費. 諸悪の根源.
            seed.Advance(4);

            // 遅延で生える処理をシミュレート.
            var lazy = lazyGenerators.Count(_ => _.CountDown().IsZero);
            var newSubCounters = new SubCounter[lazy];
            for (int i = 0; i < newSubCounters.Length; i++) newSubCounters[i] = new SubCounter(ref seed);

            mainCounters[0].SimulateCountUp(ref seed, newSubCounters);
            mainCounters[1].SimulateCountUp(ref seed);
            mainCounters[2].SimulateCountUp(ref seed);
            mainCounters[3].SimulateCountUp(ref seed);
            mainCounters[4].SimulateCountUp(ref seed);
            mainCounters[5].SimulateCountUp(ref seed);

            return seed;
        }

        abstract class MapObjectCounter
        {
            public float Value { get; protected set; }
            protected readonly float coefficient;
            protected SubCounter child;

            private const float INITIAL_VALUE = 0.9999999f;
            public MapObjectCounter(float coef, float init = INITIAL_VALUE)
            {
                this.Value = init;
                this.coefficient = coef;
            }

            /// <summary>
            /// カウンタの加算処理を呼びます.
            /// そのまま子カウンタの加算処理も呼び出します.
            /// </summary>
            /// <param name="seed"></param>
            public abstract void CountUp(ref uint seed);

            /// <summary>
            /// 子カウンタを挟みこみます.
            /// </summary>
            /// <param name="newChild"></param>
            public void InterruptChild(SubCounter newChild)
            {
                newChild.child = this.child;
                this.child = newChild;
            }

            /// <summary>
            /// 生きている子カウンタ or nullに行きつくまで子カウンタを退ける.
            /// </summary>
            public void RemoveDeadChilds()
            {
                while (child != null && !child.IsLiving) this.child = child.child;
            }
        }

        class MainCounter : MapObjectCounter
        {
            private readonly Action onCarried;

            public override void CountUp(ref uint seed)
            {
                Value += seed.GetRand_f() * coefficient; // メインカウンタは死ぬことは無い.
                if (Value >= 1.0f)
                {
                    Value -= 1.0f;
                    seed.Advance();
                    InterruptChild(new SubCounter(ref seed));
                    onCarried?.Invoke();
                }

                RemoveDeadChilds();

                child?.CountUp(ref seed);
            }

            public void SimulateCountUp(ref uint seed)
            {
                var v = Value + seed.GetRand_f() * coefficient;
                if (v >= 1.0f)
                {
                    seed.Advance();
                    // サブカウンタが生える.
                    // 初期化 + 直後に加算処理が入り, 繰り上げが発生したらさらに1消費.
                    if (seed.GetRand_f() + seed.GetRand_f() * 0.5f >= 1.0f) seed.Advance();
                }

                child?.SimulateCountUp(ref seed);
            }

            public void SimulateCountUp(ref uint seed, SubCounter[] subordinateCounters)
            {
                var v = Value + seed.GetRand_f() * coefficient;
                if (v >= 1.0f)
                {
                    seed.Advance();
                    // サブカウンタが生える.
                    // 初期化 + 直後に加算処理が入り, 繰り上げが発生したらさらに1消費.
                    if (seed.GetRand_f() + seed.GetRand_f() * 0.5f >= 1.0f) seed.Advance();
                }

                // 遅延で生えたカウンタの加算処理.
                foreach (var sub in subordinateCounters)
                    sub.SimulateCountUp(ref seed);

                child?.SimulateCountUp(ref seed);
            }

            public MainCounter(Action onCarried) : base(0.01f) => this.onCarried = onCarried;
        }

        class SubCounter : MapObjectCounter
        {
            protected int lifetime = -1;
            protected int objectLifetime = 0;
            public bool IsLiving => (lifetime > 0 || objectLifetime > 0);
            public override void CountUp(ref uint seed)
            {
                // lifetimeが0になっているなら加算は行わない.
                if (lifetime == 0) seed.Advance();
                else
                {
                    Value += seed.GetRand_f() * coefficient;
                    if (Value >= 1.0f)
                    {
                        Value -= 1.0f;
                        seed.Advance();
                        objectLifetime = 21;
                    }
                    lifetime--;
                }

                objectLifetime--;

                RemoveDeadChilds();

                child?.CountUp(ref seed);
            }

            public void SimulateCountUp(ref uint seed)
            {
                if (IsLiving)
                {
                    if (lifetime == 0) 
                        seed.Advance();
                    else if (Value + seed.GetRand_f() * coefficient >= 1.0f) 
                        seed.Advance();
                }

                child?.SimulateCountUp(ref seed);
            }

            public SubCounter(ref uint seed) : base(0.5f, seed.GetRand_f()) => lifetime = 50;
        }
    }

    /// <summary>
    /// seedの列挙をサポートするクラス.
    /// </summary>
    class PyriteCaveEnumerator : IEnumerator<uint>
    {
        public uint Current => consider4frames ? counter.CalcSeedBeforeEntryingBattle(seed) : seed;

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
            counter = new PyriteCaveCounter();
        }

        private readonly uint initialSeed;
        private uint seed;
        private PyriteCaveCounter counter;
        private readonly bool consider4frames;
        public PyriteCaveEnumerator(uint seed, bool consider4frames = false)
        {
            initialSeed = seed;
            this.consider4frames = consider4frames;
            Reset();
        }
    }

    public class PyriteCave : ISeedEnumeratorHandler
    {
        private PyriteCaveCounter _counter;

        public uint Initialize(uint seed)
        {
            _counter = new PyriteCaveCounter();
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
