using System;
using System.Collections.Generic;
using System.Linq;
using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary
{
    public abstract class Criteria<T>
    {
        public abstract bool Check(T item);
    }

    public class IndividualCriteria : Criteria<GCIndividual>
    {
        private readonly IndividualCriteria[] criteria;
        public override bool Check(GCIndividual item) => criteria.All(_ => _.Check(item));
        protected IndividualCriteria() { }
        internal IndividualCriteria(IndividualCriteria[] arr) => criteria = arr;
    }
    public class IndividualCriteriaBuilder
    {
        private readonly List<IndividualCriteria> criteria;
        public IndividualCriteriaBuilder() => criteria = new List<IndividualCriteria>();

        public void AddIVsCriteria(uint[] minIVs, uint[] maxIVs) => criteria.Add(new IVsCriteria(minIVs.ToArray(), maxIVs.ToArray()));
        public void AddStatsCriteria(uint[] targetStats) => criteria.Add(new StatsCriteria(targetStats.ToArray()));
        public void AddNatureCriteria(params Nature[] targetNatures) => criteria.Add(new NatureCriteria(targetNatures));
        public void AddGenderCriteria(Gender targetGender) => criteria.Add(new GenderCriteria(targetGender));
        public void AddAbilityCriteria(string targetAbility) => criteria.Add(new AbilityCriteria(targetAbility));
        public void AddGCAbilityCriteria(string targetAbility) => criteria.Add(new GCAbilityCriteria(targetAbility));
        public void AddShinyCriteria(uint tsv, ShinyType shinyType) => criteria.Add(new ShinyCriteria(tsv, shinyType));
        public void AddHiddenPowerCriteria(uint minPower) => criteria.Add(new HiddenPowerCriteria(minPower));
        public void AddHiddenPowerTypeCriteria(params PokeType[] targetTypes) => criteria.Add(new HiddenPowerTypeCriteria(targetTypes));

        public IndividualCriteria Build() => new IndividualCriteria(criteria.ToArray());
    }

    class IVsCriteria : IndividualCriteria
    {
        private readonly uint[] minIVs;
        private readonly uint[] maxIVs;

        public override bool Check(GCIndividual item)
        {
            for (int i = 0; i < 6; i++)
                if (item.IVs[i] < minIVs[i] || maxIVs[i] < item.IVs[i]) return false;
            return true;
        }
        internal IVsCriteria(uint[] minIVs, uint[] maxIVs)
        {
            this.minIVs = minIVs;
            this.maxIVs = maxIVs;
        }
    }

    class StatsCriteria : IndividualCriteria
    {
        private readonly int[] indexes;
        private readonly uint[] targetStats;

        public override bool Check(GCIndividual item) => indexes.All(_ => targetStats[_] == item.Stats[_]);

        internal StatsCriteria(uint[] targetStats)
        {
            this.indexes = Enumerable.Range(0, 6).Where(_ => targetStats[_] != 0).ToArray();
            this.targetStats = targetStats;
        }
    }

    class NatureCriteria : IndividualCriteria
    {
        private readonly bool[] checkList = new bool[25];
        public override bool Check(GCIndividual item) => checkList[(uint)item.Nature];
        internal NatureCriteria(Nature[] natures) { foreach (var nature in natures) if (nature != Nature.other) checkList[(uint)nature] = true; }
    }

    class GenderCriteria : IndividualCriteria
    {
        private readonly Gender gender;
        public override bool Check(GCIndividual item) => item.Gender == gender;
        internal GenderCriteria(Gender gender) => this.gender = gender;
    }

    class AbilityCriteria : IndividualCriteria
    {
        private readonly string ability;
        public override bool Check(GCIndividual item) => item.Ability == ability;
        internal AbilityCriteria(string ability) => this.ability = ability;
    }
    class GCAbilityCriteria : IndividualCriteria
    {
        private readonly string ability;
        public override bool Check(GCIndividual item) => item.Ability == ability;
        internal GCAbilityCriteria(string ability) => this.ability = ability;
    }

    class ShinyCriteria : IndividualCriteria
    {
        private readonly ShinyType shinyType;
        private readonly uint tsv;
        public override bool Check(GCIndividual item) => (item.GetShinyType(tsv) & shinyType) != ShinyType.NotShiny;
        internal ShinyCriteria(uint tsv, ShinyType shinyType)
        {
            this.shinyType = shinyType;
            this.tsv = tsv;
        }
    }

    class HiddenPowerCriteria : IndividualCriteria
    {
        private readonly uint minPower;
        public override bool Check(GCIndividual item) => minPower <= item.HiddenPower;
        internal HiddenPowerCriteria(uint min) => minPower = min;
    }

    class HiddenPowerTypeCriteria : IndividualCriteria
    {
        private readonly PokeType targetType;
        public override bool Check(GCIndividual item) => (item.HiddenPowerType & targetType) != 0;
        internal HiddenPowerTypeCriteria(PokeType[] pokeTypes)
        {
            var t = PokeType.Non;
            foreach (var type in pokeTypes)
                t |= type;

            targetType = t;
        }
    }

}
