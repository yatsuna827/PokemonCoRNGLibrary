using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonCoRNGLibrary.AdvanceSource
{
    public interface ISourceCounter
    {
        uint SimulateNextFrame(uint seed);
    }
}
