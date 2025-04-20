using PokemonPRNG.LCG32.GCLCG;
using System;

namespace PokemonCoRNGLibrary.Generator
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
}
