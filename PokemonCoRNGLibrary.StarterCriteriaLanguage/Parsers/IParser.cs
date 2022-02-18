namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    interface IParser<out T>
    {
        T Parse(SymbolIterator iterator);
    }
}
