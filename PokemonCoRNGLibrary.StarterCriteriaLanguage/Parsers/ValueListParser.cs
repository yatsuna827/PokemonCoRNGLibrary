using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    abstract class ValueListParser
    {
        public uint[] Parse(SymbolIterator iterator)
        {
            if (!iterator.HasValue) throw new Exception("Unexpected EOF");

            var list = new List<uint>();

            var symbol = iterator.GetAndNext();
            if (symbol == ListOpen.Instance)
            {
                while (iterator.HasValue)
                {
                    symbol = iterator.GetAndNext();
                    if (symbol == ListClose.Instance) return list.ToArray();

                    if (symbol is Value v)
                        list.Add(ParseValue(v));
                    else
                        throw new UnexpectedSymbolException(symbol);



                    if (!iterator.HasValue) throw new Exception("Unexpected EOF");

                    symbol = iterator.GetAndNext();
                    if (symbol == ListClose.Instance)
                        return list.ToArray();

                    if (symbol != Comma.Instance)
                        throw new UnexpectedSymbolException(symbol);
                }

                throw new Exception("Unexpected EOF");
            }
            else if (symbol is Value v)
            {
                list.Add(ParseValue(v));

                return list.ToArray();
            }
            else throw new UnexpectedSymbolException(symbol);
        }

        protected abstract uint ParseValue(Value v);
    }
    class DecListParser : ValueListParser
    {
        protected override uint ParseValue(Value v)
        {
            if (v.Name.TryConvertToTID(out var tid)) return tid;

            throw new Exception("Invalid Value");
        }
    }
    class HexListParser : ValueListParser
    {
        protected override uint ParseValue(Value v)
        {
            if (v.Name.TryConvertToPID(out var pid)) return pid;

            throw new Exception("Invalid Value");
        }
    }
}
