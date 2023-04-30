using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonCoRNGLibrary.AdvanceSource
{
    public class GenerateBattleNowTeam : ISeedEnumeratorHandler
    {
        private readonly RentalPartyRank _rank;
        public GenerateBattleNowTeam(RentalPartyRank rank) => _rank = rank;

        public uint Advance(uint seed)
            => _rank.AdvanceSeed(seed);

        public uint Initialize(uint seed)
            => seed;

        public uint SelectCurrent(uint seed)
            => seed;
    }
}
