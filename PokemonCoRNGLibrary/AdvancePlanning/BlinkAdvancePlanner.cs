using System.Collections.Generic;
using System.Linq;
using PokemonCoRNGLibrary.AdvanceSource;
using PokemonPRNG.LCG32;

namespace PokemonCoRNGLibrary.AdvancePlanning
{
    public class BlinkAdvancePlanner
    {
        private readonly BlinkObjectEnumeratorHanlder _blinkHandler;
        private readonly ISeedEnumeratorHandler _handler;

        public BlinkAdvancePlanner(BlinkObject blinkObject, ISeedEnumeratorHandler seedEnumeratorHandler)
        {
            _blinkHandler = new BlinkObjectEnumeratorHanlder(blinkObject);
            _handler = seedEnumeratorHandler;
        }

        public IEnumerable<((int Frame, uint Seed) Blink, (int Frame, uint Seed) SecondAdvance)> CalculatePlanning(
            uint current,
            uint target,
            uint minInterval,
            int minBlinkFrames,
            int maxBlinkFrames,
            int minFrames,
            int maxFrames
        )
            => current.EnumerateActionSequence(_blinkHandler)
                .SkipWhile(_ => minBlinkFrames <= _.Frame)
                .TakeWhile(_ => _.Frame <= maxBlinkFrames)
                .Where(_ => minInterval <= _.Interval)
                .SelectMany((_) =>
                    _.Seed.EnumerateSeed(_handler).WithIndex()
                        .Skip(minFrames)
                        .Take(maxFrames - minFrames + 1)
                        .Where(__ => __.element == target)
                        .Select(__ => ((_.Frame, _.Seed), __))
                );
    }
}
