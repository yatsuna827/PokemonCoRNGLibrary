using FsCheck;
using FsCheck.Xunit;
using PokemonCoRNGLibrary.AdvanceSource;

namespace Tests;

public class BlinkSeedSearchProperties
{
    [Property(MaxTest = 1000, EndSize = 128)]
    public void MatchesDirectSimulation(
        uint seed,
        uint startIndex,
        PositiveInt rangeLength,
        int[] observations,
        NonNegativeInt allowableError,
        PositiveInt coolTime)
    {
        var count = 1u + (uint)(rangeLength.Get - 1) % 128;
        var minIndex = Math.Min(startIndex, uint.MaxValue - (count - 1));
        var maxIndex = minIndex + (count - 1);
        var error = allowableError.Get % 21;
        var cool = 1 + (coolTime.Get - 1) % 30;
        var intervals = observations.Take(32)
            .Select(value => (int)((uint)value % (uint)(cool + 111)))
            .ToArray();

        var expected = SearchBySimulation(seed, minIndex, maxIndex, intervals, error, cool).ToArray();
        var actual = SeedFinder.FindCurrentSeedByBlink(seed, minIndex, maxIndex, intervals, error, cool).ToArray();

        Assert.Equal(expected.Order(), actual.Order());
    }

    [Property(MaxTest = 1000, EndSize = 128)]
    public void FindsSimulatedObservations(
        uint seed,
        uint startIndex,
        PositiveInt rangeLength,
        NonNegativeInt candidateOffset,
        NonNegativeInt observationCount,
        NonNegativeInt allowableError,
        PositiveInt coolTime,
        int[] noise)
    {
        var count = 1u + (uint)(rangeLength.Get - 1) % 128;
        var minIndex = Math.Min(startIndex, uint.MaxValue - (count - 1));
        var maxIndex = minIndex + (count - 1);
        var offset = (candidateOffset.Get % 3) switch
        {
            0 => 0u,
            1 => count - 1,
            _ => (uint)candidateOffset.Get % count
        };
        var error = allowableError.Get % 21;
        var cool = 1 + (coolTime.Get - 1) % 30;
        var length = new[] { 1, 5, 10, 16, 24, 31, 32 }[observationCount.Get % 7];
        var intervals = new int[length];
        var expectedSeed = seed.NextSeed(minIndex + offset);
        var blink = new BlinkObject(cool);
        var consumed = 0u;

        for (var i = 0; i < intervals.Length; i++)
        {
            var frames = WaitForBlink(ref expectedSeed, blink, ref consumed);
            if (i == 0) frames += cool;
            var delta = noise.Length == 0 ? 0 : (int)((uint)noise[i % noise.Length] % (uint)(2 * error + 1)) - error;
            intervals[i] = frames + delta;
        }

        var expected = SearchBySimulation(seed, minIndex, maxIndex, intervals, error, cool).ToArray();
        var actual = SeedFinder.FindCurrentSeedByBlink(seed, minIndex, maxIndex, intervals, error, cool).ToArray();

        Assert.Equal(expected.Order(), actual.Order());
        Assert.Contains(expectedSeed, actual);
    }

    private static IEnumerable<uint> SearchBySimulation(
        uint seed, uint minIndex, uint maxIndex, int[] intervals, int error, int cool)
    {
        if (intervals.Length == 0) yield break;

        var startSeed = seed.NextSeed(minIndex);
        var seen = new HashSet<uint>();

        for (ulong index = minIndex; index <= maxIndex; index++)
        {
            var candidate = startSeed;
            var blink = new BlinkObject(cool);
            var consumed = 0u;
            var matches = true;

            for (var i = 0; i < intervals.Length; i++)
            {
                var frames = WaitForBlink(ref candidate, blink, ref consumed);
                if (i == 0) frames += cool;
                if (Math.Abs(frames - intervals[i]) <= error) continue;
                matches = false;
                break;
            }

            if (matches && seen.Add(candidate)) yield return candidate;
            startSeed = startSeed.NextSeed();
        }
    }

    private static int WaitForBlink(ref uint seed, BlinkObject blink, ref uint consumed)
    {
        var frames = 0;
        do
        {
            frames++;
        }
        while (!blink.CountUp(ref seed, ref consumed));

        return frames;
    }
}
