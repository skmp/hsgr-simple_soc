using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.BL.Entities
{
    public class RomItem
    {
        public int Address { get; private set; }
        public Opcode Opcode { get; private set; }

        public RomItem(int address, Opcode opcode)
        {
            Address = address;
            Opcode = opcode;
        }
    }
}
