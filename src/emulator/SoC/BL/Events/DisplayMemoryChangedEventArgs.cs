using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SoC.BL.Events
{
    public class DisplayMemoryChangedEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int OldColor { get; set; }
        public int NewColor { get; set; }
    }
}
