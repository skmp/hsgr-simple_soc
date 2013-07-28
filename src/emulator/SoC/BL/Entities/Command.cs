using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.BL.Entities
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
        neg,
        and,
        or,
        xor,
        not,
        shl,
        shr,
        sar,
        read_u8,
        read_s8,
        read_16,
        write_8,
        write_16,
        jr,
        wait,

        org,
        li,
        jrl,
        db,
        dw,
    }
}