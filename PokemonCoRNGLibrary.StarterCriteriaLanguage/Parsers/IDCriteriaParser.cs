using System;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    class IDCriteriaParser : IParser<IDCriteriaNode>
    {
        private readonly DecListParser decListParser = new DecListParser();
        private readonly HexListParser hexListParser = new HexListParser();

        public IDCriteriaNode Parse(SymbolIterator iterator)
        {
            if (!iterator.HasValue)
                throw new Exception("Unexpeced EOF");

            IDCriteriaNode node;

            var symbol = iterator.GetAndNext();
            if (symbol != ObjectOpen.Instance) 
                throw new UnexpectedSymbolException(symbol);

            symbol = iterator.GetAndNext();

            if (symbol is PropertyName p)
            {
                if (p.Name == "tid")
                    node = new TIDCriteriaNode(decListParser.Parse(iterator));
                else if (p.Name == "shiny")
                    node = new ShinyCriteriaNode(hexListParser.Parse(iterator));
                else if (p.Name == "square")
                    node = new SquareCriteriaNode(hexListParser.Parse(iterator));
                else if (p.Name == "star")
                    node = new StarCriteriaNode(hexListParser.Parse(iterator));
                else
                    throw new UnexpectedSymbolException(symbol);
            }
            else throw new UnexpectedSymbolException(symbol);

            symbol = iterator.GetAndNext();
            if (symbol == ObjectClose.Instance)
                return node;
            if (symbol != Comma.Instance)
                throw new UnexpectedSymbolException(symbol);

            if (iterator.GetAndNext() != ObjectClose.Instance) 
                throw new UnexpectedSymbolException(symbol);

            return node;
        }
    }
}
