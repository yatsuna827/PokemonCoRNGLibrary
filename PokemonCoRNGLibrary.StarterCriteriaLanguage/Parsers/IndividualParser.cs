using System;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    class IndividualParser : IParser<IndividualCriteriaNode>
    {
        private readonly ListParser<GCIndividual, HiddenPowerCriteriaNode> hiddenPowerParser
            = new ListParser<GCIndividual, HiddenPowerCriteriaNode>(new HiddenPowerParser());
        private readonly IVsListParser ivsParser
            = new IVsListParser();
        private readonly NatureListParser natureParser
            = new NatureListParser();

        public IndividualCriteriaNode Parse(SymbolIterator iterator)
        {
            if (!iterator.HasValue)
                throw new Exception("Unexpeced EOF");

            //var node = new IndividualCriteriaNode();
            HiddenPowerCriteriaNode[] hiddenPower = null;
            IVsCriteriaNode ivs = null;
            NatureCriteriaNode nature = null;

            var symbol = iterator.GetAndNext();
            if (symbol != ObjectOpen.Instance)
                throw new UnexpectedSymbolException(symbol);

            while (iterator.HasValue)
            {
                symbol = iterator.GetAndNext();

                if (symbol == ObjectClose.Instance)
                    return new IndividualCriteriaNode(ivs, nature, hiddenPower);

                if (symbol is PropertyName p)
                {
                    if (p.Name == "nature")
                        SetField<GCIndividual, NatureCriteriaNode>(natureParser, iterator, ref nature);
                    else if (p.Name == "ivs")
                        SetField<GCIndividual, IVsCriteriaNode>(ivsParser, iterator, ref ivs);
                    else if (p.Name == "hp")
                        SetField<GCIndividual, HiddenPowerCriteriaNode>(hiddenPowerParser, iterator, ref hiddenPower);
                    else
                        throw new UnexpectedSymbolException(symbol);
                }
                else throw new UnexpectedSymbolException(symbol);

                if (!iterator.HasValue) throw new Exception("Unexpected EOF");

                symbol = iterator.GetAndNext();
                if (symbol == ObjectClose.Instance)
                    return new IndividualCriteriaNode(ivs, nature, hiddenPower);

                if (symbol != Comma.Instance)
                    throw new UnexpectedSymbolException(symbol);
            }

            throw new Exception("Unexpected EOF");
        }

        private static void SetField<TArg, TNode>(IParser<TNode[]> parser, SymbolIterator iterator, ref TNode[] field)
            where TNode : ICriteriaNode<TArg>
        {
            if (field != null) throw new Exception("");
            field = parser.Parse(iterator);
        }
        private static void SetField<TArg, TNode>(IParser<TNode> parser, SymbolIterator iterator, ref TNode field)
            where TNode : ICriteriaNode<TArg>
        {
            if (field != null) throw new Exception("");
            field = parser.Parse(iterator);
        }
    }
}
