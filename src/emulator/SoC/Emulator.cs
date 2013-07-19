using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using SoC.Entities;
using System.Threading;

namespace SoC
{
    public delegate void DisplayMemoryChangedEventHandler(object o, DisplayMemoryChangedEventArgs e);
    public delegate void RegisterChangedEventHandler(object o, RegisterChangedEventArgs e);
    public delegate void ProgramCounterChangedEventHandler(object o, ProgramCounterChangedEventArgs e);

    public class DisplayMemoryChangedEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }
    }
    public class RegisterChangedEventArgs : EventArgs
    {
        public Register Register { get; set; }
    }
    public class ProgramCounterChangedEventArgs : EventArgs
    {
        public int OldProgramCounter { get; set; }
        public int NewProgramCounter { get; set; }
        public Line OldLine { get; set; }
        public Line NewLine { get; set; }
    }

    public class Emulator
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


        public Emulator(Dictionary<int, Opcode> program)
        {
            ProgramCounter = 0;
            Program = program;
            Register = new Register[16];
            for (int i = 0; i < 16; i++)
                Register[i] = new Register(i, 0);
            Memory = new byte[16384];
        }

        // State
        int ProgramCounter;
        public Register[] Register;
        public byte[] Memory;

        // Program
        Dictionary<int, Opcode> Program;
        bool breakFlag = false;

        public void Reset()
        {
            breakFlag = false;
            ProgramCounter = -1;
            SetProgramCounter(0);
            for (int i = 0; i < 16; i++)
                SetRegister(i, 0);
        }

        public void Step()
        {
            breakFlag = false;

            Opcode op = Program[ProgramCounter];
            switch (op.Command)
            {
                case Command.j:
                    FireProgramCounterChanged(ProgramCounter, op.Imm15);
                    SetProgramCounter(op.Imm15);
                    break;
                case Command.draw:
                    break;
                case Command.movh:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue & 0xFF) | ((op.Imm8 & 0xFF) << 8)));
                    IncreaseProgramCounter();
                    break;
                case Command.movl:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue & 0xFF00) | (op.Imm8 & 0xFF)));
                    IncreaseProgramCounter();
                    break;
                case Command.beq:
                    if (Register[op.Register1.Number] == Register[op.Register2.Number])
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
                case Command.shl:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue << op.Imm4) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.shr:
                    SetRegister(op.Register1.Number, (UInt16)((Register[op.Register1.Number].Value.UValue >> op.Imm4) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.not:
                    SetRegister(op.Register1.Number, (UInt16)(~(Register[op.Register1.Number].Value.UValue)));
                    IncreaseProgramCounter();
                    break;
                case Command.neg:
                    SetRegister(op.Register1.Number, (UInt16)((~(Register[op.Register1.Number].Value.UValue) + 1) & 0xFFFF));
                    IncreaseProgramCounter();
                    break;
                case Command.readm:
                    IncreaseProgramCounter();
                    break;
                case Command.writem:
                    IncreaseProgramCounter();
                    break;
                case Command.jr:
                    SetProgramCounter(Register[op.Register1.Number].Value.UValue);
                    break;
                case Command.wait:
                    // wait for a half second
                    Thread.Sleep(500);
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
        #region private void SetRegister(int register, Int16 value)
        private void SetRegister(int register, Int16 value)
        {
            FireRegisterChanged(Register[register], value);
            Register[register].Value.SValue = value;
        }
        #endregion

        // Events
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