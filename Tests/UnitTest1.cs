using PokemonCoRNGLibrary.AdvanceSource;
using PokemonCoRNGLibrary.AdvancePlanning;
using PokemonPRNG.LCG32.GCLCG;

namespace Tests
{
    public class UnitTest1
    {
        public static readonly object[][] TestCasesBattleBlinkEnemyNotBlinking = new[]
        {
            new object[] { 0xBF95A713u, 0x00440465u, new[] { 843, 502, 522, 720, 772, 423, 668, 802, 857, 410 } },
            new object[] { 0xD3599B99u, 0x032EAE5Du, new[] { 745, 573, 840, 403, 669, 497, 326, 409, 707, 674 } },
            new object[] { 0x7B4AD11Au, 0x00903C3Au, new[] { 672, 407, 561, 701, 782, 301, 520, 847, 390, 636 } },
            new object[] { 0x0260E8BEu, 0x0150F27Cu, new[] { 398, 526, 356, 725, 566, 1085, 764, 475, 582, 482 } },
            new object[] { 0x57EB1654u, 0x04260C2Eu, new[] { 350, 817, 617, 437, 657, 621, 625, 797, 537, 337 } },
            new object[] { 0x532B108Au, 0x0063ECE7u, new[] { 375, 308, 825, 715, 453, 588, 296, 671, 484, 572 } },
            new object[] { 0xDC185CDEu, 0x01716158u, new[] { 518, 291, 539, 776, 753, 727, 809, 690, 643, 812 } },
            new object[] { 0xD0C1A7BDu, 0x03B82F85u, new[] { 823, 468, 277, 564, 337, 749, 333, 195, 912, 401 } },
            new object[] { 0x2AA7C1F7u, 0x9BCC1FE3u, new[] { 421, 491, 451, 511, 266, 370, 803, 442, 839, 336 } },
            new object[] { 0x831A2597u, 0x7E15B995u, new[] { 712, 658, 669, 268, 503, 688, 777, 874, 319, 802 } },
        };

        public static readonly object[][] TestCasesBattleBlinkEnemyBlinking = new[]
        {
            new object[] { 0xC6622C93u, 0x00F1F813u, new[] { 381, 805, 418, 402, 639, 329, 706, 334, 434, 923 } },
            new object[] { 0xB0AA9996u, 0x004BB6DAu, new[] { 431, 541, 568, 227, 650, 785, 655, 801, 729, 482 } },
            new object[] { 0x655DF5B7u, 0x040BEEACu, new[] { 386, 344, 619, 548, 330, 762, 851, 566, 213, 286 } },
            new object[] { 0x8ADE2200u, 0x03F5E3C5u, new[] { 584, 418, 257, 618, 609, 361, 838, 746, 513, 585 } },
            new object[] { 0x99DE0674u, 0xD8C84D07u, new[] { 227, 506, 460, 484, 467, 805, 736, 572, 660, 691 } },
            new object[] { 0xD5C9FE9Bu, 0x03E545F4u, new[] { 608, 645, 753, 888, 809, 503, 313, 769, 261, 309 } },
            new object[] { 0x4E218E4Cu, 0x00DB1AB6u, new[] { 401, 439, 784, 275, 377, 269, 577, 697, 603, 554 } },
            new object[] { 0x7567374Du, 0x0372DBA5u, new[] { 284, 433, 701, 851, 393, 729, 730, 396, 638, 678 } },
            new object[] { 0xC3B4431Du, 0x442B0ACEu, new[] { 725, 424, 357, 785, 288, 715, 234, 903, 707, 736 } },
            new object[] { 0xC005C56Eu, 0x033FCE77u, new[] { 393, 727, 341, 519, 741, 906, 839, 699, 652, 626 } },
        };

        [Theory]
        [MemberData(nameof(TestCasesBattleBlinkEnemyNotBlinking))]
        public void TestBattleBlinkEnemyNotBlinking(uint seed, uint expected, int[] blanks)
        {
            // 本当は良くないんだけど…。
            // コーナーケースを組み立てるのが大変なので…。
            var randomized = blanks.Randomize(20);

            var handler = BlinkObjectEnumeratorHanlder.ResultScene(new BlinkObject(4), enemyBlinking: false);
            var result = SeedFinder.FindCurrentSeedByBlinkInBattle(seed, 500000, randomized, handler, 20).ToArray();

            Assert.True(result.Length > 0);
            Assert.Contains(expected, result);
            Assert.Equal(1, result.Count(_ => _ == expected));
        }

        [Theory]
        [MemberData(nameof(TestCasesBattleBlinkEnemyBlinking))]
        public void TestTestCasesBattleBlinkEnemyBlinking(uint seed, uint expected, int[] blanks)
        {
            // 本当は良くないんだけど…。
            // コーナーケースを組み立てるのが大変なので…。
            var randomized = blanks.Randomize(20);

            var handler = BlinkObjectEnumeratorHanlder.ResultScene(new BlinkObject(4), enemyBlinking: true);
            var result = SeedFinder.FindCurrentSeedByBlinkInBattle(seed, 500000, randomized, handler, 20).ToArray();

            Assert.True(result.Length > 0);
            Assert.Contains(expected, result);
            Assert.Equal(1, result.Count(_ => _ == expected));
        }
    }

    public class UnitTest2
    {
        [Fact]
        public void TestNamingScreen()
        {
            var seed = 0xDEADBEEFu;

            var arr1 = seed.EnumerateSeedAtNamingScreen().Take(100).ToArray();
            var arr2 = seed.EnumerateSeed(new NamingScreen()).Take(100).ToArray();

            Assert.Equal(arr1, arr2);
        }

        [Fact]
        public void TestCipherLabB1F()
        {
            var seed = 0xDEADBEEFu;

            var arr1 = seed.EnumerateSeedAtCipherLabB1F().Take(100).ToArray();
            var arr2 = seed.EnumerateSeed(new CipherLabB2F()).Take(100).ToArray();

            Assert.Equal(arr1, arr2);
        }

        [Fact]
        public void TestCipherLabB2F()
        {
            var seed = 0xDEADBEEFu;

            var arr1 = seed.EnumerateSeedAtCipherLabB2F().Take(100).ToArray();
            var arr2 = seed.EnumerateSeed(new CipherLabB3F()).Take(100).ToArray();

            Assert.Equal(arr1, arr2);
        }

        [Fact]
        public void TestOutskirtStand()
        {
            var seed = 0xDEADBEEFu;

            var arr1 = seed.EnumerateSeedAtOutskirtStand().Take(100).ToArray();
            var arr2 = seed.EnumerateSeed(new OutskirtStand()).Take(100).ToArray();

            Assert.Equal(arr1, arr2);
        }

        [Fact]
        public void TestPyriteCave()
        {
            var seed = 0xDEADBEEFu;

            var arr1 = seed.EnumerateSeedAtPyriteCave().Take(100).ToArray();
            var arr2 = seed.EnumerateSeed(new PyriteCave()).Take(100).ToArray();

            Assert.Equal(arr1, arr2);
        }

        [Fact]
        public void TestBattleBlink()
        {
            var seed = 0xDEADBEEFu;

            var arr1 = seed.EnumerateSeedInBattle(4, false).Take(100).ToArray();
            var arr2 = seed.EnumerateSeed(new BlinkObjectEnumeratorHanlder(
                new BlinkObject(10, 10), 
                new BlinkObject(4, 10)
            )).Take(100).ToArray();

            Assert.Equal(arr1, arr2);
        }

        [Fact]
        public void TestBlinkSequence()
        {
            var seed = 0xCAFEBEEF;

            var arr1 = seed.EnumerateBlinkingSeed(4).Take(1000).Select(_ => (_.seed, _.frame, _.interval)).ToArray();
            var arr2 = seed.EnumerateActionSequence(new BlinkObjectEnumeratorHanlder(new BlinkObject(4))).Take(1000).ToArray();

            Assert.Equal(arr1, arr2);
        }

        [Fact]
        public void TestCipherLabB2FIrregularAdvance()
        {
            var seed = 0xE6C6E208u;
            var target = 0xB27AE396u;

            var a = IrregularAdvanceCalculator.CalcBlinkAndVibravaFrame(seed, target, 50, 1800, 20000, 300, 700);
            var b = new BlinkAdvancePlanner(new BlinkObject(4), new CipherLabB2F())
                .CalculatePlanning(seed, target, 50, 1800, 20000, 300, 700);

            Assert.True(a.Any());
            Assert.Equal(a.ToArray(), b.ToArray());
        }
        [Fact]
        public void TestCipherLabB3FIrregularAdvance()
        {
            var seed = 0xE6C6E208u;
            var target = 0xB27AE396u;

            var a = IrregularAdvanceCalculator.CalcBlinkAndBubbleFrame(seed, target, 94, 1800, 20000, 300, 700);
            var b = new BlinkAdvancePlanner(new BlinkObject(4), new CipherLabB3F())
                .CalculatePlanning(seed, target, 94, 1800, 20000, 300, 700);

            Assert.True(a.Any());
            Assert.Equal(a.ToArray(), b.ToArray());
        }
        [Fact]
        public void TestPyriteCaveIrregularAdvance()
        {
            var seed = 0xE6C6E208u;
            var target = 0xB27AE396u;

            var a = IrregularAdvanceCalculator.CalcBlinkAndSmokeFrame(seed, target, 94, 1800, 20000, 300, 700);
            var b = new BlinkAdvancePlanner(new BlinkObject(4), new PyriteCave())
                .CalculatePlanning(seed, target, 94, 1800, 20000, 300, 700);

            Assert.True(a.Any());
            Assert.Equal(a.ToArray(), b.ToArray());
        }
        [Fact]
        public void TestOutskirtStandIrregularAdvance()
        {
            var seed = 0xE6C6E208u;
            var target = 0xB27AE396u;

            var a = IrregularAdvanceCalculator.CalcBlinkAndStandFrame(seed, target, 94, 1800, 20000, 300, 700);
            var b = new BlinkAdvancePlanner(new BlinkObject(4), new OutskirtStand())
                .CalculatePlanning(seed, target, 94, 1800, 20000, 300, 700);

            Assert.True(a.Any());
            Assert.Equal(a.ToArray(), b.ToArray());
        }

        [Fact]
        public void TestBlinkAdvancePlanner()
        {
            var seed = 0xE6C6E208u;
            var target = 0xB27AE396u;

            var planner = new BlinkAdvancePlanner(new BlinkObject(4), new CipherLabB2F());

            var a = planner.CalculatePlanning(seed, target, 50, 1800, 20000, 300, 700);
            var b = planner.CalculatePlanning(seed, target, 50, 1800, 20000, 300, 700);

            Assert.True(a.Any());
            Assert.Equal(a.ToArray(), b.ToArray());
        }
    }

    static class Util
    {
        static int[] ToTimeline(this int[] blanks)
        {
            var arr = new int[blanks.Length + 1];
            for (int i = 1; i < arr.Length; i++)
                arr[i] = arr[i - 1] + blanks[i - 1];

            return arr;
        }
        static int[] WithError(this int[] timeline, int error)
        {
            var arr = timeline.ToArray();
            var gosa = new int[arr.Length - 1];
            var rand = new Random();
            for (int i = 1; i < arr.Length; i++)
            {
                var v = rand.Next(error);
                arr[i] += v;
                gosa[i - 1] = v;
            }

            return arr;
        }
        static int[] ToBlanks(this int[] timeline)
        {
            var arr = new int[timeline.Length - 1];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = timeline[i + 1] - timeline[i];

            return arr;
        }

        public static int[] Randomize(this int[] blanks, int error)
            => blanks.ToTimeline().WithError(error).ToBlanks();
    }
}