using System;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    class HiddenPowerParser : IParser<HiddenPowerCriteriaNode>
    {
        private readonly HiddenPowerTypeParser typeParser = new HiddenPowerTypeParser();
        private readonly HiddenPowerPowerParser powerParser = new HiddenPowerPowerParser();

        public HiddenPowerCriteriaNode Parse(SymbolIterator iterator)
        {
            if (!iterator.HasValue)
                throw new Exception("Unexpeced EOF");

            HiddenPowerPowerCriteriaNode power = null;
            HiddenPowerTypeCriteriaNode type = null;

            var symbol = iterator.GetAndNext();
            if (symbol != ObjectOpen.Instance) 
                throw new UnexpectedSymbolException(symbol);

            while (iterator.HasValue)
            {
                symbol = iterator.GetAndNext();

                if (symbol == ObjectClose.Instance)
                    return new HiddenPowerCriteriaNode(power, type);

                if (symbol is PropertyName p)
                {
                    if (p.Name == "type")
                        SetField(typeParser, iterator, ref type);
                    else if (p.Name == "power")
                        SetField(powerParser, iterator, ref power);
                    else
                        throw new UnexpectedSymbolException(symbol);
                }
                else throw new UnexpectedSymbolException(symbol);

                if (!iterator.HasValue) throw new Exception("Unexpected EOF");

                symbol = iterator.GetAndNext();
                if (symbol == ObjectClose.Instance)
                    return new HiddenPowerCriteriaNode(power, type);

                if (symbol != Comma.Instance)
                    throw new UnexpectedSymbolException(symbol);
            }

            throw new Exception("Unexpected EOF");
        }
        private static void SetField<TNode>(IParser<TNode> parser, SymbolIterator iterator, ref TNode field)
            where TNode : ICriteriaNode<GCIndividual>
        {
            if (field != null) throw new Exception("");
            field = parser.Parse(iterator);
        }
    }
}
