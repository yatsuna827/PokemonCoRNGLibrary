using System.Linq;
using PokemonPRNG.LCG32;

namespace PokemonCoRNGLibrary.Criteria
{
    public class StatsCriteria : ICriteria<GCIndividual>
    {
        private readonly int[] indexes;
        private readonly uint[] targetStats;

        public bool CheckConditions(GCIndividual item) => indexes.All(_ => targetStats[_] == item.Stats[_]);

        public StatsCriteria(uint[] targetStats)
        {
            this.indexes = Enumerable.Range(0, 6).Where(_ => targetStats[_] != 0).ToArray();
            this.targetStats = targetStats.ToArray();
        }
    }

}
