using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonCoRNGLibrary
{
    public static class IrregularAdvanceCalculator
    {
        public static IEnumerable<((int frame, uint seed) blink, (int frame, uint seed) bubble)> CalcBlinkAndBubbleFrame(uint currentSeed, uint targetSeed, uint minInterval, int minBlinkFrame, int maxBlinkFrame, int minFrame, int maxFrame, int blinkCool = 4)
        {
            foreach (var (seed, interval, frame, _) in currentSeed.EnumerateBlinkingSeed(blinkCool)
                .SkipWhile(_ => minBlinkFrame <= _.frame)
                .TakeWhile(_ => _.frame <= maxBlinkFrame)
                .Where(_ => minInterval <= _.interval)) 
                    foreach (var res in seed.EnumerateSeedAtCipherLabWithIndex()
                        .Skip(minFrame).Take(maxFrame - minFrame + 1)
                        .Where(_ => _.seed == targetSeed).Select(_ => ((frame, seed), (_.index, _.seed))))
                            yield return res;
        }
        public static IEnumerable<((int frame, uint seed) blink, (int frame, uint seed) stand)> CalcBlinkAndStandFrame(uint currentSeed, uint targetSeed, uint minInterval, int minBlinkFrame, int maxBlinkFrame, int minFrame, int maxFrame, int blinkCool = 4)
        {
            foreach (var (seed, interval, frame, _) in currentSeed.EnumerateBlinkingSeed(blinkCool)
                .SkipWhile(_ => minBlinkFrame <= _.frame)
                .TakeWhile(_ => _.frame <= maxBlinkFrame)
                .Where(_ => minInterval <= _.interval))
                foreach (var res in seed.EnumerateSeedAtOutskirtStandWithIndex()
                    .Skip(minFrame).Take(maxFrame - minFrame + 1)
                    .Where(_ => _.seed == targetSeed).Select(_ => ((frame, seed), (_.index, _.seed))))
                    yield return res;
        }
    }
}
