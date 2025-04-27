using PokemonCoRNGLibrary.AdvanceSource;

namespace Tests
{
    public class PyriteTown_Test
    {
        [Fact]
        public void TestPyriteTownWithNPC()
        {
            var seed = 0x8BDDC51u.NextSeed(6);
            var actual = seed.EnumerateSeed(new PyriteTownWithNPC(48)).Skip(100).First();

            // カウンタの値は0.5675353,0.61759937,0.029838443だけど取り出すのが面倒
            Assert.Equal(0x98003E4Fu, actual);
        }

        [Fact]
        public Task TestPyriteTownSnapshot()
        {
            var seed = 0x1234ABCDu;
            //var actual = seed.EnumerateSeed(new PyriteTown()).Take(200).Select(_ => $"{_:X8}").ToArray();
            var actual = seed.EnumerateSeed(new PyriteTown().Apply((_, c) => c.SimulateNextFrame(_.NextSeed(4)))).Take(200).Select(_ => $"{_:X8}").ToArray();

            return Verify(actual);
        }

    }
}
