using System;
using System.Collections.Generic;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    class NatureListParser : IParser<NatureCriteriaNode>
    {
        public NatureCriteriaNode Parse(SymbolIterator iterator)
        {
            if (!iterator.HasValue)
                throw new Exception("Unexpeced EOF");

            var symbol = iterator.GetAndNext();
            if (symbol == ListOpen.Instance)
            {
                var list = new List<Nature>();

                while (iterator.HasValue)
                {
                    symbol = iterator.GetAndNext();
                    if (symbol == ListClose.Instance)
                        return new NatureCriteriaNode(list.ToArray());

                    if (symbol is Value v)
                        list.Add(ParseNature(v));
                    else
                        throw new UnexpectedSymbolException(symbol);

                    if (!iterator.HasValue) throw new Exception("Unexpected EOF");

                    symbol = iterator.GetAndNext();

                    if (symbol == ListClose.Instance)
                        return new NatureCriteriaNode(list.ToArray());

                    if (symbol != Comma.Instance)
                        throw new UnexpectedSymbolException(symbol);
                }

                throw new Exception("Unexpected EOF");
            }
            else if (symbol is Value v)
                return new NatureCriteriaNode(ParseNature(v));
            else
                throw new UnexpectedSymbolException(symbol);
        }

        private static Nature ParseNature(Value v)
        {
            var nature = v.Name.TryConvertToNature();
            if (nature == Nature.other) throw new Exception("Invalid Value");

            return nature;
        }
    }
}
