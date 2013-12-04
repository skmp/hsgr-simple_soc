using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.Device
{
    class Ice
    {
        // Register Read/Write
        #region public static void WriteRegister(IDevice device, int register, ushort value)
        public static void WriteRegister(IDevice device, int register, ushort value)
        {
            byte req;

            req = (byte)(0x40 | (value & 0x0f));          //4:data[0]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x50 | ((value >> 4) & 0x0f));   //5:data[1]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x60 | ((value >> 8) & 0x0f));   //6:data[2]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x70 | ((value >> 12) & 0x0f));  //7:data[3]                   -> echo
            WriteCommandWithEchoCheck(device, req);

            req = (byte)(0x90 | (register & 0x0f));       //9:n  -> reg[n] = data       -> echo
            WriteCommandWithEchoCheck(device, req);
        }
        #endregion
        #region public static ushort ReadRegister(IDevice device, int register)
        public static ushort ReadRegister(IDevice device, int register)
        {
            ushort value = 0;
            byte req;
            byte resp;

            req = (byte)(0xa0 | (register & 0x0f));       //10:n -> data = reg[n]       -> echo
            WriteCommandWithEchoCheck(device, req);

            req = (byte)(0x84);                           //8:4 -> read data[0]         -> 4:data[0]
            resp = WriteCommandWithResult(device, req);
            value = resp;
            req = (byte)(0x85);                           //8:5 -> read data[1]         -> 5:data[1]
            resp = WriteCommandWithResult(device, req);
            value = (ushort)(value | (resp << 4));
            req = (byte)(0x86);                           //8:6 -> read data[2]         -> 6:data[2]
            resp = WriteCommandWithResult(device, req);
            value = (ushort)(value | (resp << 8));
            req = (byte)(0x87);                           //8:7 -> read data[3]         -> 7:data[3]
            resp = WriteCommandWithResult(device, req);
            value = (ushort)(value | (resp << 12));

            return value;
        }
        #endregion

        // Memory Read/Write
        #region public static ushort ReadAddresValue(IDevice device, int address)
        public static ushort ReadAddresValue(IDevice device, int address)
        {
            ushort value = 0;
            byte req;
            byte resp;

            req = (byte)(0x00 | (address & 0x0f));          //0:addr[0]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x10 | ((address >> 4) & 0x0f));   //1:addr[1]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x20 | ((address >> 8) & 0x0f));   //2:addr[2]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x30 | ((address >> 12) & 0x0f));  //3:addr[3]                   -> echo
            WriteCommandWithEchoCheck(device, req);

            req = (byte)(0x88);                             //8:8 -> read mem to data     -> echo
            WriteCommandWithEchoCheck(device, req);

            req = (byte)(0x84);                             //8:4 -> read data[0]         -> 4:data[0]
            resp = WriteCommandWithResult(device, req);
            value = resp;
            req = (byte)(0x85);                             //8:5 -> read data[1]         -> 5:data[1]
            resp = WriteCommandWithResult(device, req);
            value = (ushort)(value | (resp << 4));
            req = (byte)(0x86);                             //8:6 -> read data[2]         -> 6:data[2]
            resp = WriteCommandWithResult(device, req);
            value = (ushort)(value | (resp << 8));
            req = (byte)(0x87);                             //8:7 -> read data[3]         -> 7:data[3]
            resp = WriteCommandWithResult(device, req);
            value = (ushort)(value | (resp << 12));

            return value;
        }
        #endregion
        #region public static void WriteAddresValue(IDevice device, int address, ushort value)
        public static void WriteAddresValue(IDevice device, int address, ushort value)
        {
            byte req;

            req = (byte)(0x00 | (address & 0x0f));          //0:addr[0]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x10 | ((address >> 4) & 0x0f));   //1:addr[1]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x20 | ((address >> 8) & 0x0f));   //2:addr[2]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x30 | ((address >> 12) & 0x0f));  //3:addr[3]                   -> echo
            WriteCommandWithEchoCheck(device, req);

            req = (byte)(0x40 | (value & 0x0f));            //4:data[0]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x50 | ((value >> 4) & 0x0f));     //5:data[1]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x60 | ((value >> 8) & 0x0f));     //6:data[2]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x70 | ((value >> 12) & 0x0f));    //7:data[3]                   -> echo
            WriteCommandWithEchoCheck(device, req);

            req = (byte)(0x89);                             //8:9 -> write data to mem    -> echo
            WriteCommandWithEchoCheck(device, req);

            return;
        }
        #endregion

        // Commands
        #region public static void UnsetResetSignal(IDevice device)
        public static void UnsetResetSignal(IDevice device)
        {
            byte req;

            req = (byte)(0xb0);                             //11:0 -> reset = 0           -> echo
            WriteCommandWithEchoCheck(device, req);
        }
        #endregion
        #region public static void SetResetSignal(IDevice device)
        public static void SetResetSignal(IDevice device)
        {
            byte req;

            req = (byte)(0xb1);                             //11:1 -> reset = 1           -> echo
            WriteCommandWithEchoCheck(device, req);
        }
        #endregion
        #region public static void Halt(IDevice device)
        public static void Halt(IDevice device)
        {
            byte req;

            req = (byte)(0xb2);                             //11:2 -> halt                -> echo
            WriteCommandWithEchoCheck(device, req);
        }
        #endregion
        #region public static void Resume(IDevice device)
        public static void Resume(IDevice device)
        {
            byte req;

            req = (byte)(0xb3);                             //11:3 -> resume              -> echo
            WriteCommandWithEchoCheck(device, req);
        }
        #endregion
        #region public static void Step(IDevice device)
        public static void Step(IDevice device)
        {
            byte req;

            req = (byte)(0xb4);                             //11:4 -> step                -> echo
            WriteCommandWithEchoCheck(device, req);
        }
        #endregion
        #region public static void SetProgramCounter(IDevice device, int value)
        public static void SetProgramCounter(IDevice device, int value)
        {
            byte req;

            req = (byte)(0x40 | (value & 0x0f));            //4:data[0]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x50 | ((value >> 4) & 0x0f));     //5:data[1]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x60 | ((value >> 8) & 0x0f));     //6:data[2]                   -> echo
            WriteCommandWithEchoCheck(device, req);
            req = (byte)(0x70 | ((value >> 12) & 0x0f));    //7:data[3]                   -> echo
            WriteCommandWithEchoCheck(device, req);

            req = (byte)(0xb5);                             //11:5 -> pc = data           -> echo
            WriteCommandWithEchoCheck(device, req);
        }
        #endregion
        #region public static int ReadProgramCounter(IDevice device)
        public static int ReadProgramCounter(IDevice device)
        {
            int value = 0;
            byte req;
            byte resp;

            req = (byte)(0xb6);                             //11:6 -> data = pc           -> echo
            WriteCommandWithEchoCheck(device, req);

            req = (byte)(0x84);                             //8:4 -> read data[0]         -> 4:data[0]
            resp = WriteCommandWithResult(device, req);
            value = resp;
            req = (byte)(0x85);                             //8:5 -> read data[1]         -> 5:data[1]
            resp = WriteCommandWithResult(device, req);
            value = (ushort)(value | (resp << 4));
            req = (byte)(0x86);                             //8:6 -> read data[2]         -> 6:data[2]
            resp = WriteCommandWithResult(device, req);
            value = (ushort)(value | (resp << 8));
            req = (byte)(0x87);                             //8:7 -> read data[3]         -> 7:data[3]
            resp = WriteCommandWithResult(device, req);
            value = (ushort)(value | (resp << 12));

            return value;
        }
        #endregion

        // Private Helpers
        #region private static void WriteCommandWithEchoCheck(IDevice device, byte command)
        private static void WriteCommandWithEchoCheck(IDevice device, byte command)
        {
            byte resp = device.WriteCommand(command);
            if (command != resp)
                throw new Exception("Communication Error");
        }
        #endregion
        #region private static byte WriteCommandWithResult(IDevice device, byte command)
        private static byte WriteCommandWithResult(IDevice device, byte command)
        {
            byte resp = device.WriteCommand(command);
            if ((command & 0x0f) != (resp >> 4))
                throw new Exception("Communication Error");

            return (byte)(resp & 0x0f);
        }
        #endregion
    }
}
