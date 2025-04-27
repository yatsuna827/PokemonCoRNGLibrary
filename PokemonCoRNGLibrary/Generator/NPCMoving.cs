using System;
using PokemonPRNG.LCG32.GCLCG;

using PokemonCoRNGLibrary.AdvanceSource;

namespace PokemonCoRNGLibrary.Generator.Extension
{
    public static class NPCMovingExtension
    {
        public static float GenerateNPCWait(ref this uint seed)
        {
            var r = seed.GetRand_f() + seed.GetRand_f() - 1.0f;
            return 5.0f + 3.0f * r;
        }

        /// <summary>
        /// 移動方向(rad)を返します。0が下で逆時計回りです。
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static float GenerateNPCDirection(ref this uint seed)
        {
            var r = seed.GetRand_f();
            return 2 * (float)Math.PI * r;
        }
    }
    
    public static class PyriteTownNPCExtension
    {
        public static bool Initialize(ref this uint seed)
        {
            seed.Advance(3);
            // 4人目に方向決定するラルゴのみ、角度によっては障害物と当たって、移動に掛かる時間が変動してしまう
            var joy = seed.GetRand_f();
            seed.Advance(2);

            // かけらさん情報, 乱数が0.340 <= 0.495のときに移動時間が一定でなくなる
            return joy < 0.340f || 0.495f < joy;
        }

        public static float ComputeMinimumWait(this uint seed, int framesNpcMove = 48)
        {
            var counter = new PyriteTownCounter();
            for (int i = 0; i < framesNpcMove; i++)
                counter.CountUp(ref seed);

            var wait = seed.GenerateNPCWait();
            for (int i = 1; i < 6; i++)
                wait = Math.Min(wait, seed.GenerateNPCWait());

            return wait;
        }

        public static float[] ComputeNPCsWait(this uint seed, int framesNpcMove = 48)
        {
            var counter = new PyriteTownCounter();
            for (int i = 0; i < framesNpcMove; i++)
                counter.CountUp(ref seed);

            var wait = new float[6];
            for (int i = 0; i < 6; i++)
                wait[i] = seed.GenerateNPCWait();

            return wait;
        }

        public static bool IsSafeInPyriteTown(this uint seed, float minWait, int framesNpcMove = 48)
        {
            return seed.Initialize() && seed.ComputeMinimumWait(framesNpcMove) >= minWait;
        }

    }

}
