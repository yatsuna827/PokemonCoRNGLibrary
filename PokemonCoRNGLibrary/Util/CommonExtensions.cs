using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary
{
    static class GenerateExtensions
    {
        internal static uint[] GetIVs(ref this uint seed)
        {
            uint HAB = seed.GetRand();
            uint SCD = seed.GetRand();
            return new uint[6] {
                HAB & 0x1f,
                (HAB >> 5) & 0x1f,
                (HAB >> 10) & 0x1f,
                (SCD >> 5) & 0x1f,
                (SCD >> 10) & 0x1f,
                SCD & 0x1f
            };
        }
        internal static Gender GetGender(this uint pid, GenderRatio ratio)
        {
            if (ratio == GenderRatio.Genderless) return Gender.Genderless;
            return (pid & 0xFF) < (uint)ratio ? Gender.Female : Gender.Male;
        }
        internal static bool IsShiny(this uint pid, uint tsv) => (tsv ^ (pid & 0xFFFF) ^ (pid >> 16)) < 8;
        internal static ShinyType GetShinyType(this uint pid, uint tsv)
        {
            var v = tsv ^ (pid & 0xFFFF) ^ (pid >> 16);
            if (v >= 8) return ShinyType.NotShiny;
            if (v == 0) return ShinyType.Square;
            return ShinyType.Star;
        }

        internal static uint Advance1000(ref this uint seed) => seed = 0xDD867B21 * seed + 0xD252C5A8;
    }
}
