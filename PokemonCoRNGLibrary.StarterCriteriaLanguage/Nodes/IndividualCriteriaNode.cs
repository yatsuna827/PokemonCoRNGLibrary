using System;
using System.Linq;
using System.Collections.Generic;
using PokemonPRNG.LCG32;

using static PokemonPRNG.LCG32.Criteria;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    public class IndividualCriteriaNode : ICriteriaNode<GCIndividual>
    {
        private readonly IVsCriteriaNode ivs;
        private readonly NatureCriteriaNode nature;
        private readonly OrCriteriaNode<GCIndividual, HiddenPowerCriteriaNode> hp;

        public IndividualCriteriaNode(IVsCriteriaNode ivs, NatureCriteriaNode nature, HiddenPowerCriteriaNode[] hiddenPower)
        {
            if (ivs == null && nature == null && hiddenPower == null)
                throw new Exception("空のオブジェクトは許可されていません");

            if (ivs != null)
                this.ivs = ivs;
            if (nature != null)
                this.nature = nature;
            if (hiddenPower != null)
                this.hp = new OrCriteriaNode<GCIndividual, HiddenPowerCriteriaNode>(hiddenPower);
        }

        public ICriteria<GCIndividual> Build()
        {
            var arr = new ICriteriaNode<GCIndividual>[] { nature, ivs, hp }.Where(_ => _ != null).Select(_ => _.Build()).ToArray();

            return AND(arr);
        }

        public string Serialize()
        {
            var arr = new List<string>();
            if (ivs != null)
                arr.Add($"ivs: {ivs.Serialize()}");
            if (nature != null)
                arr.Add($"nature: {nature.Serialize()}");
            if (hp != null)
                arr.Add($"hp: {hp.Serialize()}");

            if (arr.Count == 0) return "{ }";

            return $"{{ {string.Join(", ", arr)} }}";
        }
    }
}
