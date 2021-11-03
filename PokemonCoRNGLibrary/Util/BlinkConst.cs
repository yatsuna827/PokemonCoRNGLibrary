﻿using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.GCLCG;
using System.Runtime.InteropServices;

namespace PokemonCoRNGLibrary
{
    // 瞬き閾値を正確に指定するために使う.
    [StructLayout(LayoutKind.Explicit)]
    class FLOAT
    {
        [FieldOffset(0)] public readonly float Dec;
        [FieldOffset(0)] public readonly uint Integer;
        public FLOAT(uint _) => Integer = _;
    }

    public static class BlinkConst
    {
        // レガシー.
        internal static readonly float[] blinkThresholds_float = new float[172]
        {
            new FLOAT(0x00000000).Dec,
            new FLOAT(0x3A2D03DA).Dec,
            new FLOAT(0x3AAB4476).Dec,
            new FLOAT(0x3AFE479A).Dec,
            new FLOAT(0x3B27C5AC).Dec,
            new FLOAT(0x3B4F87DA).Dec,
            new FLOAT(0x3B766A55).Dec,
            new FLOAT(0x3B8E368F).Dec,
            new FLOAT(0x3BA0C81B).Dec,
            new FLOAT(0x3BB2E9CD).Dec,
            new FLOAT(0x3BC49BA6).Dec,
            new FLOAT(0x3BD5DDA6).Dec,
            new FLOAT(0x3BE6AFCE).Dec,
            new FLOAT(0x3BF7121C).Dec,
            new FLOAT(0x3C038248).Dec,
            new FLOAT(0x3C0B4397).Dec,
            new FLOAT(0x3C12CCF7).Dec,
            new FLOAT(0x3C1A1E6C).Dec,
            new FLOAT(0x3C2137F5).Dec,
            new FLOAT(0x3C281990).Dec,
            new FLOAT(0x3C2EC340).Dec,
            new FLOAT(0x3C353501).Dec,
            new FLOAT(0x3C3B6ED7).Dec,
            new FLOAT(0x3C4170C1).Dec,
            new FLOAT(0x3C473ABC).Dec,
            new FLOAT(0x3C4CCCCE).Dec,
            new FLOAT(0x3C5226F1).Dec,
            new FLOAT(0x3C574929).Dec,
            new FLOAT(0x3C5C3374).Dec,
            new FLOAT(0x3C60E5D1).Dec,
            new FLOAT(0x3C656043).Dec,
            new FLOAT(0x3C69A2C7).Dec,
            new FLOAT(0x3C6DAD60).Dec,
            new FLOAT(0x3C71800A).Dec,
            new FLOAT(0x3C751AC9).Dec,
            new FLOAT(0x3C787D9D).Dec,
            new FLOAT(0x3C7BA884).Dec,
            new FLOAT(0x3C7E9B7D).Dec,
            new FLOAT(0x3C80AB45).Dec,
            new FLOAT(0x3C81ECD5).Dec,
            new FLOAT(0x3C83126F).Dec,
            new FLOAT(0x3C841C13).Dec,
            new FLOAT(0x3C8509C0).Dec,
            new FLOAT(0x3C85DB77).Dec,
            new FLOAT(0x3C869138).Dec,
            new FLOAT(0x3C872B03).Dec,
            new FLOAT(0x3C87A8D6).Dec,
            new FLOAT(0x3C880AB4).Dec,
            new FLOAT(0x3C88509C).Dec,
            new FLOAT(0x3C887A8E).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            new FLOAT(0x3C888889).Dec,
            1.0f,
            1.0f
        };

        internal static readonly uint[] blinkThresholds = new uint[172]
        {
            0x0001, 0x002C, 0x0056, 0x0080, 0x00A8, 0x00D0, 0x00F7, 0x011D, 0x0142, 0x0166, 
            0x018A, 0x01AC, 0x01CE, 0x01EF, 0x020F, 0x022E, 0x024C, 0x0269, 0x0285, 0x02A1, 
            0x02BC, 0x02D5, 0x02EE, 0x0306, 0x031D, 0x0334, 0x0349, 0x035E, 0x0371, 0x0384, 
            0x0396, 0x03A7, 0x03B7, 0x03C7, 0x03D5, 0x03E2, 0x03EF, 0x03FB, 0x0406, 0x0410, 
            0x0419, 0x0421, 0x0429, 0x042F, 0x0435, 0x043A, 0x043E, 0x0441, 0x0443, 0x0444, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x10000, 0x10000,
        };
        // rand < threshなら瞬きする.
        internal static readonly uint[] blinkThresholds_even = new uint[86]
        {
            0x0001, 0x0056, 0x00A8, 0x00F7, 0x0142, 0x018A, 0x01CE, 0x020F, 0x024C, 0x0285, 
            0x02BC, 0x02EE, 0x031D, 0x0349, 0x0371, 0x0396, 0x03B7, 0x03D5, 0x03EF, 0x0406, 
            0x0419, 0x0429, 0x0435, 0x043E, 0x0443, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 
            0x0445, 0x0445, 0x0445, 0x0445, 0x0445, 0x10000
        };
    }
}
