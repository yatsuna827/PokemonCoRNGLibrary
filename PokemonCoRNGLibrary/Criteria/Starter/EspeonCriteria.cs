using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary.Criteria.Starter
{
    public class EspeonCriteria : ICriteria<CoStarterResult>
    {
        private readonly ICriteria<GCIndividual> criteria;
        public bool CheckConditions(CoStarterResult item)
            => criteria.CheckConditions(item.Espeon);

        public EspeonCriteria(ICriteria<GCIndividual> criteria)
            => this.criteria = criteria;
    }
}
