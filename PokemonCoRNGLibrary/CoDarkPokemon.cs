using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.GCLCG;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary
{
    /// <summary>
    /// ダークポケモンの情報をまとめたクラスです. 
    /// </summary>
    public class CoDarkPokemon : IGeneratable<GCIndividual>
    {
        public readonly GCSlot darkPokemon;
        public readonly IReadOnlyList<GCSlot> preGeneratePokemons;

        internal CoDarkPokemon(string name, uint lv)
        {
            darkPokemon = new GCSlot(name, lv);
            preGeneratePokemons = new GCSlot[0];
        }
        internal CoDarkPokemon(string name, uint lv, GCSlot[] preGenerate)
        {
            darkPokemon = new GCSlot(name, lv);
            preGeneratePokemons = preGenerate;
        }

        public GCIndividual Generate(uint seed)
        {
            uint DummyTSV = seed.GetRand() ^ seed.GetRand();
            foreach(var pokemon in preGeneratePokemons)
                pokemon.Generate(seed, out seed, DummyTSV);

            return darkPokemon.Generate(seed, out seed, DummyTSV);
        }

        public IEnumerable<(uint seed, GCIndividual Individual)> CalcBack(uint h, uint a, uint b, uint c, uint d, uint s, bool deduplication = false)
        {
            foreach (var genSeed in SeedFinder.FindGeneratingSeed(h, a, b, c, d, s, false))
            {
                var stack = new Stack<(int Index, CalcBackCell Cell)>();
                stack.Push((preGeneratePokemons.Count, CalcBackCell.CreateCell(darkPokemon, genSeed)));
                var loopBreak = false;
                while (!loopBreak && stack.Count > 0)
                {
                    (var index, var cell) = stack.Pop(); 
                    if (index-- == 0)
                    {
                        // tsv判定.
                        if (!cell.CheckTSVGeneration()) continue;

                        yield return cell.GetResult();

                        loopBreak = deduplication; // 重複を除く場合はloopBreakをtrueにしてループを抜ける.

                        continue;
                    }
                    foreach (var _c in cell.GetGeneratableCell(preGeneratePokemons[index]))
                        stack.Push((index, _c));
                }
            }
        }

        internal static readonly IReadOnlyList<CoDarkPokemon> darkPokemonList;
        internal static readonly Dictionary<string, CoDarkPokemon> darkPokemonDictionary;

        public static CoDarkPokemon GetDarkPokemon(int index) => 0 <= index && index < darkPokemonList.Count ? darkPokemonList[index] : null;
        public static CoDarkPokemon GetDarkPokemon(string name) => darkPokemonDictionary.TryGetValue(name, out var p) ? p : null;
        public static IReadOnlyList<CoDarkPokemon> GetAllCoDarkPokemons() { return darkPokemonList; }

        static CoDarkPokemon()
        {
            var CoList = new List<CoDarkPokemon>();
            CoList.Add(new CoDarkPokemon("マクノシタ", 30, new GCSlot[] {
                new GCSlot("ヨマワル", Gender.Male, Nature.Quirky),
                new GCSlot("イトマル", Gender.Female, Nature.Hardy)
            }));
            CoList.Add(new CoDarkPokemon("ベイリーフ", 30));
            CoList.Add(new CoDarkPokemon("マグマラシ", 30));
            CoList.Add(new CoDarkPokemon("アリゲイツ", 30));
            CoList.Add(new CoDarkPokemon("ヨルノズク", 30));
            CoList.Add(new CoDarkPokemon("モココ", 30));
            CoList.Add(new CoDarkPokemon("ポポッコ", 30));
            CoList.Add(new CoDarkPokemon("ヌオー", 30));
            CoList.Add(new CoDarkPokemon("ムウマ", 30));
            CoList.Add(new CoDarkPokemon("マグマッグ", 30));
            CoList.Add(new CoDarkPokemon("オオタチ", 33));
            CoList.Add(new CoDarkPokemon("ヤンヤンマ", 33));
            CoList.Add(new CoDarkPokemon("テッポウオ", 20));
            CoList.Add(new CoDarkPokemon("マンタイン", 33));
            CoList.Add(new CoDarkPokemon("ハリーセン", 33));
            CoList.Add(new CoDarkPokemon("アサナン", 33));
            CoList.Add(new CoDarkPokemon("ノコッチ", 33));
            CoList.Add(new CoDarkPokemon("チルット", 33));
            CoList.Add(new CoDarkPokemon("ウソッキー", 35));
            CoList.Add(new CoDarkPokemon("カポエラー", 38));
            CoList.Add(new CoDarkPokemon("エンテイ", 40));
            CoList.Add(new CoDarkPokemon("レディアン", 40));
            CoList.Add(new CoDarkPokemon("スイクン", 40));
            CoList.Add(new CoDarkPokemon("グライガー", 43, new GCSlot[] {
                new GCSlot("ヒメグマ", Gender.Male, Nature.Serious),
                new GCSlot("プリン", Gender.Female, Nature.Docile),
                new GCSlot("キノココ", Gender.Male, Nature.Bashful)
            }));
            CoList.Add(new CoDarkPokemon("オドシシ", 43));
            CoList.Add(new CoDarkPokemon("イノムー", 43));
            CoList.Add(new CoDarkPokemon("ニューラ", 43));
            CoList.Add(new CoDarkPokemon("エイパム", 43));
            CoList.Add(new CoDarkPokemon("ヤミカラス", 43, new GCSlot[] {
                new GCSlot("キバニア", Gender.Male, Nature.Docile),
                new GCSlot("コノハナ", Gender.Female, Nature.Serious),
                new GCSlot("デルビル", Gender.Male, Nature.Bashful)
            }));
            CoList.Add(new CoDarkPokemon("フォレトス", 43));
            CoList.Add(new CoDarkPokemon("アリアドス", 43));
            CoList.Add(new CoDarkPokemon("グランブル", 43));
            CoList.Add(new CoDarkPokemon("ビブラーバ", 43));
            CoList.Add(new CoDarkPokemon("ライコウ", 40));
            CoList.Add(new CoDarkPokemon("キマワリ", 45));
            CoList.Add(new CoDarkPokemon("デリバード", 45));
            CoList.Add(new CoDarkPokemon("ヘラクロス", 45, new GCSlot[] {
                new GCSlot("アメモース", Gender.Male, Nature.Hardy),
                new GCSlot("アリアドス", Gender.Female, Nature.Hardy),
            }));
            CoList.Add(new CoDarkPokemon("エアームド", 47));
            CoList.Add(new CoDarkPokemon("ミルタンク", 48));
            CoList.Add(new CoDarkPokemon("アブソル", 48));
            CoList.Add(new CoDarkPokemon("ヘルガー", 48));
            CoList.Add(new CoDarkPokemon("トロピウス", 49));
            CoList.Add(new CoDarkPokemon("メタグロス", 50));
            CoList.Add(new CoDarkPokemon("バンギラス", 55));
            CoList.Add(new CoDarkPokemon("ドーブル", 45));
            CoList.Add(new CoDarkPokemon("リングマ", 45, new GCSlot[] {
                new GCSlot("ゴーリキー", Gender.Female, Nature.Calm),
                new GCSlot("ヌマクロー", Gender.Male, Nature.Mild),
                new GCSlot("ダーテング", Gender.Female, Nature.Gentle)
            }));
            CoList.Add(new CoDarkPokemon("ツボツボ", 45));
            CoList.Add(new CoDarkPokemon("トゲチック", 20));

            darkPokemonList = CoList;
            darkPokemonDictionary = CoList.ToDictionary(_ => _.darkPokemon.pokemon.Name, _ => _);
        }
    }
}
