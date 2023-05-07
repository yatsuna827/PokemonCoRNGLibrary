using System.Collections.Generic;
using PokemonPRNG.LCG32.GCLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.Gen3;

namespace PokemonCoRNGLibrary.AdvanceSource
{
    public class EnterBattleNow : ISeedEnumeratorHandler
    {
        private static readonly GenderFixedSlot[][] dummyTeam = new GenderFixedSlot[][]
        {
            new[] {
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "FemaleOnly"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M3F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Male),
            },
            new[] {
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
            },
            new[] {
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F3"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
            },
            new[] {
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "FemaleOnly"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
            }
        };

        public uint Advance(uint seed)
        {
            seed.Advance(118);
            foreach (var team in dummyTeam)
            {
                seed.Advance(2); // tsv生成されてるけど色回避判定は無いらしい？
                foreach (var poke in team)
                    poke.Generate(ref seed);
            }
            seed.Advance(7); // 完全に条件無しのダミーの生成

            return seed;
        }

        public uint Initialize(uint seed)
            => seed;

        public uint SelectCurrent(uint seed)
            => seed;
    }
}
