using PokemonCoRNGLibrary.AdvanceSource;

namespace Tests
{
    public class OutskirtStand_Test
    {
        [Fact]
        public Task TestOutskirtStandSnapshot()
        {
            var seed = 0x1234ABCDu;
            //var actual = seed.EnumerateSeed(new OutskirtStand()).Take(200).Select(_ => $"{_:X8}").ToArray();
            var actual = seed.EnumerateSeed(new OutskirtStand().Apply((_, c) => c.SimulateNextFrame(_.NextSeed(4)))).Take(200).Select(_ => $"{_:X8}").ToArray();

            return Verify(actual);
        }
    }
}
