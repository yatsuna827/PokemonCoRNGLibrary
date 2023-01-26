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
    public partial class CoDarkPokemon : IGeneratable<RNGResult<GCIndividual>>
    {
        public readonly GCSlot slot;
        public readonly IReadOnlyList<GCSlot> preGeneratePokemons;

        private CoDarkPokemon(string name, uint lv)
        {
            slot = new GCSlot(name, lv);
            preGeneratePokemons = new GCSlot[0];
        }
        private CoDarkPokemon(string name, uint lv, GCSlot[] preGenerate)
        {
            slot = new GCSlot(name, lv);
            preGeneratePokemons = preGenerate;
        }
        private CoDarkPokemon(GCSlot slot, GCSlot[] preGenerate)
        {
            this.slot = slot;
            preGeneratePokemons = preGenerate;
        }

        public RNGResult<GCIndividual> Generate(uint seed)
        {
            var head = seed;
            uint DummyTSV = seed.GetRand() ^ seed.GetRand();
            foreach(var pokemon in preGeneratePokemons)
                pokemon.Generate(ref seed, DummyTSV);

            var res = slot.Generate(ref seed, DummyTSV);

            return new RNGResult<GCIndividual>(res, head, seed);
        }
        public IEnumerable<(uint seed, GCIndividual Individual)> CalcBack(uint h, uint a, uint b, uint c, uint d, uint s, bool deduplication = false)
        {
            foreach (var genSeed in SeedFinder.FindGeneratingSeed(h, a, b, c, d, s, false))
            {
                var stack = new Stack<(int Index, CalcBackCell Cell)>();
                stack.Push((preGeneratePokemons.Count, CalcBackCell.CreateCell(slot, genSeed)));
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
    }

    public partial class CoDarkPokemon
    {

        private static readonly IReadOnlyList<CoDarkPokemon> darkPokemonList;
        private static readonly Dictionary<string, CoDarkPokemon> darkPokemonDictionary;

        public static CoDarkPokemon GetDarkPokemon(int index) => 0 <= index && index < darkPokemonList.Count ? darkPokemonList[index] : null;
        public static CoDarkPokemon GetDarkPokemon(string name) => darkPokemonDictionary.TryGetValue(name, out var p) ? p : null;
        public static IReadOnlyList<CoDarkPokemon> GetAllCoDarkPokemons() { return darkPokemonList; }

        static CoDarkPokemon()
        {
            var list = new List<CoDarkPokemon>
            {
                new CoDarkPokemon("マクノシタ", 30, new GCSlot[] {
                    new GCSlot("ヨマワル", Gender.Male, Nature.Quirky),
                    new GCSlot("イトマル", Gender.Female, Nature.Hardy)
                }),
                new CoDarkPokemon("ベイリーフ", 30),
                new CoDarkPokemon("マグマラシ", 30),
                new CoDarkPokemon("アリゲイツ", 30),
                new CoDarkPokemon("ヨルノズク", 30),
                new CoDarkPokemon("モココ", 30),
                new CoDarkPokemon("ポポッコ", 30),
                new CoDarkPokemon("ヌオー", 30),
                new CoDarkPokemon("ムウマ", 30),
                new CoDarkPokemon("マグマッグ", 30),
                new CoDarkPokemon("オオタチ", 33),
                new CoDarkPokemon("ヤンヤンマ", 33),
                new CoDarkPokemon("テッポウオ", 20),
                new CoDarkPokemon("マンタイン", 33),
                new CoDarkPokemon("ハリーセン", 33),
                new CoDarkPokemon("アサナン", 33),
                new CoDarkPokemon("ノコッチ", 33),
                new CoDarkPokemon("チルット", 33),
                new CoDarkPokemon("ウソッキー", 35),
                new CoDarkPokemon("カポエラー", 38),
                new CoDarkPokemon("エンテイ", 40),
                new CoDarkPokemon("レディアン", 40),
                new CoDarkPokemon("スイクン", 40),
                new CoDarkPokemon("グライガー", 43, new GCSlot[] {
                    new GCSlot("ヒメグマ", Gender.Male, Nature.Serious),
                    new GCSlot("プリン", Gender.Female, Nature.Docile),
                    new GCSlot("キノココ", Gender.Male, Nature.Bashful)
                }),
                new CoDarkPokemon("オドシシ", 43),
                new CoDarkPokemon("イノムー", 43),
                new CoDarkPokemon("ニューラ", 43),
                new CoDarkPokemon("エイパム", 43),
                new CoDarkPokemon("ヤミカラス", 43, new GCSlot[] {
                    new GCSlot("キバニア", Gender.Male, Nature.Docile),
                    new GCSlot("コノハナ", Gender.Female, Nature.Serious),
                    new GCSlot("デルビル", Gender.Male, Nature.Bashful)
                }),
                new CoDarkPokemon("フォレトス", 43),
                new CoDarkPokemon("アリアドス", 43),
                new CoDarkPokemon("グランブル", 43),
                new CoDarkPokemon("ビブラーバ", 43),
                new CoDarkPokemon("ライコウ", 40),
                new CoDarkPokemon("キマワリ", 45),
                new CoDarkPokemon("デリバード", 45),
                new CoDarkPokemon("ヘラクロス", 45, new GCSlot[] {
                    new GCSlot("アメモース", Gender.Male, Nature.Hardy),
                    new GCSlot("アリアドス", Gender.Female, Nature.Hardy),
                }),
                new CoDarkPokemon("エアームド", 47),
                new CoDarkPokemon("ミルタンク", 48),
                new CoDarkPokemon("アブソル", 48),
                new CoDarkPokemon("ヘルガー", 48),
                new CoDarkPokemon("トロピウス", 49),
                new CoDarkPokemon("メタグロス", 50),
                new CoDarkPokemon("バンギラス", 55),
                new CoDarkPokemon("ドーブル", 45),
                new CoDarkPokemon("リングマ", 45, new GCSlot[] {
                    new GCSlot("ゴーリキー", Gender.Female, Nature.Calm),
                    new GCSlot("ヌマクロー", Gender.Male, Nature.Mild),
                    new GCSlot("ダーテング", Gender.Female, Nature.Gentle)
                }),
                new CoDarkPokemon("ツボツボ", 45),
                new CoDarkPokemon("トゲチック", 20),

                new CoDarkPokemon(new ExtraGCSlot("トゲピー", 20), new GCSlot[] {
                    new GCSlot("ヤミラミ", Gender.Male, Nature.Careful),
                    new GCSlot("ベトベター", Gender.Male, Nature.Impish),
                    new GCSlot("ゴクリン", Gender.Male, Nature.Quirky)
                }),
                new CoDarkPokemon(new ExtraGCSlot("メリープ", 37), new GCSlot[] {
                    new GCSlot("エネコ", Gender.Female, Nature.Naughty),
                    new GCSlot("ハリーセン", Gender.Female, Nature.Timid),
                    new GCSlot("ヨマワル", Gender.Female, Nature.Serious)
                }),
                new CoDarkPokemon(new ExtraGCSlot("ハッサム", 50), new GCSlot[] {
                    new GCSlot("ヤミカラス", Gender.Female, Nature.Jolly),
                    new GCSlot("ネンドール", Gender.Genderless, Nature.Brave),
                    new GCSlot("ハガネール", Gender.Male, Nature.Adamant)
                })
            };

            darkPokemonList = list;
            darkPokemonDictionary = list.ToDictionary(_ => _.slot.Pokemon.Name, _ => _);
        }
    }
}
