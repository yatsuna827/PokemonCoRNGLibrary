using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;
using PokemonStandardLibrary.Gen3;
using PokemonPRNG.LCG32.GCLCG;
using PokemonPRNG.LCG32;

namespace PokemonCoRNGLibrary
{
    public static class GameTitleAdvances
    {
        private static readonly GenderFixedSlot dummySlot = new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Male);
        private static readonly IReadOnlyList<IReadOnlyList<GenderFixedSlot>> dummyTeam = new GenderFixedSlot[][]
        {
            new GenderFixedSlot[] {
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "FemaleOnly"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M3F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Male),
            },
            new GenderFixedSlot[] {
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
            },
            new GenderFixedSlot[] {
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F3"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
            },
            new GenderFixedSlot[] {
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "FemaleOnly"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Female),
                new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M1F1"), Gender.Male),
            }
        };

        /// <summary>
        /// タイトル画面入る前(ロゴが出てくるあたり？)で発生する消費.
        /// 消費後の現在seedを返します.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="finSeed"></param>
        /// <returns></returns>
        public static uint GameInitialization(this uint seed)
        {
            seed.Advance1000();
            seed.Advance(2); // tsv生成されてるけど色回避判定は無いらしい？

            // なんかエフィブラっぽいよね
            dummySlot.Generate(ref seed);
            dummySlot.Generate(ref seed);
            seed.Advance(2); // 用途不明

            return seed;
        }

        /// <summary>
        /// とにかくバトルの画面に入るときに発生する消費.
        /// 消費後の現在seedを返します.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="finSeed"></param>
        /// <returns></returns>
        public static uint GenerateDummyTeam(this uint seed)
        {
            seed.Advance(118);
            foreach(var team in dummyTeam)
            {
                seed.Advance(2); // tsv生成されてるけど色回避判定は無いらしい？
                foreach (var poke in team)
                    poke.Generate(ref seed);
            }
            seed.Advance(7); // 完全に条件無しのダミーの生成

            return seed;
        }
    }

    /// <summary>
    /// タイトル画面入る前(ロゴが出てくるあたり？)で発生する消費.
    /// </summary>
    public class GameInitialization : ILcgUser
    {
        private static readonly GenderFixedSlot dummySlot = new GenderFixedSlot(Pokemon.GetPokemon("Dummy", "M7F1"), Gender.Male);

        public void Use(ref uint seed)
        {
            seed.Advance1000();
            seed.Advance(2); // tsv生成されてるけど色回避判定は無いらしい？

            // なんかエフィブラっぽいよね
            dummySlot.Generate(ref seed);
            dummySlot.Generate(ref seed);
            seed.Advance(2); // 用途不明
        }
    }

    /// <summary>
    /// とにかくバトルの画面に入るときに発生する消費.
    /// </summary>
    public class GenerateDummyTeam : ILcgUser
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

        public void Use(ref uint seed)
        {
            seed.Advance(118);
            foreach (var team in dummyTeam)
            {
                seed.Advance(2); // tsv生成されてるけど色回避判定は無いらしい？
                foreach (var poke in team)
                    seed.Used(poke, 0x10000u);
            }
            seed.Advance(7); // 完全に条件無しのダミーの生成
        }
    }
}
