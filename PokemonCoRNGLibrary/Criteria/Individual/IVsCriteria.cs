using System;
using System.Linq;
using PokemonPRNG.LCG32;


namespace PokemonCoRNGLibrary.Criteria
{
    public class IVsCriteria : ICriteria<GCIndividual>
    {
        private readonly uint[] minIVs;
        private readonly uint[] maxIVs;

        public bool CheckConditions(GCIndividual item)
        {
            for (int i = 0; i < 6; i++)
                if (item.IVs[i] < minIVs[i] || maxIVs[i] < item.IVs[i]) return false;
            return true;
        }
        public IVsCriteria(uint[] minIVs, uint[] maxIVs)
        {
            if (minIVs == null || maxIVs == null || minIVs.Length != 6 || maxIVs.Length != 6)
                throw new ArgumentException();

            this.minIVs = minIVs.ToArray();
            this.maxIVs = maxIVs.ToArray();
        }
    }
}
