using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.Device
{
    interface IDevice
    {
        // name
        string DeviceName { get; }

        // capablities
        bool CanConfigure { get; }
        bool CanReset { get; }
        bool CanHalt { get; }
        bool CanResume { get; }
        bool CanStep { get; }
        bool CanDisplay { get; }

        // do
        void Configure();
        void Instantiate();
        byte WriteCommand(byte command);
        void Destroy();

        // extensions
        void ShowDisplay();
    }
}
