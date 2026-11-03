using FsCheck;
using FsCheck.Xunit;
using PokemonCoRNGLibrary;
using PokemonPRNG.LCG32.GCLCG;
using Xunit;

namespace Tests;

public class BlinkSeedSearchFasterProperties
{
    [Property(MaxTest = 1000, EndSize = 256)]
    public void MatchesEnumeratedTimeline(uint seed, uint startIndex, PositiveInt rangeLength,
        NonEmptyArray<int> observations, NonNegativeInt allowableError, PositiveInt coolTime)
    {
        var count = 1u + (uint)(rangeLength.Get - 1) % 2048;
        var minIndex = Math.Min(startIndex, uint.MaxValue - (count - 1));
        var maxIndex = minIndex + count - 1;
        var error = allowableError.Get % 21;
        var cool = 1 + (coolTime.Get - 1) % 30;
        var input = observations.Get.Take(256).Select(value => (int)((uint)value % (uint)(cool + 111))).ToArray();
        var expected = SearchByTimeline(seed, minIndex, maxIndex, input, error, cool).ToArray();
        var actual = SeedFinder.FindCurrentSeedByBlinkFaster(seed, minIndex, maxIndex, input, error, cool).ToArray();
        Assert.Equal(expected.Order(), actual.Order());
    }

    [Property(MaxTest = 1000, EndSize = 256)]
    public void FindsEnumeratedObservations(uint seed, uint rangeStart, PositiveInt rangeLength,
        NonNegativeInt candidateOffset, NonNegativeInt observationCount, NonNegativeInt allowableError,
        PositiveInt coolTime, int[] noise)
    {
        var count = 1u + (uint)(rangeLength.Get - 1) % 2048;
        var minIndex = (rangeStart % 3) switch
        {
            0 => 0u,
            1 => uint.MaxValue - (count - 1),
            _ => Math.Min(rangeStart, uint.MaxValue - (count - 1))
        };
        var maxIndex = minIndex + count - 1;
        var length = new[] { 1, 5, 10, 32, 128, 255, 256 }[observationCount.Get % 7];
        var error = allowableError.Get % 21;
        var cool = 1 + (coolTime.Get - 1) % 30;
        using var source = seed.NextSeed(minIndex).EnumerateBlinkingSeed(cool).GetEnumerator();
        var timeline = new List<(uint seed, int interval, int frame, uint lcgIndex)>();
        do
        {
            source.MoveNext();
            timeline.Add(source.Current);
        }
        while (source.Current.lcgIndex <= count);
        var lastHead = Math.Max(0, timeline.Count - 2);
        var head = (candidateOffset.Get % 3) switch
        {
            0 => 0,
            1 => lastHead,
            _ => candidateOffset.Get % (lastHead + 1)
        };
        while (timeline.Count < head + length)
        {
            source.MoveNext();
            timeline.Add(source.Current);
        }
        var input = new int[length];
        for (var i = 0; i < length; i++)
        {
            var delta = noise.Length == 0 ? 0 : (int)((uint)noise[i % noise.Length] % (uint)(2 * error + 1)) - error;
            input[i] = timeline[head + i].interval + delta;
        }
        var expected = SearchByTimeline(seed, minIndex, maxIndex, input, error, cool).ToArray();
        var actual = SeedFinder.FindCurrentSeedByBlinkFaster(seed, minIndex, maxIndex, input, error, cool).ToArray();
        Assert.Equal(expected.Order(), actual.Order());
        Assert.Contains(timeline[head + length - 1].seed, actual);
    }

    [Fact]
    public void TerminatesAtFullPeriod()
    {
        const uint seed = 0x12345678;
        var timeline = seed.EnumerateBlinkingSeed(4).Skip(16).Take(64).ToArray();
        var input = timeline.Select(value => value.interval).ToArray();
        var actual = SeedFinder.FindCurrentSeedByBlinkFaster(seed, 0, uint.MaxValue, input, 0, 4).ToArray();
        Assert.Contains(timeline[^1].seed, actual);
    }

    private static IEnumerable<uint> SearchByTimeline(uint seed, uint minIndex, uint maxIndex,
        int[] input, int error, int cool)
    {
        var count = (ulong)maxIndex - minIndex + 1;
        using var source = seed.NextSeed(minIndex).EnumerateBlinkingSeed(cool).GetEnumerator();
        var timeline = new List<(uint seed, int interval, int frame, uint lcgIndex)>();
        for (var head = 0; ; head++)
        {
            while (timeline.Count < head + input.Length)
            {
                source.MoveNext();
                timeline.Add(source.Current);
            }
            if (head > 0 && timeline[head].lcgIndex > count) yield break;
            var matched = true;
            for (var i = 0; i < input.Length; i++)
            {
                var interval = timeline[head + i].interval;
                if (unchecked(input[i] + error) < interval || interval < unchecked(input[i] - error))
                {
                    matched = false;
                    break;
                }
            }
            if (matched) yield return timeline[head + input.Length - 1].seed;
        }
    }
}
