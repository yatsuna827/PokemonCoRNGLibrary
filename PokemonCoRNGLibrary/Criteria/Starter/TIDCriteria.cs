using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;

namespace PokemonCoRNGLibrary.Criteria.Starter
{
    public class TIDCriteria : ICriteria<CoStarterResult>
    {
        private readonly HashSet<uint> tids;
        public bool CheckConditions(CoStarterResult item)
            => tids.Contains(item.TID);

        public TIDCriteria(params uint[] tid)
            => this.tids = new HashSet<uint>(tid);
    }

}
