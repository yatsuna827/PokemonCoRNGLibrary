using System;
using System.Linq;
using System.Collections.Generic;
using PokemonPRNG.LCG32;

using static PokemonPRNG.LCG32.Criteria;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    public class HiddenPowerCriteriaNode : ICriteriaNode<GCIndividual>
    {
        private readonly HiddenPowerPowerCriteriaNode power;
        private readonly HiddenPowerTypeCriteriaNode type;

        public HiddenPowerCriteriaNode(HiddenPowerPowerCriteriaNode power, HiddenPowerTypeCriteriaNode type)
        {
            if (power == null && type == null)
                throw new Exception("空のオブジェクトは許可されていません");

            if (power != null)
                this.power = power;
            if (type != null)
                this.type = type;
        }

        public ICriteria<GCIndividual> Build()
            => AND(new ICriteriaNode<GCIndividual>[] { type, power }.Where(_ => _ != null).Select(_ => _.Build()).ToArray());

        public string Serialize()
        {
            var arr = new List<string>();
            if (type != null)
                arr.Add($"type: {type.Serialize()}");
            if (power != null)
                arr.Add($"power: {power.Serialize()}");

            return $"{{ {string.Join(", ", arr)} }}";
        }
    }
}
