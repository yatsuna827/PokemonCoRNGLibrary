using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.GCLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;
using PokemonStandardLibrary.PokeDex.Gen3;

namespace PokemonCoRNGLibrary
{
    // 生成可能な個体の最低限の情報をまとめたクラス.
    public class GCSlot : ISideEffectiveGeneratable<GCIndividual, uint>
    {
        public uint Lv { get; }
        public Pokemon.Species Pokemon { get; }
        public Gender FixedGender { get; }
        public Nature FixedNature { get; }

        public GCIndividual Generate(ref uint seed, uint tsv = 0x10000)
        {
            var rep = seed;
            seed.Advance(2); // dummyPID
            uint[] IVs = seed.GetIVs();
            var abilityIndex = seed.GetRand(2);
            uint pid;
            bool skip;
            while (true)
            {
                pid = (seed.GetRand() << 16) | seed.GetRand() ;
                if (FixedGender != Gender.Genderless && pid.GetGender(Pokemon.GenderRatio) != FixedGender)
                    continue;
                if (FixedNature != Nature.other && (Nature)(pid % 25) != FixedNature)
                    continue;
                if (skip = pid.IsShiny(tsv))
                    continue;

                break;
            }

            return Pokemon.GetIndividual(Lv, IVs, pid, abilityIndex).SetRepSeed(rep).SetShinySkipped(skip);
        }

        public GCSlot(Pokemon.Species p, Gender g = Gender.Genderless, Nature n = Nature.other)
        {
            Pokemon = p;
            Lv = 50;
            FixedGender = g;
            FixedNature = n;
        }
        public GCSlot(string name, Gender g = Gender.Genderless, Nature n = Nature.other)
        {
            Pokemon = PokemonStandardLibrary.PokeDex.Gen3.Pokemon.GetPokemon(name);
            Lv = 50;
            FixedGender = g;
            FixedNature = n;
        }
        public GCSlot(string name, uint lv, Gender g = Gender.Genderless, Nature n = Nature.other)
        {
            Pokemon = PokemonStandardLibrary.PokeDex.Gen3.Pokemon.GetPokemon(name);
            Lv = lv;
            FixedGender = g;
            FixedNature = n;
        }
    }
}
