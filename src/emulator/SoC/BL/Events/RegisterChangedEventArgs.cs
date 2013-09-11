using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoC.BL.Entities;

namespace SoC.BL.Events
{
    public class RegisterChangedEventArgs : EventArgs
    {
        public Register Register { get; set; }
    }
}
