using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    class SymbolIterator
    {
        private readonly IEnumerator<Symbol> enumerator;

        public Symbol Peek { get => enumerator.Current; }
        public bool HasValue { get; private set; }
        public Symbol GetAndNext()
        {
            var value = enumerator.Current;
            HasValue = enumerator.MoveNext();
            return value;
        }
        public void MoveNext()
        {
            HasValue = enumerator.MoveNext();
        }

        public SymbolIterator(IEnumerator<Symbol> from)
        {
            enumerator = from;
            MoveNext();
        }
    }

}
