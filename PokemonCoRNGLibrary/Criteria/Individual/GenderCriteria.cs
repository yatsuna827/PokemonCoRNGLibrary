using PokemonPRNG.LCG32;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary.Criteria
{
    public class GenderCriteria : ICriteria<GCIndividual>
    {
        private readonly Gender gender;
        public bool CheckConditions(GCIndividual item) => item.Gender == gender;
        internal GenderCriteria(Gender gender) => this.gender = gender;
    }
}
