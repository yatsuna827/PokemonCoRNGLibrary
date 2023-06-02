using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.GCLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;
using PokemonStandardLibrary.Gen3;
using System;

namespace PokemonCoRNGLibrary
{
    public class GCSlot : IGeneratable<GCIndividual, uint>, IGeneratableEffectful<GCIndividual, uint>, ILcgUser<uint>
    {
        public uint Lv { get; }
        public Pokemon.Species Species { get; }

        public GCIndividual Generate(uint seed, uint tsv = 0x10000)
        {
            var rep = seed;
            seed.Advance(2); // dummyPID
            var ivs = GenerateIVs(ref seed);
            var abilityIndex = seed.GetRand(2);
            var (pid, skipped) = GeneratePID(ref seed, tsv);

            return Species.GetIndividual(Lv, ivs, pid, abilityIndex).SetRepSeed(rep).SetShinySkipped(skipped);
        }

        public GCIndividual Generate(ref uint seed, uint tsv = 0x10000)
        {
            var rep = seed;
            seed.Advance(2); // dummyPID
            var ivs = GenerateIVs(ref seed);
            var abilityIndex = seed.GetRand(2);
            var (pid, skipped) = GeneratePID(ref seed, tsv);

            return Species.GetIndividual(Lv, ivs, pid, abilityIndex).SetRepSeed(rep).SetShinySkipped(skipped);
        }

        public void Use(ref uint seed, uint tsv = 0x10000)
        {
            seed.Advance(2); // dummyPID
            GenerateIVs(ref seed);
            seed.Advance(); // ability
            GeneratePID(ref seed, tsv);
        }

        private (uint PID, bool Skipped) GeneratePID(ref uint seed, uint tsv)
        {
            var skip = false;
            while (true)
            {
                var pid = (seed.GetRand() << 16) | seed.GetRand();

                if (!IsValidPID(pid)) 
                    continue;

                if (pid.IsShiny(tsv))
                {
                    skip = true;
                    continue;
                }

                return (pid, skip);
            }
        }

        protected virtual bool IsValidPID(uint pid) => true;

        protected virtual uint[] GenerateIVs(ref uint seed)
            => seed.GetIVs();

        public GCSlot(Pokemon.Species species, uint lv)
        {
            Lv = lv;
            Species = species;
        }

        public GCSlot(string name, uint lv)
        {
            Lv = lv;
            Species = Pokemon.GetPokemon(name);
        }
    }

    public class FixedSlot : GCSlot
    {
        public Gender FixedGender { get; }
        public Nature FixedNature { get; }

        protected override bool IsValidPID(uint pid)
        {
            if (FixedGender != Gender.Genderless && pid.GetGender(Species.GenderRatio) != FixedGender)
                return false;

            if (FixedNature != Nature.other && (Nature)(pid % 25) != FixedNature)
                return false;

            return true;
        }

        public FixedSlot(string name, Gender g, Nature n) : base(name, 50)
        {
            FixedGender = g;
            FixedNature = n;
        }
    }

    public class GenderFixedSlot : GCSlot
    {
        public Gender FixedGender { get; }

        protected override bool IsValidPID(uint pid)
            => FixedGender == Gender.Genderless || pid.GetGender(Species.GenderRatio) == FixedGender;

        public GenderFixedSlot(Pokemon.Species p, Gender g) : base(p, 50)
        {
            FixedGender = g;
        }
        public GenderFixedSlot(string name, uint lv, Gender g) : base(name, lv)
        {
            FixedGender = g;
        }
    }

    /// <summary>
    /// 個体値が0固定の個体
    /// </summary>
    public class ExtraGCSlot : GCSlot
    {
        public Gender FixedGender { get; }
        public Nature FixedNature { get; }

        protected override bool IsValidPID(uint pid)
        {
            if (FixedGender != Gender.Genderless && pid.GetGender(Species.GenderRatio) != FixedGender)
                return false;

            if (FixedNature != Nature.other && (Nature)(pid % 25) != FixedNature)
                return false;

            return true;
        }

        protected override uint[] GenerateIVs(ref uint seed)
        {
            // 生成処理自体は走る
            seed.Advance(2);

            return new uint[6];
        }

        public ExtraGCSlot(string name, uint lv, Gender gender, Nature nature) : base(name, lv) 
            => (FixedGender, FixedNature) = (gender, nature);
    }
}
