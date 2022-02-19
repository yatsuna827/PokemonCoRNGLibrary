using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    class SymbolIterator
    {
        private readonly IEnumerator<Symbol> enumerator;
        private readonly List<Symbol> usedSymbols = new List<Symbol>();

        public string GetUsedSymbols() => string.Join("", usedSymbols);

        public Symbol Peek { get => enumerator.Current; }
        public bool HasValue { get; private set; }
        public Symbol GetAndNext()
        {
            if(enumerator.Current != null)
                usedSymbols.Add(enumerator.Current);
            var value = enumerator.Current;
            HasValue = enumerator.MoveNext();
            return value;
        }
        public void MoveNext()
        {
            if (enumerator.Current != null)
                usedSymbols.Add(enumerator.Current);
            HasValue = enumerator.MoveNext();
        }

        public SymbolIterator(IEnumerator<Symbol> from)
        {
            enumerator = from;
            MoveNext();
        }
    }

}
