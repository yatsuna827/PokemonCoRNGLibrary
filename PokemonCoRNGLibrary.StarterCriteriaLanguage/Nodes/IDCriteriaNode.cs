using System;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonCoRNGLibrary.Criteria.Starter;


namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    public interface IDCriteriaNode : ICriteriaNode<CoStarterResult> { }
    public class TIDCriteriaNode : IDCriteriaNode
    {
        private readonly uint[] tids;
        public TIDCriteriaNode(uint[] tids) => this.tids = tids;

        public ICriteria<CoStarterResult> Build()
        {
            if (tids.Length == 0) throw new Exception("空のリストは許可されていません");

            return new TIDCriteria(tids.ToArray());
        }
        public string Serialize()
        {
            if (tids.Length == 0) return "{ tid: [] }";
            if (tids.Length == 1) return $"{{ tid: {tids[0]:d5} }}";

            return $"{{ tid: [ {string.Join(", ", tids.Select(_ => $"{_:d5}"))} ] }}";
        }
    }
    public class ShinyCriteriaNode : IDCriteriaNode
    {
        private readonly uint[] pids;
        public ShinyCriteriaNode(uint[] pids) => this.pids = pids;

        public ICriteria<CoStarterResult> Build()
        {
            if (pids.Length == 0) throw new Exception("空のリストは許可されていません");

            return new ShinyTSVCriteria(pids.ToArray());
        }

        public string Serialize()
        {
            if (pids.Length == 0) return "{ shiny: [] }";
            if (pids.Length == 1) return $"{{ shiny: {pids[0]:X8} }}";

            return $"{{ shiny: [ {string.Join(", ", pids.Select(_ => $"{_:X8}"))} ] }}";
        }
    }
    public class SquareCriteriaNode : IDCriteriaNode
    {
        private readonly uint[] pids;
        public SquareCriteriaNode(uint[] pids) => this.pids = pids;

        public ICriteria<CoStarterResult> Build()
        {
            if (pids.Length == 0) throw new Exception("空のリストは許可されていません");

            return new SquareTSVCriteria(pids.ToArray());
        }

        public string Serialize()
        {
            if (pids.Length == 0) return "{ square: [] }";
            if (pids.Length == 1) return $"{{ square: {pids[0]:X8} }}";

            return $"{{ square: [ {string.Join(", ", pids.Select(_ => $"{_:X8}"))} ] }}";
        }
    }
    public class StarCriteriaNode : IDCriteriaNode
    {
        private readonly uint[] pids;
        public StarCriteriaNode(uint[] pids) => this.pids = pids;

        public ICriteria<CoStarterResult> Build()
        {
            if (pids.Length == 0) throw new Exception("空のリストは許可されていません");

            return new StarTSVCriteria(pids.ToArray());
        }

        public string Serialize()
        {
            if (pids.Length == 0) return "{ star: [] }";
            if (pids.Length == 1) return $"{{ star: {pids[0]:X8} }}";

            return $"{{ star: [ {string.Join(", ", pids.Select(_ => $"{_:X8}"))} ] }}";
        }
    }
}
