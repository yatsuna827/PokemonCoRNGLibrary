using PokemonPRNG.LCG32;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    public interface ICriteriaNode<in T>
    {
        ICriteria<T> Build();
        string Serialize();
    }

}
