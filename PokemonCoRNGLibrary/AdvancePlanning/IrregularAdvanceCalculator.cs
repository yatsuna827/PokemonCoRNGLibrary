﻿using System;
using System.Collections.Generic;
using System.Linq;
using PokemonCoRNGLibrary.AdvanceSource;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary
{
    public static class IrregularAdvanceCalculator
    {
        /// <summary>
        /// minBlinkFrame: 瞬きを見続ける時間の最小値,
        /// maxBlinkFrame: 瞬きを見続ける時間の最大値,
        /// maxSnatchlist: スナッチリスト開閉回数の最大値
        /// </summary>
        /// <param name="currentSeed"></param>
        /// <param name="targetSeed"></param>
        /// <param name="minInterval"></param>
        /// <param name="minBlinkFrame"></param>
        /// <param name="maxBlinkFrame"></param>
        /// <param name="maxSnatchList"></param>
        /// <param name="blinkCool"></param>
        /// <returns></returns>
        public static IEnumerable<((int frame, uint seed)blink, int snatchList)> CalcBlinkAndSnatchList
            (uint currentSeed, uint targetSeed, uint minInterval, int minBlinkFrame, int maxBlinkFrame, int maxSnatchList, int blinkCool = 4)
        {
            var targetIndex = targetSeed.GetIndex(currentSeed);
            foreach (var (seed, interval, frame, lcgIndex) in currentSeed.EnumerateBlinkingSeed(blinkCool)
                // ここはなに…？
                .SkipWhile(_ => minBlinkFrame <= _.frame)
                .TakeWhile(_ => _.frame <= maxBlinkFrame)
                .Where(_ => minInterval <= _.interval))
            {
                foreach (var res in seed.EnumerateSeed(new OpenSnatchList()).WithIndex()
                    .Take(maxSnatchList)
                    .Where(_ => _.Element == targetSeed && _.Index <= maxSnatchList).Select(_ => ((frame, seed), _.Index)))
                    yield return res;
            }
        }

        [Obsolete]
        public static IEnumerable<((int frame, uint seed) blink, (int frame, uint seed) bubble)> CalcBlinkAndVibravaFrame(uint currentSeed, uint targetSeed, uint minInterval, int minBlinkFrame, int maxBlinkFrame, int minFrame, int maxFrame, int blinkCool = 4)
        {
            foreach (var (seed, interval, frame, _) in currentSeed.EnumerateBlinkingSeed(blinkCool)
                .SkipWhile(_ => minBlinkFrame <= _.frame)
                .TakeWhile(_ => _.frame <= maxBlinkFrame)
                .Where(_ => minInterval <= _.interval))
                foreach (var res in seed.EnumerateSeed(new CipherLabB2F()).WithIndex()
                    .Skip(minFrame).Take(maxFrame - minFrame + 1)
                    .Where(_ => _.Element == targetSeed).Select(_ => ((frame, seed), (_.Index, _.Element))))
                    yield return res;
        }
        [Obsolete]
        public static IEnumerable<((int frame, uint seed) blink, (int frame, uint seed) bubble)> CalcBlinkAndBubbleFrame(uint currentSeed, uint targetSeed, uint minInterval, int minBlinkFrame, int maxBlinkFrame, int minFrame, int maxFrame, int blinkCool = 4)
        {
            foreach (var (seed, interval, frame, _) in currentSeed.EnumerateBlinkingSeed(blinkCool)
                .SkipWhile(_ => minBlinkFrame <= _.frame)
                .TakeWhile(_ => _.frame <= maxBlinkFrame)
                .Where(_ => minInterval <= _.interval)) 
                    foreach (var res in seed.EnumerateSeed(new CipherLabB3F()).WithIndex()
                        .Skip(minFrame).Take(maxFrame - minFrame + 1)
                        .Where(_ => _.Element == targetSeed).Select(_ => ((frame, seed), (_.Index, _.Element))))
                            yield return res;
        }
        [Obsolete]
        public static IEnumerable<((int frame, uint seed) blink, (int frame, uint seed) stand)> CalcBlinkAndStandFrame(uint currentSeed, uint targetSeed, uint minInterval, int minBlinkFrame, int maxBlinkFrame, int minFrame, int maxFrame, int blinkCool = 4)
        {
            foreach (var (seed, interval, frame, _) in currentSeed.EnumerateBlinkingSeed(blinkCool)
                .SkipWhile(_ => minBlinkFrame <= _.frame)
                .TakeWhile(_ => _.frame <= maxBlinkFrame)
                .Where(_ => minInterval <= _.interval))
                foreach (var res in seed.EnumerateSeed(new OutskirtStand()).WithIndex()
                    .Skip(minFrame).Take(maxFrame - minFrame + 1)
                    .Where(_ => _.Element == targetSeed).Select(_ => ((frame, seed), (_.Index, _.Element))))
                    yield return res;
        }
        [Obsolete]
        public static IEnumerable<((int frame, uint seed) blink, (int frame, uint seed) stand)> CalcBlinkAndSmokeFrame(uint currentSeed, uint targetSeed, uint minInterval, int minBlinkFrame, int maxBlinkFrame, int minFrame, int maxFrame, int blinkCool = 4)
        {
            foreach (var (seed, interval, frame, _) in currentSeed.EnumerateBlinkingSeed(blinkCool)
                .SkipWhile(_ => minBlinkFrame <= _.frame)
                .TakeWhile(_ => _.frame <= maxBlinkFrame)
                .Where(_ => minInterval <= _.interval))
                foreach (var res in seed.EnumerateSeed(new PyriteCave()).WithIndex()
                    .Skip(minFrame).Take(maxFrame - minFrame + 1)
                    .Where(_ => _.Element == targetSeed).Select(_ => ((frame, seed), (_.Index, _.Element))))
                    yield return res;
        }
    
    }
}
