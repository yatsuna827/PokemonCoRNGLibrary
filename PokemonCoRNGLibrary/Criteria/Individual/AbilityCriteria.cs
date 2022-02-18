using PokemonPRNG.LCG32;

namespace PokemonCoRNGLibrary.Criteria
{
    public class AbilityCriteria : ICriteria<GCIndividual>
    {
        private readonly string ability;
        public bool CheckConditions(GCIndividual item) => item.Ability == ability;
        internal AbilityCriteria(string ability) => this.ability = ability;
    }
    public class GCAbilityCriteria : ICriteria<GCIndividual>
    {
        private readonly string ability;
        public bool CheckConditions(GCIndividual item) => item.Ability == ability;
        internal GCAbilityCriteria(string ability) => this.ability = ability;
    }
}
