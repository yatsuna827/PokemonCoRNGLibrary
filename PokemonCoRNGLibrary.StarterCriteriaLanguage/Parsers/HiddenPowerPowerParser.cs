using System;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    class HiddenPowerPowerParser : IParser<HiddenPowerPowerCriteriaNode>
    {
        public HiddenPowerPowerCriteriaNode Parse(SymbolIterator iterator)
        {
            if (!iterator.HasValue) throw new Exception("Unexpected EOF");

            var symbol = iterator.GetAndNext();
            if (symbol is Value v)
            {
                if (!uint.TryParse(v.Name, out var p) || p < 30 || 70 < p)
                    throw new Exception("Invalud Value");

                return new HiddenPowerPowerCriteriaNode(p);
            }
            else throw new UnexpectedSymbolException(symbol);
        }
    }
}
