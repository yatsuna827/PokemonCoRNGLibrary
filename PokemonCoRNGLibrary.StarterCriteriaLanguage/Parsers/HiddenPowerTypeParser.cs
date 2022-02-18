using System;
using System.Collections.Generic;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    class HiddenPowerTypeParser : IParser<HiddenPowerTypeCriteriaNode>
    {
        public HiddenPowerTypeCriteriaNode Parse(SymbolIterator iterator)
        {
            if (!iterator.HasValue) throw new Exception("Unexpected EOF");

            var symbol = iterator.GetAndNext();
            if (symbol == ListOpen.Instance)
            {
                var list = new List<PokeType>();

                while (iterator.HasValue)
                {
                    symbol = iterator.GetAndNext();
                    if (symbol == ListClose.Instance)
                        return new HiddenPowerTypeCriteriaNode(list.ToArray());

                    if (symbol is Value v)
                        list.Add(ParseType(v));
                    else
                        throw new UnexpectedSymbolException(symbol);



                    if (!iterator.HasValue) throw new Exception("Unexpected EOF");

                    symbol = iterator.GetAndNext();
                    if (symbol == ListClose.Instance)
                        return new HiddenPowerTypeCriteriaNode(list.ToArray());

                    if (symbol != Comma.Instance)
                        throw new UnexpectedSymbolException(symbol);
                }

                throw new Exception("Unexpected EOF");
            }
            else if (symbol is Value v)
                return new HiddenPowerTypeCriteriaNode(ParseType(v));
            else
                throw new UnexpectedSymbolException(symbol);
        }

        private static PokeType ParseType(Value v)
        {
            var type = v.Name.TryConvertToPokeType();
            if (type == PokeType.Non) throw new Exception("Invalid Value");

            return type;
        }
    }
}
