using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    class IVsListParser : IParser<IVsCriteriaNode>
    {
        public IVsCriteriaNode Parse(SymbolIterator iterator)
        {
            if (!iterator.HasValue)
                throw new Exception("Unexpeced EOF");

            var min = new List<uint>(6);
            var max = new List<uint>(6);

            var symbol = iterator.GetAndNext();
            if (symbol != ListOpen.Instance)
                throw new UnexpectedSymbolException(symbol);

            while (iterator.HasValue)
            {
                symbol = iterator.GetAndNext();

                if (symbol == ListClose.Instance)
                    return new IVsCriteriaNode(min.ToArray(), max.ToArray());

                if (symbol is Value v) ParseIV(v, min, max);
                else throw new UnexpectedSymbolException(symbol);



                if (!iterator.HasValue) throw new Exception("Unexpected EOF");

                symbol = iterator.GetAndNext();

                if (symbol == ListClose.Instance)
                    return new IVsCriteriaNode(min.ToArray(), max.ToArray());

                if (symbol != Comma.Instance)
                    throw new UnexpectedSymbolException(symbol);
            }

            throw new Exception("Unexpected EOF");
        }

        private static void ParseIV(Value v, List<uint> min, List<uint> max)
        {
            if (v.Name == "_")
            {
                min.Add(0);
                max.Add(31);
            }
            else if (v.Name.Contains('-'))
            {
                // xx-xxの形になっているか？
                var s = v.Name.Split('-');
                if (s.Length != 2) throw new Exception("Invalid Value");

                if (!uint.TryParse(s[0], out var l) || 31 < l) throw new Exception("Invalid Value");
                if (!uint.TryParse(s[1], out var r) || 31 < r) throw new Exception("Invalid Value");

                min.Add(l);
                max.Add(r);
            }
            else
            {
                if (!uint.TryParse(v.Name, out var iv) || 31 < iv) throw new Exception("Invalid Value");

                min.Add(iv);
                max.Add(iv);
            }
        }
    }

}
