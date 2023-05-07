using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.GCLCG;

namespace PokemonCoRNGLibrary
{
    public class DarkPokemonGenerator : IGeneratable<RNGResult<GCIndividual>>
    {
        private readonly GCSlot _slot;
        private readonly FixedSlot[] _preGeneratePokemons;

        public RNGResult<GCIndividual> Generate(uint seed)
        {
            var head = seed;
            var dummyTSV = seed.GetRand() ^ seed.GetRand();
            foreach (var pokemon in _preGeneratePokemons)
                seed.Used(pokemon, dummyTSV);

            var res = _slot.Generate(ref seed, dummyTSV);

            return new RNGResult<GCIndividual>(res, head, seed);
        }

        // これは別のオブジェクトに切り出すべきなのでは…？
        public IEnumerable<(uint Seed, GCIndividual Individual)> CalcBack(uint h, uint a, uint b, uint c, uint d, uint s, bool deduplication = false)
        {
            foreach (var genSeed in SeedFinder.FindGeneratingSeed(h, a, b, c, d, s, false))
            {
                var stack = new Stack<(int Index, CalcBackCell Cell)>();
                stack.Push((_preGeneratePokemons.Length, CalcBackCell.CreateCell(_slot, genSeed)));
                var loopBreak = false;
                while (!loopBreak && stack.Count > 0)
                {
                    (var index, var cell) = stack.Pop();
                    if (index-- == 0)
                    {
                        // tsv判定.
                        if (!cell.CheckTSVGeneration()) continue;

                        yield return cell.GetResult();

                        loopBreak = deduplication; // 重複を除く場合はloopBreakをtrueにしてループを抜ける.

                        continue;
                    }
                    foreach (var _c in cell.GetGeneratableCell(_preGeneratePokemons[index]))
                        stack.Push((index, _c));
                }
            }
        }

        public DarkPokemonGenerator(GCSlot slot, FixedSlot[] preGeneratePokemons = null)
        {
            _slot = slot;
            _preGeneratePokemons = preGeneratePokemons?.ToArray() ?? new FixedSlot[0];
        }
    }
}
