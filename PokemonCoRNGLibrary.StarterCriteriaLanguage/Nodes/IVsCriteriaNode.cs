using System;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonCoRNGLibrary.Criteria;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    public class IVsCriteriaNode : ICriteriaNode<GCIndividual>
    {
        private readonly uint[] min = new uint[6];
        private readonly uint[] max = new uint[6];

        public IVsCriteriaNode(uint[] min, uint[] max)
        {
            if (min == null || max == null || min.Length != 6 || max.Length != 6) throw new Exception();

            this.min = min.ToArray();
            this.max = max.ToArray();
        }

        public ICriteria<GCIndividual> Build()
            => new IVsCriteria(min, max);

        public string Serialize()
        {
            var res = new string[6];
            for (int i = 0; i < 6; i++)
            {
                if (min[i] == max[i]) res[i] = $"{min[i]}";
                else if (min[i] == 0 && max[i] == 31) res[i] = "_";
                else res[i] = $"{min[i]}-{max[i]}";
            }

            return $"[ {string.Join(", ", res)} ]";
        }
    }
}
