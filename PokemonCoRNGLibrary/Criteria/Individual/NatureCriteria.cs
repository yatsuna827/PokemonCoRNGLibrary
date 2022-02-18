using PokemonPRNG.LCG32;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary.Criteria
{
    public class NatureCriteria : ICriteria<GCIndividual>
    {
        private readonly bool[] checkList = new bool[25];
        public bool CheckConditions(GCIndividual item) => checkList[(uint)item.Nature];
        public NatureCriteria(params Nature[] natures) { foreach (var nature in natures) if (nature != Nature.other) checkList[(uint)nature] = true; }
    }
}
