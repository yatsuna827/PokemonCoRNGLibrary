using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PokemonStandardLibrary;
using PokemonStandardLibrary.Gen3;
using PokemonPRNG;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary
{
    public static class BattleNow
    {
        public static class SingleBattle
        {
            public static readonly RentalTeamRank Ultimate = new RentalTeamRank("シングル最強", new FixedSlot[][]
            {
                new FixedSlot[] {
                    new FixedSlot("バシャーモ", Gender.Male, Nature.Sassy),
                    new FixedSlot("ラフレシア", Gender.Female, Nature.Gentle),
                    new FixedSlot("ランターン", Gender.Female, Nature.Modest),
                    new FixedSlot("オニゴーリ", Gender.Male, Nature.Rash),
                    new FixedSlot("グランブル", Gender.Male, Nature.Naughty),
                    new FixedSlot("ジュペッタ", Gender.Female, Nature.Naughty),
                },
                new FixedSlot[] {
                    new FixedSlot("エンテイ", Gender.Genderless, Nature.Hasty),
                    new FixedSlot("ゴローニャ", Gender.Female, Nature.Impish),
                    new FixedSlot("ベトベトン", Gender.Male, Nature.Lonely),
                    new FixedSlot("コータス", Gender.Male, Nature.Mild),
                    new FixedSlot("ライボルト", Gender.Female, Nature.Mild),
                    new FixedSlot("ドククラゲ", Gender.Male, Nature.Serious),
                },
                new FixedSlot[] {
                    new FixedSlot("ラグラージ", Gender.Male, Nature.Brave),
                    new FixedSlot("フーディン", Gender.Female, Nature.Mild),
                    new FixedSlot("ルンパッパ", Gender.Male, Nature.Modest),
                    new FixedSlot("トドゼルガ", Gender.Female, Nature.Bashful),
                    new FixedSlot("ゴルダック", Gender.Male, Nature.Modest),
                    new FixedSlot("バクオング", Gender.Female, Nature.Adamant),
                },
                new FixedSlot[] {
                    new FixedSlot("ライコウ", Gender.Genderless, Nature.Mild),
                    new FixedSlot("キュウコン", Gender.Female, Nature.Rash),
                    new FixedSlot("マタドガス", Gender.Female, Nature.Adamant),
                    new FixedSlot("ツボツボ", Gender.Female, Nature.Sassy),
                    new FixedSlot("アーマルド", Gender.Male, Nature.Adamant),
                    new FixedSlot("ネイティオ", Gender.Male, Nature.Quirky),
                },
                new FixedSlot[] {
                    new FixedSlot("メガニウム", Gender.Male, Nature.Quiet),
                    new FixedSlot("バクフーン", Gender.Male, Nature.Mild),
                    new FixedSlot("オーダイル", Gender.Male, Nature.Modest),
                    new FixedSlot("エーフィ", Gender.Male, Nature.Rash),
                    new FixedSlot("ブラッキー", Gender.Male, Nature.Bold),
                    new FixedSlot("カイロス", Gender.Female, Nature.Naughty),
                },
                new FixedSlot[] {
                    new FixedSlot("スイクン", Gender.Genderless, Nature.Modest),
                    new FixedSlot("デンリュウ", Gender.Female, Nature.Quiet),
                    new FixedSlot("ネンドール", Gender.Genderless, Nature.Lonely),
                    new FixedSlot("オドシシ", Gender.Male, Nature.Adamant),
                    new FixedSlot("ポリゴン2", Gender.Genderless, Nature.Rash),
                    new FixedSlot("ドンファン", Gender.Female, Nature.Adamant),
                },
                new FixedSlot[] {
                    new FixedSlot("メタグロス", Gender.Genderless, Nature.Lonely),
                    new FixedSlot("ユレイドル", Gender.Male, Nature.Impish),
                    new FixedSlot("カイリキー", Gender.Male, Nature.Adamant),
                    new FixedSlot("エアームド", Gender.Female, Nature.Lonely),
                    new FixedSlot("サイドン", Gender.Female, Nature.Adamant),
                    new FixedSlot("ハリテヤマ", Gender.Male, Nature.Adamant),
                },
                new FixedSlot[] {
                    new FixedSlot("ヘラクロス", Gender.Female, Nature.Adamant),
                    new FixedSlot("ソーナンス", Gender.Male, Nature.Timid),
                    new FixedSlot("ミロカロス", Gender.Female, Nature.Modest),
                    new FixedSlot("ドードリオ", Gender.Male, Nature.Adamant),
                    new FixedSlot("ノクタス", Gender.Female, Nature.Modest),
                    new FixedSlot("ヤミラミ", Gender.Male, Nature.Adamant),
                }
            });
            public static readonly RentalTeamRank Hard = new RentalTeamRank("シングル強い", new FixedSlot[][]
            {
                new FixedSlot[] {
                    new FixedSlot("バクーダ", Gender.Male, Nature.Impish),
                    new FixedSlot("ランターン", Gender.Female, Nature.Quirky),
                    new FixedSlot("ラフレシア", Gender.Female, Nature.Careful),
                    new FixedSlot("マルノーム", Gender.Male, Nature.Docile),
                    new FixedSlot("アブソル", Gender.Female, Nature.Bold),
                    new FixedSlot("オドシシ", Gender.Female, Nature.Bashful),
                },
                new FixedSlot[] {
                    new FixedSlot("ブーピッグ", Gender.Female, Nature.Naughty),
                    new FixedSlot("ハリテヤマ", Gender.Male, Nature.Modest),
                    new FixedSlot("グランブル", Gender.Female, Nature.Lonely),
                    new FixedSlot("ジュペッタ", Gender.Male, Nature.Adamant),
                    new FixedSlot("コータス", Gender.Male, Nature.Hasty),
                    new FixedSlot("ライチュウ", Gender.Male, Nature.Sassy),
                },
                new FixedSlot[] {
                    new FixedSlot("ダーテング", Gender.Male, Nature.Bold),
                    new FixedSlot("マルマイン", Gender.Genderless, Nature.Relaxed),
                    new FixedSlot("バクオング", Gender.Male, Nature.Impish),
                    new FixedSlot("ドククラゲ", Gender.Male, Nature.Quirky),
                    new FixedSlot("ゴローニャ", Gender.Female, Nature.Gentle),
                    new FixedSlot("オニゴーリ", Gender.Male, Nature.Docile),
                },
                new FixedSlot[] {
                    new FixedSlot("キレイハナ", Gender.Female, Nature.Careful),
                    new FixedSlot("サイドン", Gender.Male, Nature.Calm),
                    new FixedSlot("サクラビス", Gender.Male, Nature.Gentle),
                    new FixedSlot("マタドガス", Gender.Female, Nature.Docile),
                    new FixedSlot("レアコイル", Gender.Genderless, Nature.Careful),
                    new FixedSlot("フーディン", Gender.Male, Nature.Relaxed),
                },
                new FixedSlot[] {
                    new FixedSlot("ユレイドル", Gender.Male, Nature.Naughty),
                    new FixedSlot("カイロス", Gender.Female, Nature.Bold),
                    new FixedSlot("アーマルド", Gender.Male, Nature.Calm),
                    new FixedSlot("ミルタンク", Gender.Female, Nature.Relaxed),
                    new FixedSlot("ネンドール", Gender.Genderless, Nature.Sassy),
                    new FixedSlot("ホエルオー", Gender.Female, Nature.Careful),
                },
                new FixedSlot[] {
                    new FixedSlot("ドンファン", Gender.Female, Nature.Gentle),
                    new FixedSlot("ゴルダック", Gender.Male, Nature.Jolly),
                    new FixedSlot("ザングース", Gender.Male, Nature.Brave),
                    new FixedSlot("デンリュウ", Gender.Female, Nature.Impish),
                    new FixedSlot("ヘラクロス", Gender.Male, Nature.Bold),
                    new FixedSlot("ヘルガー", Gender.Male, Nature.Relaxed),
                },
                new FixedSlot[] {
                    new FixedSlot("ベトベトン", Gender.Female, Nature.Bashful),
                    new FixedSlot("サメハダー", Gender.Male, Nature.Calm),
                    new FixedSlot("キュウコン", Gender.Female, Nature.Quirky),
                    new FixedSlot("ポリゴン2", Gender.Genderless, Nature.Impish),
                    new FixedSlot("カイリキー", Gender.Male, Nature.Bold),
                    new FixedSlot("サーナイト", Gender.Female, Nature.Impish),
                },
                new FixedSlot[] {
                    new FixedSlot("ケッキング", Gender.Female, Nature.Naive),
                    new FixedSlot("ギャラドス", Gender.Male, Nature.Impish),
                    new FixedSlot("ボスゴドラ", Gender.Male, Nature.Lax),
                    new FixedSlot("トドゼルガ", Gender.Male, Nature.Relaxed),
                    new FixedSlot("ライボルト", Gender.Female, Nature.Quiet),
                    new FixedSlot("ノクタス", Gender.Female, Nature.Careful),
                },
            });
            public static readonly RentalTeamRank Normal = new RentalTeamRank("シングル普通", new FixedSlot[][]
            {
                new FixedSlot[]
                {
                    new FixedSlot("サニーゴ", Gender.Female, Nature.Sassy),
                    new FixedSlot("ポポッコ", Gender.Male, Nature.Impish),
                    new FixedSlot("ゴーリキー", Gender.Male, Nature.Brave),
                    new FixedSlot("プラスル", Gender.Male, Nature.Brave),
                    new FixedSlot("アゲハント", Gender.Male, Nature.Mild),
                    new FixedSlot("マッスグマ", Gender.Female, Nature.Modest),
                },
                new FixedSlot[]
                {
                    new FixedSlot("ユンゲラー", Gender.Male, Nature.Sassy),
                    new FixedSlot("クチート", Gender.Male, Nature.Naughty),
                    new FixedSlot("ドクケイル", Gender.Female, Nature.Careful),
                    new FixedSlot("オオスバメ", Gender.Female, Nature.Sassy),
                    new FixedSlot("ジュプトル", Gender.Male, Nature.Lax),
                    new FixedSlot("ヌマクロー", Gender.Male, Nature.Docile),
                },
                new FixedSlot[]
                {
                    new FixedSlot("マイナン", Gender.Female, Nature.Docile),
                    new FixedSlot("アリアドス", Gender.Male, Nature.Bold),
                    new FixedSlot("サイホーン", Gender.Female, Nature.Hardy),
                    new FixedSlot("デルビル", Gender.Male, Nature.Sassy),
                    new FixedSlot("ヘイガニ", Gender.Male, Nature.Adamant),
                    new FixedSlot("ソーナンス", Gender.Male, Nature.Naive),
                },
                new FixedSlot[]
                {
                    new FixedSlot("ユキワラシ", Gender.Female, Nature.Serious),
                    new FixedSlot("ベトベター", Gender.Female, Nature.Bold),
                    new FixedSlot("コダック", Gender.Female, Nature.Hasty),
                    new FixedSlot("コイル", Gender.Genderless, Nature.Rash),
                    new FixedSlot("ヒノアラシ", Gender.Male, Nature.Relaxed),
                    new FixedSlot("ヨーギラス", Gender.Male, Nature.Relaxed),
                },
                new FixedSlot[]
                {
                    new FixedSlot("チリーン", Gender.Female, Nature.Impish),
                    new FixedSlot("ドードー", Gender.Female, Nature.Jolly),
                    new FixedSlot("ケーシィ", Gender.Male, Nature.Naive),
                    new FixedSlot("キモリ", Gender.Male, Nature.Quirky),
                    new FixedSlot("ビリリダマ", Gender.Genderless, Nature.Naive),
                    new FixedSlot("ヒトデマン", Gender.Genderless, Nature.Timid),
                },
                new FixedSlot[]
                {
                    new FixedSlot("デリバード", Gender.Female, Nature.Careful),
                    new FixedSlot("モココ", Gender.Female, Nature.Sassy),
                    new FixedSlot("バネブー", Gender.Male, Nature.Adamant),
                    new FixedSlot("チルット", Gender.Female, Nature.Naughty),
                    new FixedSlot("メノクラゲ", Gender.Male, Nature.Brave),
                    new FixedSlot("ドンメル", Gender.Male, Nature.Lonely),
                },
                new FixedSlot[]
                {
                    new FixedSlot("ベイリーフ", Gender.Male, Nature.Bold),
                    new FixedSlot("マグマラシ", Gender.Male, Nature.Modest),
                    new FixedSlot("アリゲイツ", Gender.Male, Nature.Calm),
                    new FixedSlot("ヤミカラス", Gender.Female, Nature.Brave),
                    new FixedSlot("チャーレム", Gender.Female, Nature.Impish),
                    new FixedSlot("トドグラー", Gender.Female, Nature.Mild),
                },
                new FixedSlot[]
                {
                    new FixedSlot("ポワルン", Gender.Female, Nature.Bashful),
                    new FixedSlot("ネイティ", Gender.Female, Nature.Hasty),
                    new FixedSlot("ホエルコ", Gender.Female, Nature.Bold),
                    new FixedSlot("メタング", Gender.Genderless, Nature.Hasty),
                    new FixedSlot("コモルー", Gender.Male, Nature.Gentle),
                    new FixedSlot("グラエナ", Gender.Male, Nature.Docile),
                }
            });
            public static readonly RentalTeamRank Easy = new RentalTeamRank("シングル弱い", new FixedSlot[][]
            {
                new FixedSlot[] {
                    new FixedSlot("ピチュー", Gender.Male, Nature.Impish),
                    new FixedSlot("マクノシタ", Gender.Male, Nature.Serious),
                    new FixedSlot("ポチエナ", Gender.Male, Nature.Lonely),
                    new FixedSlot("ヨマワル", Gender.Female, Nature.Careful),
                    new FixedSlot("タネボー", Gender.Female, Nature.Modest),
                    new FixedSlot("ジグザグマ", Gender.Female, Nature.Hasty),
                },
                new FixedSlot[] {
                    new FixedSlot("マリル", Gender.Female, Nature.Rash),
                    new FixedSlot("ズバット", Gender.Male, Nature.Sassy),
                    new FixedSlot("ドジョッチ", Gender.Male, Nature.Mild),
                    new FixedSlot("マグマッグ", Gender.Female, Nature.Bashful),
                    new FixedSlot("レディバ", Gender.Male, Nature.Adamant),
                    new FixedSlot("エネコ", Gender.Female, Nature.Impish),
                },
                new FixedSlot[] {
                    new FixedSlot("ソーナノ", Gender.Male, Nature.Lax),
                    new FixedSlot("ウリムー", Gender.Female, Nature.Gentle),
                    new FixedSlot("オタチ", Gender.Male, Nature.Brave),
                    new FixedSlot("ホーホー", Gender.Female, Nature.Careful),
                    new FixedSlot("キルリア", Gender.Female, Nature.Sassy),
                    new FixedSlot("キャモメ", Gender.Female, Nature.Adamant),
                },
                new FixedSlot[] {
                    new FixedSlot("プリン", Gender.Female, Nature.Jolly),
                    new FixedSlot("アサナン", Gender.Male, Nature.Serious),
                    new FixedSlot("タマザラシ", Gender.Male, Nature.Timid),
                    new FixedSlot("メリープ", Gender.Female, Nature.Quiet),
                    new FixedSlot("イシツブテ", Gender.Male, Nature.Rash),
                    new FixedSlot("イトマル", Gender.Male, Nature.Hardy),
                },
                new FixedSlot[] {
                    new FixedSlot("ラクライ", Gender.Male, Nature.Modest),
                    new FixedSlot("ロコン", Gender.Female, Nature.Bold),
                    new FixedSlot("ナマケロ", Gender.Male, Nature.Relaxed),
                    new FixedSlot("クヌギダマ", Gender.Male, Nature.Brave),
                    new FixedSlot("カゲボウズ", Gender.Female, Nature.Bashful),
                    new FixedSlot("タッツー", Gender.Male, Nature.Quirky),
                },
                new FixedSlot[] {
                    new FixedSlot("キノココ", Gender.Male, Nature.Sassy),
                    new FixedSlot("ゴニョニョ", Gender.Female, Nature.Lonely),
                    new FixedSlot("ユキワラシ", Gender.Female, Nature.Quiet),
                    new FixedSlot("アメタマ", Gender.Female, Nature.Naive),
                    new FixedSlot("ピカチュウ", Gender.Male, Nature.Hardy),
                    new FixedSlot("サンド", Gender.Female, Nature.Docile),
                },
                new FixedSlot[] {
                    new FixedSlot("ヒノアラシ", Gender.Male, Nature.Calm),
                    new FixedSlot("ケーシィ", Gender.Male, Nature.Timid),
                    new FixedSlot("ドードー", Gender.Female, Nature.Hasty),
                    new FixedSlot("ワンリキー", Gender.Male, Nature.Brave),
                    new FixedSlot("ワニノコ", Gender.Male, Nature.Docile),
                    new FixedSlot("チルット", Gender.Female, Nature.Quiet),
                },
                new FixedSlot[] {
                    new FixedSlot("バネブー", Gender.Female, Nature.Naive),
                    new FixedSlot("ベトベター", Gender.Male, Nature.Hardy),
                    new FixedSlot("ツチニン", Gender.Male, Nature.Serious),
                    new FixedSlot("ココドラ", Gender.Female, Nature.Lonely),
                    new FixedSlot("ラブカス", Gender.Female, Nature.Lax),
                    new FixedSlot("デルビル", Gender.Male, Nature.Brave),
                },
            });
        }
        public static class DoubleBattle
        {
            public static readonly RentalTeamRank Ultimate = new RentalTeamRank("ダブル最強", new FixedSlot[][]
            {
                new FixedSlot[] {
                    new FixedSlot("ヘラクロス", Gender.Male, Nature.Careful),
                    new FixedSlot("オオスバメ", Gender.Female, Nature.Jolly),
                    new FixedSlot("ミロカロス", Gender.Female, Nature.Calm),
                    new FixedSlot("テッカニン", Gender.Male, Nature.Jolly),
                    new FixedSlot("マタドガス", Gender.Female, Nature.Adamant),
                    new FixedSlot("キュウコン", Gender.Female, Nature.Lax),
                },
                new FixedSlot[] {
                    new FixedSlot("メタグロス", Gender.Genderless, Nature.Adamant),
                    new FixedSlot("ノクタス", Gender.Male, Nature.Modest),
                    new FixedSlot("ツボツボ", Gender.Female, Nature.Calm),
                    new FixedSlot("レジスチル", Gender.Genderless, Nature.Careful),
                    new FixedSlot("ユレイドル", Gender.Female, Nature.Sassy),
                    new FixedSlot("アーマルド", Gender.Female, Nature.Adamant),
                },
                new FixedSlot[] {
                    new FixedSlot("ジュカイン", Gender.Male, Nature.Timid),
                    new FixedSlot("グランブル", Gender.Female, Nature.Adamant),
                    new FixedSlot("ラグラージ", Gender.Male, Nature.Sassy),
                    new FixedSlot("レジロック", Gender.Genderless, Nature.Brave),
                    new FixedSlot("エアームド", Gender.Female, Nature.Adamant),
                    new FixedSlot("バシャーモ", Gender.Male, Nature.Adamant),
                },
                new FixedSlot[] {
                    new FixedSlot("オオスバメ", Gender.Female, Nature.Jolly),
                    new FixedSlot("ハリテヤマ", Gender.Male, Nature.Careful),
                    new FixedSlot("ミルタンク", Gender.Female, Nature.Brave),
                    new FixedSlot("フーディン", Gender.Male, Nature.Modest),
                    new FixedSlot("ワタッコ", Gender.Female, Nature.Calm),
                    new FixedSlot("サメハダー", Gender.Male, Nature.Adamant),
                },
                new FixedSlot[] {
                    new FixedSlot("スターミー", Gender.Genderless, Nature.Modest),
                    new FixedSlot("レジアイス", Gender.Genderless, Nature.Modest),
                    new FixedSlot("ポリゴン2", Gender.Genderless, Nature.Hardy),
                    new FixedSlot("マルマイン", Gender.Genderless, Nature.Modest),
                    new FixedSlot("レアコイル", Gender.Genderless, Nature.Bashful),
                    new FixedSlot("ソルロック", Gender.Genderless, Nature.Quiet),
                },
                new FixedSlot[] {
                    new FixedSlot("メガニウム", Gender.Female, Nature.Quiet),
                    new FixedSlot("バクフーン", Gender.Male, Nature.Bashful),
                    new FixedSlot("オーダイル", Gender.Male, Nature.Adamant),
                    new FixedSlot("エーフィ", Gender.Female, Nature.Modest),
                    new FixedSlot("ブラッキー", Gender.Male, Nature.Bold),
                    new FixedSlot("リングマ", Gender.Male, Nature.Adamant),
                },
                new FixedSlot[] {
                    new FixedSlot("サイドン", Gender.Male, Nature.Hardy),
                    new FixedSlot("ライボルト", Gender.Female, Nature.Naive),
                    new FixedSlot("フライゴン", Gender.Male, Nature.Docile),
                    new FixedSlot("ギャラドス", Gender.Male, Nature.Brave),
                    new FixedSlot("マンタイン", Gender.Female, Nature.Calm),
                    new FixedSlot("ドードリオ", Gender.Male, Nature.Jolly),
                },
                new FixedSlot[] {
                    new FixedSlot("スイクン", Gender.Genderless, Nature.Careful),
                    new FixedSlot("ライコウ", Gender.Genderless, Nature.Naive),
                    new FixedSlot("エンテイ", Gender.Genderless, Nature.Modest),
                    new FixedSlot("ボスゴドラ", Gender.Male, Nature.Brave),
                    new FixedSlot("ドンファン", Gender.Female, Nature.Hardy),
                    new FixedSlot("ボーマンダ", Gender.Male, Nature.Jolly),
                },
            });
            public static readonly RentalTeamRank Hard = new RentalTeamRank("ダブル強い", new FixedSlot[][]
            {
                new FixedSlot[] {
                    new FixedSlot("ムウマ", Gender.Female, Nature.Jolly),
                    new FixedSlot("ソーナンス", Gender.Female, Nature.Hardy),
                    new FixedSlot("ヌオー", Gender.Female, Nature.Sassy),
                    new FixedSlot("オオスバメ", Gender.Male, Nature.Jolly),
                    new FixedSlot("ゴルバット", Gender.Male, Nature.Careful),
                    new FixedSlot("サンドパン", Gender.Male, Nature.Adamant),
                },
                new FixedSlot[] {
                    new FixedSlot("オオタチ", Gender.Male, Nature.Jolly),
                    new FixedSlot("チャーレム", Gender.Male, Nature.Timid),
                    new FixedSlot("ヌマクロー", Gender.Male, Nature.Sassy),
                    new FixedSlot("ネイティオ", Gender.Male, Nature.Modest),
                    new FixedSlot("ザングース", Gender.Male, Nature.Adamant),
                    new FixedSlot("ハブネーク", Gender.Male, Nature.Bashful),
                },
                new FixedSlot[] {
                    new FixedSlot("ユンゲラー", Gender.Male, Nature.Hasty),
                    new FixedSlot("マッスグマ", Gender.Female, Nature.Impish),
                    new FixedSlot("サニーゴ", Gender.Female, Nature.Bashful),
                    new FixedSlot("レディアン", Gender.Female, Nature.Quirky),
                    new FixedSlot("ドードリオ", Gender.Male, Nature.Quiet),
                    new FixedSlot("イノムー", Gender.Male, Nature.Rash),
                },
                new FixedSlot[] {
                    new FixedSlot("グラエナ", Gender.Male, Nature.Modest),
                    new FixedSlot("アメモース", Gender.Female, Nature.Serious),
                    new FixedSlot("カポエラー", Gender.Male, Nature.Careful),
                    new FixedSlot("クチート", Gender.Female, Nature.Careful),
                    new FixedSlot("グランブル", Gender.Male, Nature.Brave),
                    new FixedSlot("オドシシ", Gender.Female, Nature.Modest),
                },
                new FixedSlot[] {
                    new FixedSlot("アリゲイツ", Gender.Male, Nature.Mild),
                    new FixedSlot("ヤミカラス", Gender.Female, Nature.Rash),
                    new FixedSlot("ピカチュウ", Gender.Female, Nature.Relaxed),
                    new FixedSlot("ベトベトン", Gender.Male, Nature.Lonely),
                    new FixedSlot("キレイハナ", Gender.Female, Nature.Docile),
                    new FixedSlot("ゴーリキー", Gender.Male, Nature.Adamant),
                },
                new FixedSlot[] {
                    new FixedSlot("ワカシャモ", Gender.Male, Nature.Adamant),
                    new FixedSlot("トロピウス", Gender.Female, Nature.Quiet),
                    new FixedSlot("カクレオン", Gender.Female, Nature.Lonely),
                    new FixedSlot("ナマズン", Gender.Male, Nature.Mild),
                    new FixedSlot("ニューラ", Gender.Female, Nature.Modest),
                    new FixedSlot("マルノーム", Gender.Male, Nature.Serious),
                },
                new FixedSlot[] {
                    new FixedSlot("マグマラシ", Gender.Female, Nature.Serious),
                    new FixedSlot("トドグラー", Gender.Male, Nature.Gentle),
                    new FixedSlot("キリンリキ", Gender.Male, Nature.Modest),
                    new FixedSlot("ミルタンク", Gender.Female, Nature.Adamant),
                    new FixedSlot("ゴローニャ", Gender.Male, Nature.Adamant),
                    new FixedSlot("ベイリーフ", Gender.Male, Nature.Hardy),
                },
                new FixedSlot[] {
                    new FixedSlot("ライチュウ", Gender.Male, Nature.Timid),
                    new FixedSlot("ゴルダック", Gender.Male, Nature.Modest),
                    new FixedSlot("ドンファン", Gender.Female, Nature.Serious),
                    new FixedSlot("カイロス", Gender.Male, Nature.Brave),
                    new FixedSlot("ジュプトル", Gender.Female, Nature.Timid),
                    new FixedSlot("プクリン", Gender.Female, Nature.Careful),
                },
            });
            public static readonly RentalTeamRank Normal = new RentalTeamRank("ダブル普通", new FixedSlot[][]
            {
                new FixedSlot[] {
                    new FixedSlot("ピカチュウ", Gender.Female, Nature.Hardy),
                    new FixedSlot("コダック", Gender.Male, Nature.Modest),
                    new FixedSlot("キモリ", Gender.Female, Nature.Docile),
                    new FixedSlot("ヨーギラス", Gender.Male, Nature.Lonely),
                    new FixedSlot("ミズゴロウ", Gender.Female, Nature.Lax),
                    new FixedSlot("アチャモ", Gender.Male, Nature.Naive),
                },
                new FixedSlot[] {
                    new FixedSlot("ナゾノクサ", Gender.Female, Nature.Mild),
                    new FixedSlot("ヒメグマ", Gender.Male, Nature.Rash),
                    new FixedSlot("ゴマゾウ", Gender.Female, Nature.Brave),
                    new FixedSlot("チコリータ", Gender.Male, Nature.Quirky),
                    new FixedSlot("ワニノコ", Gender.Female, Nature.Naughty),
                    new FixedSlot("ヒノアラシ", Gender.Male, Nature.Modest),
                },
                new FixedSlot[] {
                    new FixedSlot("タツベイ", Gender.Male, Nature.Hasty),
                    new FixedSlot("チョンチー", Gender.Male, Nature.Modest),
                    new FixedSlot("キバニア", Gender.Female, Nature.Gentle),
                    new FixedSlot("ベトベター", Gender.Male, Nature.Jolly),
                    new FixedSlot("ケーシィ", Gender.Female, Nature.Timid),
                    new FixedSlot("デルビル", Gender.Male, Nature.Hardy),
                },
                new FixedSlot[] {
                    new FixedSlot("オタチ", Gender.Female, Nature.Impish),
                    new FixedSlot("ビブラーバ", Gender.Male, Nature.Calm),
                    new FixedSlot("サイホーン", Gender.Male, Nature.Lonely),
                    new FixedSlot("デリバード", Gender.Female, Nature.Relaxed),
                    new FixedSlot("ビリリダマ", Gender.Genderless, Nature.Timid),
                    new FixedSlot("ネイティ", Gender.Male, Nature.Modest),
                },
                new FixedSlot[] {
                    new FixedSlot("ゴローン", Gender.Female, Nature.Brave),
                    new FixedSlot("チルット", Gender.Female, Nature.Calm),
                    new FixedSlot("プラスル", Gender.Male, Nature.Docile),
                    new FixedSlot("マイナン", Gender.Female, Nature.Hasty),
                    new FixedSlot("ホエルコ", Gender.Female, Nature.Gentle),
                    new FixedSlot("サナギラス", Gender.Male, Nature.Serious),
                },
                new FixedSlot[] {
                    new FixedSlot("レディアン", Gender.Female, Nature.Gentle),
                    new FixedSlot("コータス", Gender.Male, Nature.Gentle),
                    new FixedSlot("チリーン", Gender.Female, Nature.Gentle),
                    new FixedSlot("ドーブル", Gender.Male, Nature.Gentle),
                    new FixedSlot("サンドパン", Gender.Male, Nature.Gentle),
                    new FixedSlot("サニーゴ", Gender.Female, Nature.Gentle),
                },
                new FixedSlot[] {
                    new FixedSlot("ウパー", Gender.Male, Nature.Naughty),
                    new FixedSlot("グライガー", Gender.Male, Nature.Hasty),
                    new FixedSlot("サボネア", Gender.Male, Nature.Modest),
                    new FixedSlot("モココ", Gender.Female, Nature.Quirky),
                    new FixedSlot("ウリムー", Gender.Female, Nature.Docile),
                    new FixedSlot("ドンメル", Gender.Male, Nature.Mild),
                },
                new FixedSlot[] {
                    new FixedSlot("ノズパス", Gender.Male, Nature.Sassy),
                    new FixedSlot("パールル", Gender.Male, Nature.Careful),
                    new FixedSlot("ゴーリキー", Gender.Female, Nature.Impish),
                    new FixedSlot("アリアドス", Gender.Male, Nature.Calm),
                    new FixedSlot("チャーレム", Gender.Female, Nature.Serious),
                    new FixedSlot("エネコロロ", Gender.Male, Nature.Jolly),
                },
            });
            public static readonly RentalTeamRank Easy = new RentalTeamRank("ダブル弱い", new FixedSlot[][]
            {
                new FixedSlot[] {
                    new FixedSlot("ポチエナ", Gender.Male, Nature.Naughty),
                    new FixedSlot("クヌギダマ", Gender.Female, Nature.Quirky),
                    new FixedSlot("ウリムー", Gender.Female, Nature.Mild),
                    new FixedSlot("サンド", Gender.Male, Nature.Bashful),
                    new FixedSlot("ナマケロ", Gender.Female, Nature.Impish),
                    new FixedSlot("メリープ", Gender.Female, Nature.Timid),
                },
                new FixedSlot[] {
                    new FixedSlot("ラルトス", Gender.Female, Nature.Calm),
                    new FixedSlot("キノココ", Gender.Male, Nature.Sassy),
                    new FixedSlot("オタチ", Gender.Male, Nature.Hasty),
                    new FixedSlot("ロコン", Gender.Female, Nature.Mild),
                    new FixedSlot("ヨマワル", Gender.Female, Nature.Sassy),
                    new FixedSlot("イトマル", Gender.Male, Nature.Relaxed),
                },
                new FixedSlot[] {
                    new FixedSlot("ゴニョニョ", Gender.Male, Nature.Gentle),
                    new FixedSlot("カゲボウズ", Gender.Female, Nature.Adamant),
                    new FixedSlot("マグマッグ", Gender.Male, Nature.Quiet),
                    new FixedSlot("ツチニン", Gender.Female, Nature.Relaxed),
                    new FixedSlot("ドジョッチ", Gender.Male, Nature.Brave),
                    new FixedSlot("アサナン", Gender.Female, Nature.Naughty),
                },
                new FixedSlot[] {
                    new FixedSlot("ピチュー", Gender.Male, Nature.Bold),
                    new FixedSlot("ヤジロン", Gender.Genderless, Nature.Quiet),
                    new FixedSlot("ハスボー", Gender.Female, Nature.Calm),
                    new FixedSlot("ブルー", Gender.Male, Nature.Lonely),
                    new FixedSlot("レディバ", Gender.Male, Nature.Docile),
                    new FixedSlot("マリル", Gender.Male, Nature.Mild),
                },
                new FixedSlot[] {
                    new FixedSlot("ヒマナッツ", Gender.Female, Nature.Timid),
                    new FixedSlot("トゲピー", Gender.Male, Nature.Naive),
                    new FixedSlot("ケムッソ", Gender.Male, Nature.Serious),
                    new FixedSlot("ココドラ", Gender.Male, Nature.Careful),
                    new FixedSlot("ヌケニン", Gender.Genderless, Nature.Naive),
                    new FixedSlot("マクノシタ", Gender.Female, Nature.Hardy),
                },
                new FixedSlot[] {
                    new FixedSlot("エネコ", Gender.Female, Nature.Impish),
                    new FixedSlot("プリン", Gender.Female, Nature.Adamant),
                    new FixedSlot("キルリア", Gender.Female, Nature.Bashful),
                    new FixedSlot("アメタマ", Gender.Female, Nature.Modest),
                    new FixedSlot("ナックラー", Gender.Female, Nature.Careful),
                    new FixedSlot("ジグザグマ", Gender.Male, Nature.Naive),
                },
                new FixedSlot[] {
                    new FixedSlot("ダンバル", Gender.Genderless, Nature.Lonely),
                    new FixedSlot("ワンリキー", Gender.Male, Nature.Careful),
                    new FixedSlot("ラクライ", Gender.Male, Nature.Rash),
                    new FixedSlot("タッツー", Gender.Male, Nature.Calm),
                    new FixedSlot("ユキワラシ", Gender.Male, Nature.Bold),
                    new FixedSlot("ドンメル", Gender.Male, Nature.Naughty),
                },
                new FixedSlot[] {
                    new FixedSlot("パールル", Gender.Female, Nature.Serious),
                    new FixedSlot("コイル", Gender.Genderless, Nature.Lax),
                    new FixedSlot("タネボー", Gender.Female, Nature.Jolly),
                    new FixedSlot("ププリン", Gender.Female, Nature.Rash),
                    new FixedSlot("ドガース", Gender.Male, Nature.Gentle),
                    new FixedSlot("イシツブテ", Gender.Female, Nature.Adamant),
                },
            });
        }
    }
    public class RentalTeamRank : IGeneratable<RentalBattleResult>, ILcgUser
    {
        public string RuleName { get; }

        private readonly FixedSlot[][] teams;
        private static readonly string[] playerNames = { "レオ", "ユータ", "タツキ" };
        public RentalBattleResult Generate(uint seed)
        {
            var head = seed;

            var enemyTeamIndex = seed.GetRand() & 0x7;
            uint playerTeamIndex;
            do { playerTeamIndex = seed.GetRand() & 0x7; } while (enemyTeamIndex == playerTeamIndex);

            var enemyTSV = seed.GetRand() ^ seed.GetRand();
            var enemyTeam = teams[(int)enemyTeamIndex].Select(_ => _.Generate(ref seed, enemyTSV)).ToArray();

            var playerNameIndex = seed.GetRand(3);
            var playerName = playerNames[playerNameIndex];

            var playerTSV = seed.GetRand() ^ seed.GetRand();
            var playerTeam = teams[(int)playerTeamIndex].Select(_ => _.Generate(ref seed, playerTSV)).ToArray();

            return new RentalBattleResult(head, seed, playerName, playerTeam, enemyTeam);
        }

        public byte GenerateCode(uint seed, out uint finSeed)
        {
            var enemyTeamIndex = seed.GetRand() & 0x7;
            uint playerTeamIndex;
            do { playerTeamIndex = seed.GetRand() & 0x7; } while (enemyTeamIndex == playerTeamIndex);

            var enemyTSV = seed.GetRand() ^ seed.GetRand();
            foreach (var poke in teams[(int)enemyTeamIndex])
                poke.Generate(ref seed, enemyTSV);

            var playerNameIndex = seed.GetRand(3);

            var playerTSV = seed.GetRand() ^ seed.GetRand();
            foreach (var poke in teams[playerTeamIndex])
                poke.Generate(ref seed, playerTSV);

            finSeed = seed;

            return (byte)(playerNameIndex * 8 + playerTeamIndex);
        }

        public uint AdvanceSeed(uint seed)
        {
            seed.Used(this);
            return seed;
        }

        public void Use(ref uint seed)
        {
            var enemyTeamIndex = seed.GetRand() & 0x7;
            uint playerTeamIndex;
            do { playerTeamIndex = seed.GetRand() & 0x7; } while (enemyTeamIndex == playerTeamIndex);

            var enemyTSV = seed.GetRand() ^ seed.GetRand();
            foreach (var poke in teams[enemyTeamIndex])
                seed.Used(poke, enemyTSV);

            seed.Advance(); // PlayerName

            var playerTSV = seed.GetRand() ^ seed.GetRand();
            foreach (var poke in teams[playerTeamIndex])
                seed.Used(poke, playerTSV);
        }

        internal RentalTeamRank(string label, FixedSlot[][] p)
        {
            RuleName = label;
            teams = p;
        }
    }
    public class RentalBattleResult
    {
        public uint HeadSeed { get; }
        public uint TailSeed { get; }
        public string PlayerName { get; }
        public IReadOnlyList<GCIndividual> PlayerTeam { get; }
        public IReadOnlyList<GCIndividual> EnemyTeam { get; }

        public RentalBattleResult(uint head, uint tail, string pName, GCIndividual[] p, GCIndividual[] e)
        {
            HeadSeed = head;
            TailSeed = tail;
            PlayerName = pName;
            PlayerTeam = p;
            EnemyTeam = e;
        }
    }
}
