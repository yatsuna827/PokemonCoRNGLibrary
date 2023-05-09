﻿using PokemonCoRNGLibrary.ProvidedData;
using PokemonPRNG.LCG32;

namespace Tests
{
    public class TestGeneration
    {
        [Fact]
        public void TestCalcBack()
        {
            var actual = ProvidedCoDarkPokemonData.Get("ヘラクロス").GetGenerator()
                .CalcBack(26, 30, 10, 11, 26, 7).Select(_ => _.Seed).ToArray();
            var expected = new uint[]
            {
                0xBEEF01E6u,
                0x00910032u,
                0x3E8CE03Cu,
                0xE343D096u,
                0x01EED3C0u,
                0x6693003Au,
                0xF4822084u,
                0xB9E0531Eu,
                0x6856AA88u,
                0xA45ACD42u,
                0x67B395CCu,
                0xD922B2A6u,
                0x4A5B4650u,
                0xD6AD874Au,
                0x300F6014u,
                0xD56A0F2Eu,
                0x0454C718u,
                0x89A44E52u,
                0xEF779F5Cu,
                0xBCA988B6u,
                0x58CF4CE0u,
                0x012C425Au,
                0xDE4273A4u,
                0x06E83F3Eu,
                0xE50AF7A8u,
                0x51868362u,
                0xA7B9FCECu,
                0x56C152C6u,
                0xE57BE770u,
                0x47C8316Au,
                0x969C5B34u,
                0x09E3E34Eu,
                0x4A4A3C38u,
                0x225A6C72u,
                0x119BAE7Cu,
                0x999310D6u,
                0x1BD21600u,
                0x197A547Au,
                0x67DE16C4u,
                0xCB25FB5Eu,
                0x2F2394C8u,
                0xB7B90982u,
                0xED7DB40Cu,
                0xB087C2E6u,
                0x2A82D890u,
                0x027BAB8Au,
                0x6808A654u,
                0x78B7876Eu,
                0xD9E80158u,
                0x727B5A92u,
                0xCB010D9Cu,
                0x104868F6u,
                0xD37F2F20u,
                0xBC45369Au,
                0x445D09E4u,
                0x91E1877Eu,
                0x6C2881E8u,
                0x68BA5FA2u,
                0x9906BB2Cu,
                0x86BE0306u,
                0xFBF819B0u,
                0x3D8FF5AAu,
                0xD15C4174u,
                0xF72CFB8Eu,
                0x72B61678u,
                0x75CF18B2u,
                0x35AFBCBCu,
                0x4B119116u,
                0x3C5E9840u,
                0xCA54E8BAu,
                0x9AC74D04u,
                0xFA62E39Eu,
                0x75A1BF08u,
                0x4A5285C2u,
                0xFE5D124Cu,
                0x0DAC1326u,
                0x7063AAD0u,
                0x03CD0FCAu,
                0x739F2C94u,
                0x6E8C3FAEu,
                0x883C7B98u,
                0x7C1DA6D2u,
                0x5FAFBBDCu,
                0x08368936u,
                0x46F85160u,
                0xF8716ADAu,
                0x0624E024u,
                0xB7F20FBEu,
                0xD9174C28u,
                0x96497BE2u,
                0x6588B96Cu,
                0x0D99F346u,
                0xD24D8BF0u,
                0x33FAF9EAu,
                0x63D967B4u,
                0xDC1D53CEu,
                0x420330B8u,
                0x292F04F2u,
                0x4B090AFCu,
                0x99FF5156u,
                0x17D45A80u,
                0xCF62BCFAu,
                0x957DC344u,
                0x91D70BDEu,
                0xD8112948u,
                0xDA674202u,
                0x0A91B08Cu,
                0xE2CFA366u,
                0xA03DBD10u,
                0x80E1B40Au,
                0x2B12F2D4u,
                0x512837EEu,
                0x7B9235D8u,
                0x74CB3312u,
                0xEDC3AA1Cu,
                0xE6B3E976u,
                0x077AB3A0u,
                0xABF0DF1Au,
                0xCBD9F664u,
                0x6359D7FEu,
                0x68175668u,
                0xF873D822u,
                0x1D7FF7ACu,
                0x7D952386u,
                0x8CBC3E30u,
                0x71493E2Au,
                0xC653CDF4u,
                0xF2F4EC0Eu,
                0xC4718AF8u,
                0xAABA3132u,
                0x31E7993Cu,
                0x689C5196u,
                0xA2735CC0u,
                0xBEE3D13Au,
                0xA0417984u,
                0x1BC2741Eu,
                0x32B1D388u,
                0x26373E42u,
                0xC25B8ECCu,
                0x623273A6u,
                0x7E510F50u,
                0x5FF9984Au,
                0xA6A3F914u,
                0xFACB702Eu,
                0x60293018u,
                0x6AC3FF52u,
                0xF57CD85Cu,
                0x2E0089B6u,
                0xA94655E0u,
                0x0D03935Au,
                0x7DBC4CA4u,
                0xBE58E03Eu,
                0x9568A0A8u,
                0xED797462u,
                0x112C75ECu,
                0xA8EF93C6u,
                0x8F843070u,
                0x7BBAC26Au,
                0xB10B7434u,
                0xB5F3C44Eu,
                0x46412538u,
                0xA8B09D72u,
                0x0A8B677Cu,
                0xD92891D6u,
                0x107B9F00u,
                0x6F18257Au,
                0x43526FC4u,
                0x62651C5Eu,
                0xA1C3BDC8u,
                0x2C027A82u,
                0x15FAAD0Cu,
                0xFE1483E6u,
                0x0EDDA190u,
                0xC754BC8Au,
                0x3E923F54u,
                0x85B5E86Eu,
                0x22416A58u,
                0xAC480B92u,
                0x371B469Cu,
                0xA05C69F6u,
                0x009B3820u,
                0x91E9879Au,
                0x440BE2E4u,
                0x332F287Eu,
                0x1D4B2AE8u,
                0x139A50A2u,
                0xD0CE342Cu,
                0xA1E94406u,
                0x7EE562B0u,
                0x198F86AAu,
                0x1C405A74u,
                0xDF59DC8Eu,
                0x53B1FF78u,
                0x115249B2u,
                0x353475BCu,
                0x4DE41216u,
                0xD62D2140u,
                0xF63FB9BAu,
                0x46F0A604u,
                0x6FFF049Eu,
                0x8186E808u,
                0x2A08F6C2u,
                0x35AF0B4Cu,
                0x68B5D426u,
                0x962373D0u,
                0x1D3320CAu,
                0x8B1DC594u,
                0x4C27A0AEu,
                0xEE1AE498u,
                0xC79757D2u,
                0xB2DEF4DCu,
            };

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestGenerateWithPreGenerate()
        {
            var actual = ProvidedCoDarkPokemonData.Get("ヤミカラス").GetGenerator().Generate(0xDEADFACE);
            Assert.Equal(new uint[] { 0, 12, 7, 11, 24, 28 }, actual.Content.IVs);
            Assert.Equal(0x1DAFA6DDu, actual.Content.PID);
        }
        [Fact]
        public void TestGenerate()
        {
            var seed = 0x1C71CE9Bu;
            var poke = ProvidedCoDarkPokemonData.Get("マクノシタ").GetGenerator();

            var actual = seed.Generate(poke);
            Assert.Equal(new uint[] { 2, 19, 16, 1, 24, 29 }, actual.Content.IVs);
            Assert.Equal(0xDFAD0343u, actual.Content.PID);
        }
    }
}
