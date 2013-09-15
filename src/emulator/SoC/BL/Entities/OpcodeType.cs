using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.BL.Entities
{
    public enum OpcodeType
    {
        ThreeArg_RegRegReg = 1,       // group 1  [draw r0,r1,r2]   
        ThreeArg_RegRegImm4,          // group 1  [beq r0,r1,0x7 | bgt r0,r1,0xA | ba r0,r1,0xE]
        ThreeArg_RegRegImm4label,     // group 1  [beq r0,r1,Label | bgt r0,r1,Label | ba r0,r1,Label]
        TwoArg_RegReg,                // group 2  [mov r0,r1 | add r0,r1 | sub r0,r1 | and r0,r1 | or r0,r1 | xor r0,r1]
        TwoArg_RegImm8,               // group 1  [movh r0,0xFF | movl r0,0xAA, addi r3,0x1, subi r4,10]
        TwoArg_RegImm4,               // group 2  [shl r0,0xF | shr r0,0xC]
        OneArg_Reg,                   // group 3  [not r0 | neg r0]
        OneArg_Imm4,                  // group 4  [wait 0xF]
        OneArg_Imm15,                 // group 0  [j 0x0000]
        OneArg_Imm15label,            // group 0  [j Label]

        TwoArg_RegImm16,              // Pseudo instruction  ([li r0,0xFFAA] -> [movh r0,0xFF - movl r0,0xAA]) | ([jrl r15,0xFFAA] -> [movh r15,0xFF - movl r15,0xAA - jr r15])
        TwoArg_RegImm16label,         // Pseudo instruction  [jrl r15,Label] -> [movh r15,Label_H - movl r15,Label_L - jr r15]
        TwoArg_RegImm16label_h,       // Pseudo instruction  (intermediate type for handling TwoArg_RegImm16label)
        TwoArg_RegImm16label_l,       // Pseudo instruction  (intermediate type for handling TwoArg_RegImm16label)
        OneArg_Imm16,                 // Pseudo instruction  [org 0x0100]

        OneArg_Data16,                // Pseudo instruction  [dw 0xA3BC]
    }
}
