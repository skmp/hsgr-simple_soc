using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using SoC.Assembler.Entities;
using System.Threading;
using SoC.Emulator.Events;

namespace SoC.Emulator
{
    public delegate void DisplayMemoryChangedEventHandler(object o, DisplayMemoryChangedEventArgs e);
    public delegate void RegisterChangedEventHandler(object o, RegisterChangedEventArgs e);
    public delegate void ProgramCounterChangedEventHandler(object o, ProgramCounterChangedEventArgs e);

    public class EmulatorMain
    {
        public event DisplayMemoryChangedEventHandler DisplayMemoryChanged;
        public event RegisterChangedEventHandler RegisterChanged;
        public event ProgramCounterChangedEventHandler ProgramCounterChanged;

        public void OnDisplayMemoryChanged(DisplayMemoryChangedEventArgs e)
        {
            if (DisplayMemoryChanged != null)
                DisplayMemoryChanged(this, e);
        }
        public void OnRegisterChanged(RegisterChangedEventArgs e)
        {
            if (RegisterChanged != null)
                RegisterChanged(this, e);
        }
        public void OnProgramCounterChanged(ProgramCounterChangedEventArgs e)
        {
            if (ProgramCounterChanged != null)
                ProgramCounterChanged(this, e);
        }


        public EmulatorMain(UInt16[] memory)
        {
            Register = new Register[16];
            for (int i = 0; i < 16; i++)
                Register[i] = new Register(i, 0xffff);

            Memory = memory;

            Display = new byte[256, 256];
            Array.Clear(Display, 0, 256 * 256);

            UInt16 o;
            Opcode op;
            Program = new Dictionary<int, Opcode>();
            for (int address = 0; address < 32768; address += 2)
            {
                o = Convert.ToUInt16((Memory[address + 1] << 8) | Memory[address]);
                op = OpcodeDictionary.Get(o);
                Program.Add(address, op);
            }
        }

        public EmulatorMain(Dictionary<int, Opcode> program)
        {
            Program = program;
            Register = new Register[16];
            for (int i = 0; i < 16; i++)
                Register[i] = new Register(i, 0xffff);

            Memory = new UInt16[16 * 1024]; // 32K memory
            Array.Clear(Memory, 0, 16 * 1024);

            Display = new byte[256, 256];
            Array.Clear(Display, 0, 256 * 256);


            foreach (int address in program.Keys)
            {
                UInt16[] m = program[address].GetOpcodeMemoryValue();
                Array.Copy(m, 0, Memory, address, m.Length);
            }
        }

        // State
        int ProgramCounter;
        public Register[] Register;
        public UInt16[] Memory;
        public byte[,] Display;

        // Program
        Dictionary<int, Opcode> Program;
        bool breakFlag = true;

        public void Reset()
        {
            breakFlag = true;
            SetProgramCounter(0);
            ProgramCounter = -1;
            SetProgramCounter(0);
            for (int i = 0; i < 16; i++)
                SetRegister(i, 0);
        }

        public void Step()
        {
            breakFlag = true;

            Opcode op = Program[ProgramCounter];

            switch (op.Command)
            {
                case Command.j:
                    SetProgramCounter(op.Imm15);
                    break;
                case Command.draw:
                    SetDisplayPixel(Register[op.Register1.Number], Register[op.Register2.Number], Register[op.Register3.Number]);
                    IncreaseProgramCounter();
                    break;
                case Command.movh:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue & 0xFF) | ((op.Imm8 & 0xFF) << 8)));
                    IncreaseProgramCounter();
                    break;
                case Command.movl:
                    SetRegister(op.Register1.Number, (UInt16)((op.Imm8 & 0xFF)));
                    IncreaseProgramCounter();
                    break;
                case Command.beq:
                    if (Register[op.Register1.Number].Value.UValue == Register[op.Register2.Number].Value.UValue)
                        SetProgramCounter(ProgramCounter + op.Imm4);
                    else
                        IncreaseProgramCounter();
                    break;
                case Command.bgt:  // Signed compare
                    if (Register[op.Register1.Number].Value.SValue > Register[op.Register2.Number].Value.SValue)
                        SetProgramCounter(ProgramCounter + op.Imm4);
                    else
                        IncreaseProgramCounter();
                    break;
                case Command.ba:  // Unsigned compare
                    if (Register[op.Register1.Number].Value.UValue > Register[op.Register2.Number].Value.UValue)
                        SetProgramCounter(ProgramCounter + op.Imm4);
                    else
                        IncreaseProgramCounter();
                    break;
                case Command.mov:
                    SetRegister(op.Register1.Number, (UInt16)(Register[op.Register2.Number].Value.UValue & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.add:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue + Register[op.Register2.Number].Value.UValue) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.sub:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue - Register[op.Register2.Number].Value.UValue) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.neg:
                    SetRegister(op.Register1.Number, (UInt16)((~(Register[op.Register1.Number].Value.UValue) + 1) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.and:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue & Register[op.Register2.Number].Value.UValue) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.or:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue | Register[op.Register2.Number].Value.UValue) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.xor:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue ^ Register[op.Register2.Number].Value.UValue) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.not:
                    SetRegister(op.Register1.Number, (UInt16)(~(Register[op.Register1.Number].Value.UValue)));
                    IncreaseProgramCounter();
                    break;
                case Command.shl:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue << op.Imm4) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.shr:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue >> op.Imm4) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.sar:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue >> op.Imm4) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.read_16:
                    UInt16 raddr_16 = Register[op.Register2.Number].Value.UValue;
                    UInt16 rval_16 = (ushort)Memory[raddr_16];
                    SetRegister(op.Register1.Number, rval_16);
                    IncreaseProgramCounter();
                    break;
                case Command.write_16:
                    UInt16 waddr_16 = Register[op.Register2.Number].Value.UValue;
                    UInt16 wval_16 = Register[op.Register1.Number].Value.UValue;
                    Memory[waddr_16] = wval_16;
                    IncreaseProgramCounter();
                    break;
                case Command.addi:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue + op.Imm4) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.subi:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue - op.Imm4) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.jr:
                    SetProgramCounter(Register[op.Register1.Number].Value.UValue);
                    break;
                case Command.wait:
                    // wait for a half second
                    Thread.Sleep(70);
                    FireDisplayMemoryChanged(0, 0, 0, -1);
                    IncreaseProgramCounter();
                    break;
                case Command.org:
                    throw new Exception("Internal: Pseudo opcode ORG should never be possible");
                case Command.li:
                    throw new Exception("Internal: Pseudo opcode LI should never be possible");
                case Command.jrl:
                    throw new Exception("Internal: Pseudo opcode JRL should never be possible");
                default:
                    throw new Exception("Internal: Unhandled command: " + op.Command.ToString());
            }
        }

        public void Run()
        {
            if (!breakFlag)
                return;

            breakFlag = false;

            while (!breakFlag)
            {
                Step();
                Application.DoEvents();
            }
        }

        public void Break()
        {
            breakFlag = true;
        }

        // private helper functions
        // ProgramCounter
        #region private void IncreaseProgramCounter()
        private void IncreaseProgramCounter()
        {
            int newPC = ProgramCounter + 1;
            if (newPC > 65535)
                newPC = 0;

            SetProgramCounter(newPC);
        }
        #endregion
        #region private void SetProgramCounter(int newProgramCounter)
        private void SetProgramCounter(int newProgramCounter)
        {
            FireProgramCounterChanged(ProgramCounter, newProgramCounter);
            ProgramCounter = newProgramCounter;
        }
        #endregion
        //Register
        #region private void SetRegister(int register, UInt16 value)
        private void SetRegister(int register, UInt16 value)
        {
            UInt16 oldValue = Register[register].Value.UValue;
            Register[register].Value.UValue = value;
            FireRegisterChanged(Register[register], oldValue);
        }
        #endregion
        // Display
        private void SetDisplayPixel(Register reg1, Register reg2, Register reg3)
        {
            // Set the new Pixel value
            int oldColor = Display[reg1.Value.UValue, reg2.Value.UValue];
            Display[reg1.Value.UValue, reg2.Value.UValue] = (byte)reg3.Value.UValue;
            
            // Moved to wait command to emulate the VSync
            FireDisplayMemoryChanged(reg1.Value.UValue, reg2.Value.UValue, oldColor, reg3.Value.UValue);

            // Return the previous Pixel value
            UInt16 oldValue = reg3.Value.UValue;
            reg3.Value.UValue = (ushort)oldColor;
            FireRegisterChanged(reg3, oldValue);
        }
        // Memory
        private void SetMemoryValue()
        {
        }

        // Events
        #region private void FireDisplayMemoryChanged(int X, int Y, int OldColor, int NewColor)
        private void FireDisplayMemoryChanged(int X, int Y, int OldColor, int NewColor)
        {
            if (OldColor == NewColor)
                return;

            DisplayMemoryChangedEventArgs args = new DisplayMemoryChangedEventArgs();
            args.X = X;
            args.Y = Y;
            args.OldColor = OldColor;
            args.NewColor = NewColor;

            OnDisplayMemoryChanged(args);
        }
        #endregion 
        #region private void FireProgramCounterChanged(int OldProgramCounter, int NewProgramCounter)
        private void FireProgramCounterChanged(int OldProgramCounter, int NewProgramCounter)
        {
            if (OldProgramCounter == NewProgramCounter)
                return;

            ProgramCounterChangedEventArgs args = new ProgramCounterChangedEventArgs();
            args.OldProgramCounter = OldProgramCounter;
            args.NewProgramCounter = NewProgramCounter;
            try { args.OldLine = Program[OldProgramCounter].SourceLine; }
            catch { args.OldLine = null; }
            try { args.NewLine = Program[NewProgramCounter].SourceLine; }
            catch { args.NewLine = null; }

            OnProgramCounterChanged(args);
        }
        #endregion
        #region private void FireRegisterChanged(Register register, int oldvalue)
        private void FireRegisterChanged(Register register, int oldvalue)
        {
            if (register.Value.SValue == oldvalue || register.Value.UValue == oldvalue)
                return;

            RegisterChangedEventArgs args = new RegisterChangedEventArgs();
            args.Register = register;
            OnRegisterChanged(args);
        }
        #endregion
    }
}