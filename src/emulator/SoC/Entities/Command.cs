using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.Entities
{
    public enum Command
    {
        j,
        draw,
        movh,
        movl,
        beq,
        bgt,
        ba,
        mov,
        add,
        sub,
        and,
        or,
        xor,
        shl,
        shr,
        not,
        neg,
        readm,
        writem,
        jr,
        wait,
        org,
        li,
        jrl,
    }
}