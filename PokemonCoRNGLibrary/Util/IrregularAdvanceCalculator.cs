using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;

namespace PokemonCoRNGLibrary
{
    public static class IrregularAdvanceCalculator
    {

        public static IEnumerable<((int frame, uint seed) blink, (int frame, uint seed) bubble)> CalcBlinkAndVibravaFrame(uint currentSeed, uint targetSeed, uint minInterval, int minBlinkFrame, int maxBlinkFrame, int minFrame, int maxFrame, int blinkCool = 4)
        {
            foreach (var (seed, interval, frame, _) in currentSeed.EnumerateBlinkingSeed(blinkCool)
                .SkipWhile(_ => minBlinkFrame <= _.frame)
                .TakeWhile(_ => _.frame <= maxBlinkFrame)
                .Where(_ => minInterval <= _.interval))
                foreach (var res in seed.EnumerateSeedAtCipherLabB1F().WithIndex()
                    .Skip(minFrame).Take(maxFrame - minFrame + 1)
                    .Where(_ => _.element == targetSeed).Select(_ => ((frame, seed), (_.index, _.element))))
                    yield return res;
        }

        public static IEnumerable<((int frame, uint seed) blink, (int frame, uint seed) bubble)> CalcBlinkAndBubbleFrame(uint currentSeed, uint targetSeed, uint minInterval, int minBlinkFrame, int maxBlinkFrame, int minFrame, int maxFrame, int blinkCool = 4)
        {
            foreach (var (seed, interval, frame, _) in currentSeed.EnumerateBlinkingSeed(blinkCool)
                .SkipWhile(_ => minBlinkFrame <= _.frame)
                .TakeWhile(_ => _.frame <= maxBlinkFrame)
                .Where(_ => minInterval <= _.interval)) 
                    foreach (var res in seed.EnumerateSeedAtCipherLabB2F().WithIndex()
                        .Skip(minFrame).Take(maxFrame - minFrame + 1)
                        .Where(_ => _.element == targetSeed).Select(_ => ((frame, seed), (_.index, _.element))))
                            yield return res;
        }
        public static IEnumerable<((int frame, uint seed) blink, (int frame, uint seed) stand)> CalcBlinkAndStandFrame(uint currentSeed, uint targetSeed, uint minInterval, int minBlinkFrame, int maxBlinkFrame, int minFrame, int maxFrame, int blinkCool = 4)
        {
            foreach (var (seed, interval, frame, _) in currentSeed.EnumerateBlinkingSeed(blinkCool)
                .SkipWhile(_ => minBlinkFrame <= _.frame)
                .TakeWhile(_ => _.frame <= maxBlinkFrame)
                .Where(_ => minInterval <= _.interval))
                foreach (var res in seed.EnumerateSeedAtOutskirtStand().WithIndex()
                    .Skip(minFrame).Take(maxFrame - minFrame + 1)
                    .Where(_ => _.element == targetSeed).Select(_ => ((frame, seed), (_.index, _.element))))
                    yield return res;
        }
        public static IEnumerable<((int frame, uint seed) blink, (int frame, uint seed) stand)> CalcBlinkAndSmokeFrame(uint currentSeed, uint targetSeed, uint minInterval, int minBlinkFrame, int maxBlinkFrame, int minFrame, int maxFrame, int blinkCool = 4)
        {
            foreach (var (seed, interval, frame, _) in currentSeed.EnumerateBlinkingSeed(blinkCool)
                .SkipWhile(_ => minBlinkFrame <= _.frame)
                .TakeWhile(_ => _.frame <= maxBlinkFrame)
                .Where(_ => minInterval <= _.interval))
                foreach (var res in seed.EnumerateSeedAtPyriteCave().WithIndex()
                    .Skip(minFrame).Take(maxFrame - minFrame + 1)
                    .Where(_ => _.element == targetSeed).Select(_ => ((frame, seed), (_.index, _.element))))
                    yield return res;
        }
    }
}
