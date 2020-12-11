using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;
using PokemonPRNG.LCG32.GCLCG;
using PokemonStandardLibrary.PokeDex.Gen3;
using PokemonStandardLibrary.CommonExtension;

namespace PokemonCoRNGLibrary
{
    class CalcBackCell
    {
        private readonly uint seed;
        private readonly CalcBackCore core;
        private readonly CalcBackCell prevCell;
        private readonly uint tsvCondition;
        private uint psvCondition;

        private CalcBackCell() { }
        private CalcBackCell(GCSlot slot, uint seed)
        {
            this.prevCell = RootCell.Instance;
            this.core = new CalcBackCore(seed, slot);
            this.seed = seed;
            this.tsvCondition = 0x10000;

        }
        private CalcBackCell(uint seed, uint tsv)
        {
            this.prevCell = this;
            this.core = prevCell.core;
            this.seed = seed;
            this.tsvCondition = tsv;
        }

        public static CalcBackCell CreateCell(GCSlot slot, uint seed) => new CalcBackCell(slot, seed);

        public IEnumerable<CalcBackCell> GetGeneratableCell(GCSlot slot)
        {
            var seed = this.seed;
            var tsv = this.tsvCondition;

            var fixedNature = (uint)slot.fixedNature;
            var fixedGender = slot.fixedGender;

            // PIDのチェック.
            // PIDが条件を満たしていなければyield break.
            {
                var lid = seed >> 16;
                var hid = seed.Back() >> 16;
                var pid = hid << 16 | lid;
                var psv = hid ^ lid;

                if ((pid % 25) != fixedNature) yield break; // 性格不一致
                if (pid.GetGender(slot.pokemon.GenderRatio) != fixedGender) yield break; // 性別不一致
                if (tsv < 0x10000 && (psv ^ tsv) < 8) yield break; // TSVに条件がついているが, そのTSVだと色回避に引っかかってしまう場合

                this.psvCondition = psv;
            }

            // 逆算
            var currentCondition = this.tsvCondition;
            while (true)
            {
                // 条件を満たすPIDに当たるまで, seedを返し続ける.
                yield return new CalcBackCell(seed.PrevSeed(6), currentCondition);

                var lid = seed.Back() >> 16;
                var hid = seed.Back() >> 16;
                var pid = hid << 16 | lid;
                if (pid % 25 == fixedNature && pid.GetGender(slot.pokemon.GenderRatio) == fixedGender)
                {
                    // 性格・性別が一致するPIDに当たったら
                    if (currentCondition < 0x10000 && (lid ^ hid ^ currentCondition) >= 8) yield break; // TSV指定済みで色回避が発生しないなら終了.
                    if (currentCondition == 0x10000)
                    {
                        currentCondition = lid ^ hid; // TSVが指定されていない場合はTSVを指定して続行. (色回避を発生させた場合のみ出現する個体を探す)
                        if (!CheckPSV(currentCondition)) yield break; // prevCellを全部遡って判定する. 既に固定されているPIDが光ってしまうTSVはアウト
                    }
                }
            }
        }

        internal protected virtual bool CheckPSV(uint tsv) => (psvCondition ^ tsv) >= 8 && prevCell.CheckPSV(tsv);
        internal bool CheckTSVGeneration()
        {
            var tsv = (seed >> 16) ^ (seed.PrevSeed() >> 16);
            if (tsvCondition != 0x10000 && (tsv ^ tsvCondition) >= 8) return false;
            return CheckPSV(tsv);
        }

        internal (uint seed, GCIndividual individual) GetResult()
        {
            var slot = core.slot;

            var tsv = (seed >> 16) ^ (seed.PrevSeed() >> 16);
            var skipped = ((core.pid1 >> 16) ^ (core.pid1 & 0xFFFF) ^ tsv) < 8;
            var pid = skipped ? core.pid2 : core.pid1; // 色回避が発生してしまう場合があり、とても悲しい.
            var ind = slot.pokemon.GetIndividual(slot.Lv, core.ivs, pid, core.abilityIndex).SetRepSeed(core.representativeSeed).SetShinySkipped(skipped);

            return (seed, ind);
        }

        class CalcBackCore
        {
            public readonly GCSlot slot;
            public readonly uint[] ivs;
            public readonly uint abilityIndex;
            public readonly uint pid1, pid2;
            public readonly uint representativeSeed;

            public CalcBackCore(uint seed, GCSlot slot)
            {
                this.slot = slot;
                representativeSeed = seed;

                seed.Advance(2);
                ivs = seed.GetIVs();
                abilityIndex = seed.GetRand() & 1;
                var hid = seed.GetRand();
                var lid = seed.GetRand();
                var psv = lid ^ hid;
                pid1 = lid | (hid << 16);
                do
                {
                    hid = seed.GetRand();
                    lid = seed.GetRand();
                    pid2 = lid | (hid << 16);
                } while ((hid ^ lid ^ psv) >= 8);
            }
        }
        class RootCell : CalcBackCell
        {
            internal protected override bool CheckPSV(uint tsv) => true;
            public static readonly RootCell Instance = new RootCell();
        }
    }
}
