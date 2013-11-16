using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoC.Assembler.Entities;

namespace SoC.Emulator.Events
{
    public class ProgramCounterChangedEventArgs : EventArgs
    {
        public int OldProgramCounter { get; set; }
        public int NewProgramCounter { get; set; }
        public Line OldLine { get; set; }
        public Line NewLine { get; set; }
    }
}
