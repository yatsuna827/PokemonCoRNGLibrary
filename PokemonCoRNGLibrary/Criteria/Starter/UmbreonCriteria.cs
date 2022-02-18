using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary.Criteria.Starter
{
    public class UmbreonCriteria : ICriteria<CoStarterResult>
    {
        private readonly ICriteria<GCIndividual> criteria;
        public bool CheckConditions(CoStarterResult item)
            => criteria.CheckConditions(item.Umbreon);

        public UmbreonCriteria(ICriteria<GCIndividual> criteria)
            => this.criteria = criteria;
    }

}
