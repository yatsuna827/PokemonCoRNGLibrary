using System;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;
using PokemonCoRNGLibrary.Criteria;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    public class HiddenPowerTypeCriteriaNode : ICriteriaNode<GCIndividual>
    {
        private readonly PokeType[] types;
        public HiddenPowerTypeCriteriaNode(params PokeType[] types)
        {
            if (types.Length == 0) throw new Exception("空のリストは許可されていません");

            this.types = types;
        }

        public ICriteria<GCIndividual> Build()
            => new HiddenPowerTypeCriteria(types.ToArray());

        public string Serialize()
            => types.Length == 1 ?
                types[0].ToJapanese() :
                $"[ {string.Join(", ", types.Select(_ => _.ToJapanese()))} ]";
    }
}
