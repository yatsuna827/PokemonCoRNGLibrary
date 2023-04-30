using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.GCLCG;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary
{
    public class CoStarterGenerator : IGeneratable<CoStarterResult>
    {
        private static readonly GCSlot ESPEON = new GCSlot("エーフィ", 25, Gender.Male);
        private static readonly GCSlot UMBREON = new GCSlot("ブラッキー", 26, Gender.Male);

        public CoStarterResult Generate(uint seed)
        {
            var head = seed;
            seed.Advance1000();
            uint TID = seed.GetRand();
            uint SID = seed.GetRand();
            var u = UMBREON.Generate(ref seed, TID ^ SID);
            var e = ESPEON.Generate(ref seed, TID ^ SID);

            return new CoStarterResult(head, TID, SID, e, u);
        }

        // なんでもワンライナーにすればいいってもんじゃない.
        public static IEnumerable<uint> CalcBack(uint tid, uint sid)
            => LOWER[(sid - (0x43FDU * tid)) & 0xFFFF]
                .Select(_ => ((tid << 16) | _)
                .PrevSeed(1001))
                .Where(LCGExtensions.IsAccessibleInNameScreen);

        public static IEnumerable<uint> CalcBack(uint tid, uint pid, ShinyType shinyType)
        {
            // 死ね.
            if (shinyType == ShinyType.NotShiny) yield break;

            var psv = (pid >> 16) ^ (pid & 0xFFFF);

            // 菱形になるseedを返す.
            if ((shinyType & ShinyType.Square) != 0)
                foreach (var seed in CalcBack(tid, tid ^ psv).Where(LCGExtensions.IsAccessibleInNameScreen)) 
                    yield return seed;
            
            // 星型になるseedを返す.
            if((shinyType & ShinyType.Star) != 0)
                for(uint bit =1; bit < 8; bit++)
                    foreach (var seed in CalcBack(tid, tid ^ psv ^ bit).Where(LCGExtensions.IsAccessibleInNameScreen))
                        yield return seed;
        }

        public static IEnumerable<uint> CalcBackUmbreon(uint h, uint a, uint b, uint c, uint d, uint s, bool deduplication = false)
        {
            // ブラッキーは特に再計算は無いので、ID生成分と1000消費分を戻して返す
            return SeedFinder.FindGeneratingSeed(h, a, b, c, d, s, true)
                .Select(_ => _.PrevSeed(1000))
                .Where(LCGExtensions.IsAccessibleInNameScreen);
        }

        public static IEnumerable<uint> CalcBackEspeon(uint h, uint a, uint b, uint c, uint d, uint s, bool deduplication = false)
        {
            foreach (var genSeed in SeedFinder.FindGeneratingSeed(h, a, b, c, d, s, false))
            {
                var seed = genSeed;

                // ブラッキーのPIDを確認
                var lid = seed.BackRand();
                var psv = (lid ^ seed.BackRand()) >> 3;

                // ブラッキーが♀になるPIDは出現不可
                if ((lid & 0xFF) < 0x1F)
                    continue;

                // 該当したseedを返す処理
                {
                    // 個体値+特性+dummyPIDで5消費分戻す
                    var _seed = seed.PrevSeed(5);

                    // tsv生成して色回避のチェック
                    var tsv = (_seed.BackRand() ^ _seed.BackRand()) >> 3;

                    // 色回避が発生しないことの確認
                    if (tsv != psv)
                    {
                        // 謎の1000消費分を戻してから返す
                        _seed.Back(1000);

                        // 名前入力画面の不定消費に巻き込まれないかチェック
                        if (_seed.IsAccessibleInNameScreen())
                        {
                            yield return _seed;

                            // 1つだけ返せばいい場合は中断する
                            if (deduplication) yield break;
                        }
                    }
                }

                uint tsvCondition = 0x10000;
                // pidを持つような生成開始seedを探索
                do
                {
                    // ♂のPIDに当たるまでさかのぼり続ける
                    // ここで生成されるPIDは再計算対象でないといけない
                    lid = seed.BackRand();
                    var tempPSV = (lid ^ seed.BackRand()) >> 3;

                    // ♂になるPIDなら探索終了判定に移る
                    if ((lid & 0xFF) >= 0x1F)
                    {
                        // 色回避を考慮するモードで、なおかつ対象のTSVで色回避が発生しない
                        // …なら、探索を打ち切る
                        if (tsvCondition != 0x10000 && tsvCondition != tempPSV)
                            break;

                        // 本命のPIDとPSVが同じ場合、色回避が発生すると本命まで再計算されてしまうので
                        // 色回避の考慮無しに探索を打ち切る
                        if (tempPSV == psv)
                            break;

                        // 特定のTSVの場合に限って探索を続ける
                        tsvCondition = tempPSV;
                    } 

                    // 該当したseedを返す処理
                    {
                        // 個体値+特性+dummyPIDで5消費分戻す
                        var _seed = seed.PrevSeed(5);

                        // tsv生成して色回避のチェック
                        var tsv = (_seed.BackRand() ^ _seed.BackRand()) >> 3;

                        if(tsvCondition == 0x10000)
                        {
                            // 本命のPIDに対して色回避を発生させてしまうTSVは不適格
                            if (tsv == psv)
                                continue;
                        } 
                        else
                        {
                            // ブラッキーの色回避発生時に出現する個体の場合、色回避が発生しないとダメ
                            if (tsvCondition != tsv)
                                continue;
                        }


                        // 謎の1000消費分を戻してから返す
                        _seed.Back(1000);

                        // 名前入力画面の不定消費に巻き込まれないかチェック
                        if (_seed.IsAccessibleInNameScreen())
                        {
                            yield return _seed;

                            // 1つだけ返せばいい場合は中断する
                            if (deduplication) yield break;
                        }
                    }

                } while (true);
            }

        }

        private static readonly uint[][] LOWER;
        static CoStarterGenerator()
        {
            var lower = Enumerable.Range(0, 0x10000).Select(_ => new List<uint>()).ToArray();
            for (uint y = 0; y < 0x10000; y++) lower[(y.NextSeed() >> 16)].Add(y);

            LOWER = lower.Select(_ => _.ToArray()).ToArray();
        }

    }

    public readonly struct CoStarterResult
    {
        public uint HeadSeed { get; }
        public uint TID { get; }
        public uint SID { get; }
        public GCIndividual Espeon { get; }
        public GCIndividual Umbreon { get; }

        public CoStarterResult(uint head, uint tid, uint sid, GCIndividual espeon, GCIndividual umbreon)
        {
            HeadSeed = head;
            TID = tid;
            SID = sid;
            Espeon = espeon;
            Umbreon = umbreon;
        }
    }
}
