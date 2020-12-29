using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary.IrregularAdvance
{
    class BlinkObject
    {
        private readonly int coolTime;
        private int remainCoolTime = 0;
        private int blinkCounter;

        public void Initialize(int initialCounter)
        {
            blinkCounter = initialCounter; // ほんとは1で初期化なんだけど….
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
