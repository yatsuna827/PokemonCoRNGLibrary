using System;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;
using PokemonCoRNGLibrary.Criteria;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    public class NatureCriteriaNode : ICriteriaNode<GCIndividual>
    {
        private readonly Nature[] natures;
        public NatureCriteriaNode(params Nature[] natures)
        {
            if (natures.Length == 0) throw new Exception("空のリストは許可されていません");

            this.natures = natures;
        }

        public ICriteria<GCIndividual> Build()
            => new NatureCriteria(natures);

        public string Serialize()
            => natures.Length == 1 ?
                natures[0].ToJapanese() :
                $"[ {string.Join(", ", natures.Select(_ => _.ToJapanese()))} ]";
    }
}
