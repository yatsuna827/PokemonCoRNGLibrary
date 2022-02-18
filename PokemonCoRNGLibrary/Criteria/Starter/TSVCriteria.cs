using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;

namespace PokemonCoRNGLibrary.Criteria.Starter
{
    public class ShinyTSVCriteria : ICriteria<CoStarterResult>
    {
        private readonly HashSet<uint> psvs;
        public bool CheckConditions(CoStarterResult item)
        {
            var tsv = (item.TID ^ item.SID) >> 3;

            return psvs.Contains(tsv);
        }

        public ShinyTSVCriteria(params uint[] pids)
        {
            var psvs = pids.Select(_ => ((_ & 0xFFFF) ^ (_ >> 16)) >> 3);
            this.psvs = new HashSet<uint>(psvs);
        }
    }

    public class SquareTSVCriteria : ICriteria<CoStarterResult>
    {
        private readonly HashSet<uint> psvs;
        public bool CheckConditions(CoStarterResult item)
        {
            var tsv = (item.TID ^ item.SID);

            return psvs.Contains(tsv);
        }

        public SquareTSVCriteria(params uint[] pids)
        {
            var psvs = pids.Select(_ => ((_ & 0xFFFF) ^ (_ >> 16)));
            this.psvs = new HashSet<uint>(psvs);
        }
    }

    public class StarTSVCriteria : ICriteria<CoStarterResult>
    {
        private readonly HashSet<uint> psvs;
        public bool CheckConditions(CoStarterResult item)
        {
            var tsv = (item.TID ^ item.SID);

            return psvs.Contains(tsv);
        }

        public StarTSVCriteria(params uint[] pids)
        {
            this.psvs = new HashSet<uint>();
            foreach (var psv in pids.Select(_ => ((_ & 0xFFFF) ^ (_ >> 16))))
            {
                var sv = psv & 0xFFF8;
                var rv = psv & 0x7;
                for (uint i = 0; i < 8; i++)
                {
                    if (i != rv) psvs.Add(sv | i);
                }
            }
        }
    }
}
