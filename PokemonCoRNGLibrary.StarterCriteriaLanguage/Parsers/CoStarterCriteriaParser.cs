using System;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    class CoStarterCriteriaParser : IParser<CoStarterCriteriaNode>
    {
        private readonly ListParser<GCIndividual, IndividualCriteriaNode> individualParser
            = new ListParser<GCIndividual, IndividualCriteriaNode>(new IndividualParser());
        private readonly ListParser<CoStarterResult, IDCriteriaNode> idParser
            = new ListParser<CoStarterResult, IDCriteriaNode>(new IDCriteriaParser());
        public CoStarterCriteriaNode Parse(SymbolIterator iterator)
        {
            if (!iterator.HasValue)
                throw new Exception("Unexpeced EOF");

            IDCriteriaNode[] id = null;
            IndividualCriteriaNode[] umbreon = null;
            IndividualCriteriaNode[] espeon = null;

            var symbol = iterator.GetAndNext();
            if (symbol != ObjectOpen.Instance) 
                throw new UnexpectedSymbolException(symbol);

            while (iterator.HasValue)
            {
                symbol = iterator.GetAndNext();

                if (symbol == ObjectClose.Instance)
                    return new CoStarterCriteriaNode(id, umbreon, espeon);

                if (symbol is PropertyName p)
                {
                    if (p.Name == "umbreon")
                        SetField(individualParser, iterator, ref umbreon);
                    else if (p.Name == "espeon")
                        SetField(individualParser, iterator, ref espeon);
                    else if (p.Name == "id")
                        SetField(idParser, iterator, ref id);
                    else
                        throw new UnexpectedSymbolException(symbol);
                }
                else throw new UnexpectedSymbolException(symbol);

                if (!iterator.HasValue) throw new Exception("Unexpected EOF");

                symbol = iterator.GetAndNext();
                if (symbol == ObjectClose.Instance)
                    return new CoStarterCriteriaNode(id, umbreon, espeon);

                if (symbol != Comma.Instance)
                    throw new UnexpectedSymbolException(symbol);
            }

            throw new Exception("Unexpected EOF");
        }

        private static void SetField<TArg, TNode>(ListParser<TArg, TNode> parser, SymbolIterator iterator, ref TNode[] field)
            where TNode : ICriteriaNode<TArg>
        {
            if (field != null) throw new Exception("");
            field = parser.Parse(iterator);
        }
    }
}
