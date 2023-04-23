using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary.IrregularAdvance
{
    public class BlinkObject
    {
        private readonly int coolTime;
        private int remainCoolTime = 0;
        private int blinkCounter;
        public int Counter { get => blinkCounter; }

        public void Initialize(int initialCounter)
        {
            // ほんとは1で初期化なんだけど….
            // もっと厳密にいえば0,2,5,7,9,...と進む
            // でもカウンタが10以上になるまでは判定は発生しないので、1,3,5,7,9,10...と進んだのと特に違いはない
            blinkCounter = initialCounter; 
            remainCoolTime = 0;
        }
        public bool CountUp(ref uint seed, ref uint index)
        {
            if (remainCoolTime-- > 0) return false; // マイナスに振り切るぶんには問題ないので.
            if ((blinkCounter += 2) < 10) return false;

            index++;
            if (seed.GetRand() >= BlinkConst.blinkThresholds[blinkCounter - 10]) return false;

            blinkCounter = 0;
            remainCoolTime = coolTime;

            return true;
        }
        public BlinkObject(int cool = 4, int initCounter = 0)
        {
            coolTime = cool;
            Initialize(initCounter);
        }
    }

    public class BlinkObjectEnumeratorHanlder : ISeedEnumeratorHandler
    {
        private uint _index;
        private readonly int[] _initialCounterValues;
        private readonly BlinkObject[] _blinkObjects;
        public BlinkObjectEnumeratorHanlder(params BlinkObject[] blinkObjects)
        {
            _blinkObjects = blinkObjects;
            _initialCounterValues = blinkObjects.Select(_ => _.Counter).ToArray();
        }

        public uint SelectCurrent(uint seed) => seed;

        public void MoveNext(ref uint seed)
        {
            foreach (var b in _blinkObjects)
                b.CountUp(ref seed, ref _index);
        }

        public uint Reset(uint seed)
        {
            foreach (var (obj, cnt) in _blinkObjects.Zip(_initialCounterValues, (obj, cnt) => (obj, cnt)))
                obj.Initialize(cnt);

            return seed;
        }
    }
}
