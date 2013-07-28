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

        public AssemblerOutput(List<Line> source, Dictionary<int, Opcode> binary)
        {
            Source = source;
            Binary = binary;
        }
    }
}
