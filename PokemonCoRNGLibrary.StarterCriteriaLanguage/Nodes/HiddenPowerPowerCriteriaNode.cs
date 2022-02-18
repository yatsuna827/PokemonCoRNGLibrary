using System;
using PokemonPRNG.LCG32;
using PokemonCoRNGLibrary.Criteria;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    public class HiddenPowerPowerCriteriaNode : ICriteriaNode<GCIndividual>
    {
        private readonly uint minPower;

        public ICriteria<GCIndividual> Build()
            => new HiddenPowerPowerCriteria(minPower);

        public string Serialize() => $"{minPower}";

        public HiddenPowerPowerCriteriaNode(uint minPower)
        {
            if (minPower < 30 || 70 < minPower) throw new ArgumentException("Invalid Argument");

            this.minPower = minPower;
        }
    }
}
