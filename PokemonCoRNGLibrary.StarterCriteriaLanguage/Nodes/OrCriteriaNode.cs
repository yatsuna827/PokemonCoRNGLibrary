using System;
using System.Linq;
using PokemonPRNG.LCG32;

using static PokemonPRNG.LCG32.Criteria;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    public class OrCriteriaNode<TArg> : ICriteriaNode<TArg>
    {
        private readonly ICriteriaNode<TArg>[] items;
        public OrCriteriaNode(params ICriteriaNode<TArg>[] items)
        {
            if (items == null || items.Length == 0) throw new Exception("空のリストは許可されていません");

            this.items = items;
        }

        public ICriteria<TArg> Build()
            => AND(items.Select(_ => _.Build()).ToArray());

        public string Serialize()
            => items.Length == 1 ?
                items[0].Serialize() :
                $"[ {string.Join(", ", items.Select(_ => _.Serialize()))} ]";
    }
    public class OrCriteriaNode<TArg, TNode> : ICriteriaNode<TArg>
        where TNode : ICriteriaNode<TArg>
    {
        private readonly TNode[] items;
        public OrCriteriaNode(params TNode[] items)
        {
            if (items == null || items.Length == 0) throw new Exception("空のリストは許可されていません");

            this.items = items;
        }

        public ICriteria<TArg> Build()
            => AND(items.Select(_ => _.Build()).ToArray());

        public string Serialize()
            => items.Length == 1 ?
                items[0].Serialize() :
                $"[ {string.Join(", ", items.Select(_ => _.Serialize()))} ]";
    }
}
