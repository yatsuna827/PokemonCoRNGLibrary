using System;
using System.Collections.Generic;
using PokemonPRNG.LCG32;
using PokemonCoRNGLibrary.Criteria.Starter;

using static PokemonPRNG.LCG32.Criteria;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    public class CoStarterCriteriaNode : ICriteriaNode<CoStarterResult>
    {
        private readonly OrCriteriaNode<CoStarterResult, IDCriteriaNode> id;
        private readonly OrCriteriaNode<GCIndividual, IndividualCriteriaNode> umbreon;
        private readonly OrCriteriaNode<GCIndividual, IndividualCriteriaNode> espeon;

        public CoStarterCriteriaNode(IDCriteriaNode[] id, IndividualCriteriaNode[] umbreon, IndividualCriteriaNode[] espeon)
        {
            if (id == null && umbreon == null && espeon == null)
                throw new Exception();

            if (id != null)
                this.id = new OrCriteriaNode<CoStarterResult, IDCriteriaNode>(id);
            if (umbreon != null)
                this.umbreon = new OrCriteriaNode<GCIndividual, IndividualCriteriaNode>(umbreon);
            if (espeon != null)
                this.espeon = new OrCriteriaNode<GCIndividual, IndividualCriteriaNode>(espeon);
        }

        public ICriteria<CoStarterResult> Build()
        {
            var list = new List<ICriteria<CoStarterResult>>();
            if (umbreon != null) list.Add(new UmbreonCriteria(umbreon.Build()));
            if (espeon != null) list.Add(new EspeonCriteria(espeon.Build()));
            if (id != null) list.Add(id.Build());

            if (list.Count == 0) throw new Exception("空のオブジェクトは許可されていません");

            return AND(list.ToArray());
        }

        public string Serialize()
        {
            var arr = new List<string>();
            if (id != null)
                arr.Add($"id: {id.Serialize()}");
            if (umbreon != null)
                arr.Add($"umbreon: {umbreon.Serialize()}");
            if (espeon != null)
                arr.Add($"espeon: {espeon.Serialize()}");

            if (arr.Count == 0) return "{ }";

            return $"{{ {string.Join(", ", arr)} }}";
        }
    }
}
