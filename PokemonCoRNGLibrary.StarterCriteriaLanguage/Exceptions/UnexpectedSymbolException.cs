using System;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    public class UnexpectedSymbolException : Exception
    {
        internal UnexpectedSymbolException(Symbol s)
            : base($"予期しないシンボルです; {s}")
        {

        }
    }
}
