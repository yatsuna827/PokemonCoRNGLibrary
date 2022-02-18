using PokemonPRNG.LCG32;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary.Criteria
{
    public class ShinyCriteria : ICriteria<GCIndividual>
    {
        private readonly ShinyType shinyType;
        private readonly uint tsv;
        public bool CheckConditions(GCIndividual item) => (item.GetShinyType(tsv) & shinyType) != ShinyType.NotShiny;
        public ShinyCriteria(uint tsv, ShinyType shinyType)
        {
            this.shinyType = shinyType;
            this.tsv = tsv;
        }
    }
}
