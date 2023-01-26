using System;
using System.Collections.Generic;
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
}
