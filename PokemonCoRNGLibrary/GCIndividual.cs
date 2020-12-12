using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;
using PokemonStandardLibrary.PokeDex.Gen3;

namespace PokemonCoRNGLibrary
{
    // なぜ所謂XD特性を追加した. ついでに個体生成seedも載せた.
    public class GCIndividual : Pokemon.Individual
    {
        public new static readonly GCIndividual Empty = Pokemon.GetPokemon("Dummy").GetIndividual(1, new uint[6], 0, 0);
        public string XDAbility { get; private set; }
        public uint RepresentativeSeed { get; private set; }
        public bool ShinySkipped { get; private set; }


        public GCIndividual SetRepSeed(uint seed) { RepresentativeSeed = seed; return this; }
        public GCIndividual SetShinySkipped(bool skipped) { ShinySkipped = skipped; return this; }

        internal GCIndividual(Pokemon.Species species, uint pid, uint[] ivs, uint lv, uint xdability) : base(species, pid, ivs, lv)
        {
            this.XDAbility = species.Ability[xdability];
        }
    }
    static class GCExtension
    {
        public static GCIndividual GetIndividual(this Pokemon.Species species, uint Lv, uint[] IVs, uint PID, uint ability)
            => new GCIndividual(species, PID, IVs, Lv, ability);
    }
}
