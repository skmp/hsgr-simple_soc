using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.BL.Entities
{
    public class AssemblerOutput
    {
        public List<Line> Source;
        public Dictionary<int, Opcode> Binary;
        public byte[] Memory;

        public AssemblerOutput(List<Line> source, Dictionary<int, Opcode> binary, byte[] memory)
        {
            Source = source;
            Binary = binary;
            Memory = memory;
        }
    }
}
