﻿using PokemonPRNG.LCG32;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary.Criteria
{

    public class HiddenPowerPowerCriteria : ICriteria<GCIndividual>
    {
        private readonly uint minPower;
        public bool CheckConditions(GCIndividual item) => minPower <= item.HiddenPower;
        public HiddenPowerPowerCriteria(uint min) => minPower = min;
    }

    public class HiddenPowerTypeCriteria : ICriteria<GCIndividual>
    {
        private readonly PokeType targetType;
        public bool CheckConditions(GCIndividual item) => (item.HiddenPowerType & targetType) != 0;
        public HiddenPowerTypeCriteria(PokeType[] pokeTypes)
        {
            var t = PokeType.Non;
            foreach (var type in pokeTypes)
                t |= type;

            targetType = t;
        }
    }

}