using PokemonCoRNGLibrary.AdvanceSource;


namespace Tests
{
    public class PyriteCave_Test
    {
        [Fact]
        public Task TestPyriteCaveSnapshot()
        {
            var seed = 0x1234ABCDu;
            //var actual = seed.EnumerateSeed(new PyriteCave()).Take(200).Select(_ => $"{_:X8}").ToArray();
            //var actual = seed.EnumerateSeed(new PyriteCave(), (_, c) => c.SimulateNextFrame(_.NextSeed(4))).Take(200).Select(_ => $"{_:X8}").ToArray();
            var actual = seed.EnumerateSeed(new PyriteCave().Apply((_, c) => c.SimulateNextFrame(_.NextSeed(4)))).Take(200).Select(_ => $"{_:X8}").ToArray();

            return Verify(actual);
        }
    }
}
