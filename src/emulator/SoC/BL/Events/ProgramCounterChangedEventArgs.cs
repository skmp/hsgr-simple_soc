using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoC.BL.Entities;

namespace SoC.BL.Events
{
    public class ProgramCounterChangedEventArgs : EventArgs
    {
        public int OldProgramCounter { get; set; }
        public int NewProgramCounter { get; set; }
        public Line OldLine { get; set; }
        public Line NewLine { get; set; }
    }
}
