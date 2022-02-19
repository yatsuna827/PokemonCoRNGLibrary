using System;
using System.Collections.Generic;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    class ListParser<TArg> : IParser<ICriteriaNode<TArg>[]>
    {
        private readonly IParser<ICriteriaNode<TArg>> parser;

        public ICriteriaNode<TArg>[] Parse(SymbolIterator iterator)
        {
            if (!iterator.HasValue) throw new Exception("Unexpected EOF");

            if (iterator.Peek != ListOpen.Instance) 
                return new[] { parser.Parse(iterator) };
            iterator.MoveNext();

            var list = new List<ICriteriaNode<TArg>>();
            while (iterator.HasValue)
            {
                if (iterator.Peek == ListClose.Instance)
                {
                    iterator.MoveNext();
                    return list.ToArray();
                }

                list.Add(parser.Parse(iterator));

                if (!iterator.HasValue) throw new Exception("Unexpected EOF");

                var symbol = iterator.GetAndNext();
                if (symbol == ListClose.Instance)
                    return list.ToArray();

                if (symbol != Comma.Instance)
                    throw new UnexpectedSymbolException(symbol);
            }

            throw new Exception("Unexpected EOF");
        }

        public ListParser(IParser<ICriteriaNode<TArg>> parser)
            => this.parser = parser;
    }
    class ListParser<TArg, TNode> : IParser<TNode[]>
        where TNode : ICriteriaNode<TArg>
    {
        private readonly IParser<TNode> parser;

        public TNode[] Parse(SymbolIterator iterator)
        {
            if (!iterator.HasValue) throw new Exception("Unexpected EOF");

            var symbol = iterator.Peek;
            if (symbol != ListOpen.Instance) 
                return new[] { parser.Parse(iterator) };

            var list = new List<TNode>();

            iterator.MoveNext();
            while (iterator.HasValue)
            {
                symbol = iterator.Peek;
                if (symbol == ListClose.Instance)
                {
                    iterator.MoveNext();
                    return list.ToArray();
                }

                list.Add(parser.Parse(iterator));

                if (!iterator.HasValue) throw new Exception("Unexpected EOF");

                symbol = iterator.GetAndNext();
                if (symbol == ListClose.Instance)
                    return list.ToArray();

                if (symbol != Comma.Instance)
                    throw new UnexpectedSymbolException(symbol);
            }

            throw new Exception("Unexpected EOF");
        }

        public ListParser(IParser<TNode> parser)
            => this.parser = parser;
    }
}
