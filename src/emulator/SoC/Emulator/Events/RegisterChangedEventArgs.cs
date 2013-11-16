using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoC.Assembler.Entities;

namespace SoC.Emulator.Events
{
    public class RegisterChangedEventArgs : EventArgs
    {
        public Register Register { get; set; }
    }
}
