using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.GCLCG;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary
{
    public class CoTradePokemon : IGeneratable<GCIndividual>
    {
        public readonly GCSlot slot;
        public readonly uint TID, SID;

        public static readonly CoTradePokemon Plusle = new CoTradePokemon("プラスル", 13, 37149, 00000);

        public GCIndividual Generate(uint seed) => slot.Generate(seed, out _, TID ^ SID);
        public IEnumerable<(uint seed, GCIndividual Individual)> CalcBack(uint h, uint a, uint b, uint c, uint d, uint s) => SeedFinder.FindGeneratingSeed(h, a, b, c, d, s, false).Select(_ => (_, Generate(_)));

        private CoTradePokemon(string name, uint lv, uint tid, uint sid)
        {
            slot = new GCSlot(name, lv);
            TID = tid;
            SID = sid;
        }
    }
}
