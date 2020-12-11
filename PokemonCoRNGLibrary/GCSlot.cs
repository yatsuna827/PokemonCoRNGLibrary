using System;
using PokemonPRNG.LCG32.GCLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;
using PokemonStandardLibrary.PokeDex.Gen3;

namespace PokemonCoRNGLibrary
{
    // 生成可能な個体の最低限の情報をまとめたクラス.
    public class GCSlot
    {
        public readonly uint Lv;
        public readonly Pokemon.Species pokemon;
        public readonly Gender fixedGender;
        public readonly Nature fixedNature;

        public GCIndividual Generate(uint seed, out uint finSeed, uint tsv = 0x10000)
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
                if (fixedGender != Gender.Genderless && pid.GetGender(pokemon.GenderRatio) != fixedGender)
                    continue;
                if (fixedNature != Nature.other && (Nature)(pid % 25) != fixedNature)
                    continue;
                if (skip = pid.IsShiny(tsv))
                    continue;

                break;
            }

            finSeed = seed;
            return pokemon.GetIndividual(Lv, IVs, pid, abilityIndex).SetRepSeed(rep).SetShinySkipped(skip);
        }

        internal GCSlot(Pokemon.Species p, Gender g = Gender.Genderless, Nature n = Nature.other)
        {
            pokemon = p;
            Lv = 50;
            fixedGender = g;
            fixedNature = n;
        }
        internal GCSlot(string name, Gender g = Gender.Genderless, Nature n = Nature.other)
        {
            pokemon = Pokemon.GetPokemon(name);
            Lv = 50;
            fixedGender = g;
            fixedNature = n;
        }
        internal GCSlot(string name, uint lv, Gender g = Gender.Genderless, Nature n = Nature.other)
        {
            pokemon = Pokemon.GetPokemon(name);
            Lv = lv;
            fixedGender = g;
            fixedNature = n;
        }
    }
}
