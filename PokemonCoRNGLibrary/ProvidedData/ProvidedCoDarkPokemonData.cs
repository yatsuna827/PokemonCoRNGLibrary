using PokemonStandardLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonCoRNGLibrary.ProvidedData
{
    public sealed partial class ProvidedCoDarkPokemonData
    {
        public GCSlot Slot { get; }
        public IReadOnlyList<FixedSlot> PreGeneratePokemons { get; }

        public DarkPokemonGenerator GetGenerator(FixedSlot[] preGeneratePokemonsOverride = null)
            => new DarkPokemonGenerator(Slot, preGeneratePokemonsOverride ?? PreGeneratePokemons.ToArray());

        private ProvidedCoDarkPokemonData(string name, uint lv)
        {
            Slot = new GCSlot(name, lv);
            PreGeneratePokemons = new FixedSlot[0];
        }
        private ProvidedCoDarkPokemonData(string name, uint lv, FixedSlot[] preGenerate)
        {
            Slot = new GCSlot(name, lv);
            PreGeneratePokemons = preGenerate;
        }
        private ProvidedCoDarkPokemonData(GCSlot slot, FixedSlot[] preGenerate)
        {
            Slot = slot;
            PreGeneratePokemons = preGenerate;
        }
    }

    partial class ProvidedCoDarkPokemonData
    {
        private static readonly ProvidedCoDarkPokemonData[] darkPokemons;
        private static readonly Dictionary<string, ProvidedCoDarkPokemonData> darkPokemonDictionary;

        public static ProvidedCoDarkPokemonData Get(int index) => 
            0 <= index && index < darkPokemons.Length ? darkPokemons[index] : null;
        public static ProvidedCoDarkPokemonData Get(string name) 
            => darkPokemonDictionary.TryGetValue(name, out var p) ? p : null;
        public static ProvidedCoDarkPokemonData[] GetAll() 
            => darkPokemons.ToArray();

        static ProvidedCoDarkPokemonData()
        {
            darkPokemons = new ProvidedCoDarkPokemonData[]
            {
                new ProvidedCoDarkPokemonData("マクノシタ", 30, new FixedSlot[] {
                    new FixedSlot("ヨマワル", Gender.Male, Nature.Quirky),
                    new FixedSlot("イトマル", Gender.Female, Nature.Hardy)
                }),
                new ProvidedCoDarkPokemonData("ベイリーフ", 30),
                new ProvidedCoDarkPokemonData("マグマラシ", 30),
                new ProvidedCoDarkPokemonData("アリゲイツ", 30),
                new ProvidedCoDarkPokemonData("ヨルノズク", 30),
                new ProvidedCoDarkPokemonData("モココ", 30),
                new ProvidedCoDarkPokemonData("ポポッコ", 30),
                new ProvidedCoDarkPokemonData("ヌオー", 30),
                new ProvidedCoDarkPokemonData("ムウマ", 30),
                new ProvidedCoDarkPokemonData("マグマッグ", 30),
                new ProvidedCoDarkPokemonData("オオタチ", 33),
                new ProvidedCoDarkPokemonData("ヤンヤンマ", 33),
                new ProvidedCoDarkPokemonData("テッポウオ", 20),
                new ProvidedCoDarkPokemonData("マンタイン", 33),
                new ProvidedCoDarkPokemonData("ハリーセン", 33),
                new ProvidedCoDarkPokemonData("アサナン", 33),
                new ProvidedCoDarkPokemonData("ノコッチ", 33),
                new ProvidedCoDarkPokemonData("チルット", 33),
                new ProvidedCoDarkPokemonData("ウソッキー", 35),
                new ProvidedCoDarkPokemonData("カポエラー", 38),
                new ProvidedCoDarkPokemonData("エンテイ", 40),
                new ProvidedCoDarkPokemonData("レディアン", 40),
                new ProvidedCoDarkPokemonData("スイクン", 40),
                new ProvidedCoDarkPokemonData("グライガー", 43, new FixedSlot[] {
                    new FixedSlot("ヒメグマ", Gender.Male, Nature.Serious),
                    new FixedSlot("プリン", Gender.Female, Nature.Docile),
                    new FixedSlot("キノココ", Gender.Male, Nature.Bashful)
                }),
                new ProvidedCoDarkPokemonData("オドシシ", 43),
                new ProvidedCoDarkPokemonData("イノムー", 43),
                new ProvidedCoDarkPokemonData("ニューラ", 43),
                new ProvidedCoDarkPokemonData("エイパム", 43),
                new ProvidedCoDarkPokemonData("ヤミカラス", 43, new FixedSlot[] {
                    new FixedSlot("キバニア", Gender.Male, Nature.Docile),
                    new FixedSlot("コノハナ", Gender.Female, Nature.Serious),
                    new FixedSlot("デルビル", Gender.Male, Nature.Bashful)
                }),
                new ProvidedCoDarkPokemonData("フォレトス", 43),
                new ProvidedCoDarkPokemonData("アリアドス", 43),
                new ProvidedCoDarkPokemonData("グランブル", 43),
                new ProvidedCoDarkPokemonData("ビブラーバ", 43),
                new ProvidedCoDarkPokemonData("ライコウ", 40),
                new ProvidedCoDarkPokemonData("キマワリ", 45),
                new ProvidedCoDarkPokemonData("デリバード", 45),
                new ProvidedCoDarkPokemonData("ヘラクロス", 45, new FixedSlot[] {
                    new FixedSlot("アメモース", Gender.Male, Nature.Hardy),
                    new FixedSlot("アリアドス", Gender.Female, Nature.Hardy),
                }),
                new ProvidedCoDarkPokemonData("エアームド", 47),
                new ProvidedCoDarkPokemonData("ミルタンク", 48),
                new ProvidedCoDarkPokemonData("アブソル", 48),
                new ProvidedCoDarkPokemonData("ヘルガー", 48),
                new ProvidedCoDarkPokemonData("トロピウス", 49),
                new ProvidedCoDarkPokemonData("メタグロス", 50),
                new ProvidedCoDarkPokemonData("バンギラス", 55),
                new ProvidedCoDarkPokemonData("ドーブル", 45),
                new ProvidedCoDarkPokemonData("リングマ", 45, new FixedSlot[] {
                    new FixedSlot("ゴーリキー", Gender.Female, Nature.Calm),
                    new FixedSlot("ヌマクロー", Gender.Male, Nature.Mild),
                    new FixedSlot("ダーテング", Gender.Female, Nature.Gentle)
                }),
                new ProvidedCoDarkPokemonData("ツボツボ", 45),
                new ProvidedCoDarkPokemonData("トゲチック", 20),
                new ProvidedCoDarkPokemonData(new ExtraGCSlot("トゲピー", 20, Gender.Female, Nature.Sassy), new FixedSlot[] {
                    new FixedSlot("ヤミラミ", Gender.Male, Nature.Careful),
                    new FixedSlot("ベトベター", Gender.Male, Nature.Impish),
                    new FixedSlot("ゴクリン", Gender.Male, Nature.Quirky)
                }),
                new ProvidedCoDarkPokemonData(new ExtraGCSlot("メリープ", 37, Gender.Female, Nature.Mild), new FixedSlot[] {
                    new FixedSlot("エネコ", Gender.Female, Nature.Naughty),
                    new FixedSlot("ハリーセン", Gender.Female, Nature.Timid),
                    new FixedSlot("ヨマワル", Gender.Female, Nature.Serious)
                }),
                new ProvidedCoDarkPokemonData(new ExtraGCSlot("ハッサム", 50, Gender.Male, Nature.Hasty), new FixedSlot[] {
                    new FixedSlot("ヤミカラス", Gender.Female, Nature.Jolly),
                    new FixedSlot("ネンドール", Gender.Genderless, Nature.Brave),
                    new FixedSlot("ハガネール", Gender.Male, Nature.Adamant)
                })
            };
            darkPokemonDictionary = darkPokemons.ToDictionary(_ => _.Slot.Species.Name, _ => _);
        }
    }
}
