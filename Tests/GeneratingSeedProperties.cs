using FsCheck.Xunit;
using PokemonCoRNGLibrary;
using PokemonPRNG.LCG32.GCLCG;
using Xunit;

namespace Tests;

public class GeneratingSeedProperties
{
    private static readonly uint[][] Lower = BuildLowerTable();

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MatchesAllKeys(bool generateEnemyTSV)
    {
        const uint hab = 0x5A3C;
        for (uint key = 0; key < 0x8000; key++)
        {
            var scd = unchecked(key + 0x43FDu * hab) & 0x7FFF;
            var expected = SearchByTable(hab, scd, generateEnemyTSV);
            var actual = SeedFinder.FindGeneratingSeed(hab & 31, (hab >> 5) & 31, (hab >> 10) & 31,
                (scd >> 5) & 31, (scd >> 10) & 31, scd & 31, generateEnemyTSV).ToArray();
            Assert.Equal(expected.Order(), actual.Order());
        }
    }

    [Property(MaxTest = 1000)]
    public void MatchesTableForIndividualValues(uint h, uint a, uint b, uint c, uint d, uint s,
        bool generateEnemyTSV)
    {
        h &= 31; a &= 31; b &= 31; c &= 31; d &= 31; s &= 31;
        var hab = h | (a << 5) | (b << 10);
        var scd = s | (c << 5) | (d << 10);
        Assert.Equal(SearchByTable(hab, scd, generateEnemyTSV).Order(),
            SeedFinder.FindGeneratingSeed(h, a, b, c, d, s, generateEnemyTSV).Order());
    }

    [Property(MaxTest = 1000)]
    public void FindsGeneratingSeed(uint seed, bool generateEnemyTSV)
    {
        var offset = generateEnemyTSV ? 5u : 3u;
        var first = seed.NextSeed(offset);
        var second = first.NextSeed();
        var actual = SeedFinder.FindGeneratingSeed((first >> 16) & 31, (first >> 21) & 31,
            (first >> 26) & 31, (second >> 21) & 31, (second >> 26) & 31, (second >> 16) & 31,
            generateEnemyTSV).ToArray();
        Assert.Contains(seed, actual);
        foreach (var candidate in actual)
        {
            var ivSeed = candidate.NextSeed(offset);
            Assert.Equal((first >> 16) & 0x7FFF, (ivSeed >> 16) & 0x7FFF);
            Assert.Equal((second >> 16) & 0x7FFF, (ivSeed.NextSeed() >> 16) & 0x7FFF);
        }
    }

    private static uint[] SearchByTable(uint hab, uint scd, bool generateEnemyTSV)
    {
        var offset = generateEnemyTSV ? 5u : 3u;
        var key = unchecked(scd - 0x43FDu * hab) & 0x7FFF;
        var result = new List<uint>();
        foreach (var low16 in Lower[key])
        {
            var seed = ((hab << 16) | low16).PrevSeed(offset);
            result.Add(seed);
            result.Add(seed ^ 0x80000000);
        }
        return result.ToArray();
    }

    private static uint[][] BuildLowerTable()
    {
        var lower = Enumerable.Range(0, 0x8000).Select(_ => new List<uint>()).ToArray();
        for (uint value = 0; value < 0x10000; value++) lower[(value.NextSeed() >> 16) & 0x7FFF].Add(value);
        return lower.Select(values => values.ToArray()).ToArray();
    }
}
