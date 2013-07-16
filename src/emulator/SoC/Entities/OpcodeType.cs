using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.Entities
{
    public enum OpcodeType
    {
        ThreeArg_RegRegReg = 1,       // group 1
        ThreeArg_RegRegImm4,          // group 1
        ThreeArg_RegRegImm4label,     // group 1
        TwoArg_RegReg,                // group 2
        TwoArg_RegImm16,              //    Pseudo instruction
        TwoArg_RegImm16label,         //    Pseudo instruction
        TwoArg_RegImm8,               // group 1
        TwoArg_RegImm8_label16h,      //    Pseudo instruction
        TwoArg_RegImm8_label16l,      //    Pseudo instruction
        TwoArg_RegImm4,               // group 2
        OneArg_Reg,                   // group 3
        OneArg_Imm16,                 //    Pseudo instruction
        OneArg_Imm15,                 // group 0
        OneArg_Imm15label,            // group 0
        ZeroArg                       // group 4
    }
}
