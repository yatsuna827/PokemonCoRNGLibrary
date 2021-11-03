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

    #region IndividualCriteria
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

        private IndividualCriteriaBuilder AddCriteria(IndividualCriteria c) { criteria.Add(c); return this; }

        public IndividualCriteriaBuilder AddIVsCriteria(uint[] minIVs, uint[] maxIVs) => AddCriteria(new IVsCriteria(minIVs.ToArray(), maxIVs.ToArray()));
        public IndividualCriteriaBuilder AddStatsCriteria(uint[] targetStats) => AddCriteria(new StatsCriteria(targetStats.ToArray()));
        public IndividualCriteriaBuilder AddNatureCriteria(params Nature[] targetNatures) => AddCriteria(new NatureCriteria(targetNatures));
        public IndividualCriteriaBuilder AddGenderCriteria(Gender targetGender) => AddCriteria(new GenderCriteria(targetGender));
        public IndividualCriteriaBuilder AddAbilityCriteria(string targetAbility) => AddCriteria(new AbilityCriteria(targetAbility));
        public IndividualCriteriaBuilder AddGCAbilityCriteria(string targetAbility) => AddCriteria(new GCAbilityCriteria(targetAbility));
        public IndividualCriteriaBuilder AddShinyCriteria(uint tsv, ShinyType shinyType) => AddCriteria(new ShinyCriteria(tsv, shinyType));
        public IndividualCriteriaBuilder AddHiddenPowerCriteria(uint minPower) => AddCriteria(new HiddenPowerCriteria(minPower));
        public IndividualCriteriaBuilder AddHiddenPowerTypeCriteria(params PokeType[] targetTypes) => AddCriteria(new HiddenPowerTypeCriteria(targetTypes));

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
    #endregion

    #region StarterCriteria
    public class CoStarterCriteria : Criteria<CoStarterResult>
    {
        private readonly CoStarterCriteria[] criteria;
        public override bool Check(CoStarterResult item) => criteria.All(_ => _.Check(item));
        protected CoStarterCriteria() { }
        internal CoStarterCriteria(CoStarterCriteria[] arr) => criteria = arr;
    }

    public class CoStarterCriteriaBuilder
    {
        private readonly List<CoStarterCriteria> criteria;
        public CoStarterCriteriaBuilder() => criteria = new List<CoStarterCriteria>();

        private CoStarterCriteriaBuilder AddCriteria(CoStarterCriteria c) { criteria.Add(c); return this; }

        public CoStarterCriteriaBuilder AddTIDCriteria(uint tid) 
            => AddCriteria(new TIDCriteria(tid));

        public CoStarterCriteriaBuilder AddPIDCriteria(uint pid, ShinyType shinyType = ShinyType.Square | ShinyType.Star) 
            => AddCriteria(new PIDCriteria(pid, shinyType));

        public CoStarterCriteriaBuilder AddUmbreonCriteria(IndividualCriteria criteria)
            => AddCriteria(new UmbreonCriteria(criteria));

        public CoStarterCriteriaBuilder AddEspeonCriteria(IndividualCriteria criteria)
            => AddCriteria(new EspeonCriteria(criteria));

        public CoStarterCriteria Build() => new CoStarterCriteria(criteria.ToArray());
    }

    class SeedCriteria : CoStarterCriteria
    {
        public override bool Check(CoStarterResult item)
        {
            return base.Check(item);
        }
    }

    class TIDCriteria : CoStarterCriteria
    {
        private readonly uint tid;
        public override bool Check(CoStarterResult item)
            => item.TID == tid;

        public TIDCriteria(uint tid) 
            => this.tid = tid;
    }

    class PIDCriteria : CoStarterCriteria
    {
        private readonly uint psv;
        private readonly ShinyType shinyType;
        public override bool Check(CoStarterResult item)
        {
            var sv = item.TID ^ item.SID ^ psv;

            if (shinyType == ShinyType.NotShiny) return sv >= 8;

            // 菱形
            if ((shinyType & ShinyType.Square) != 0 && sv == 0)
                return true;

            // 星型
            if ((shinyType & ShinyType.Star) != 0 && sv != 0 && sv < 8)
                return true;

            return false;
        }

        public PIDCriteria(uint pid, ShinyType shinyType)
            => (this.psv, this.shinyType) = ((pid >> 16) ^ (pid & 0xFFFF), shinyType);
    }

    class UmbreonCriteria : CoStarterCriteria
    {
        private readonly IndividualCriteria criteria;
        public override bool Check(CoStarterResult item)
            => criteria.Check(item.Umbreon);

        public UmbreonCriteria(IndividualCriteria criteria)
            => this.criteria = criteria;
    }

    class EspeonCriteria : CoStarterCriteria
    {
        private readonly IndividualCriteria criteria;
        public override bool Check(CoStarterResult item)
            => criteria.Check(item.Espeon);

        public EspeonCriteria(IndividualCriteria criteria)
            => this.criteria = criteria;
    }
    #endregion
}
