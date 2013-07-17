using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.Entities
{
    public class Opcode : ICloneable
    {
        // the opcode definition parts
        public string Command { get; private set; }
        public int Value { get; private set; }
        public OpcodeType[] ApplicableTypes { get; private set; }
        public bool Pseudo { get; private set; }

        // the source of which this opcode resulted from
        public Line SourceLine { get; private set; }

        // the opcode values. label, type, arguments
        public string Label { get; private set; }
        public OpcodeType Type { get; set; }

        #region public Register Register1
        private Register _register1;
        public Register Register1
        {
            get { return _register1; }
            set { _register1 = value; }
        }
        #endregion
        #region public Register Register2
        private Register _register2;
        public Register Register2
        {
            get { return _register2; }
            set { _register2 = value; }
        }
        #endregion
        #region public Register Register3
        private Register _register3;
        public Register Register3
        {
            get { return _register3; }
            set { _register3 = value; }
        }
        #endregion
        #region public int Imm4
        private int _imm4;
        public int Imm4
        {
            get { return _imm4; }
            set { _imm4 = value; }
        }
        #endregion
        #region public int Imm8
        private int _imm8;
        public int Imm8
        {
            get { return _imm8; }
            set { _imm8 = value; }
        }
        #endregion
        #region public int Imm15
        private int _imm15;
        public int Imm15
        {
            get { return _imm15; }
            set { _imm15 = value; }
        }
        #endregion
        #region public int Imm16
        private int _imm16;
        public int Imm16
        {
            get { return _imm16; }
            set { _imm16 = value; }
        }
        #endregion
        #region public string Imm4label
        private string _imm4label = null;
        public string Imm4label
        {
            get { return _imm4label; }
            set { _imm4label = value; }
        }
        #endregion
        #region public string Imm15label
        private string _imm15label = null;
        public string Imm15label
        {
            get { return _imm15label; }
            set { _imm15label = value; }
        }
        #endregion
        #region public string Imm16label
        private string _imm16label = null;
        public string Imm16label
        {
            get { return _imm16label; }
            set { _imm16label = value; }
        }
        #endregion


        // constructor
        #region public Opcode(string s, OpcodeType[] t, bool pseudo)
        public Opcode(string command, int value, OpcodeType[] applicableTypes, bool pseudo)
        {
            Command = command;
            Value = value;
            ApplicableTypes = applicableTypes;
            Pseudo = pseudo;
        }
        #endregion

        // helper functions
        #region public UInt16 GetOpcodeValue()
        public UInt16 GetOpcodeValue()
        {
            switch (Type)
            {
                case OpcodeType.ThreeArg_RegRegReg:
                    return Convert.ToUInt16(Value | (Register1.Number << 8) | (Register2.Number << 4) | (Register3.Number << 0));
                case OpcodeType.ThreeArg_RegRegImm4:
                    return Convert.ToUInt16(Value | (Register1.Number << 8) | (Register2.Number << 4) | (Imm4 << 0));
                case OpcodeType.ThreeArg_RegRegImm4label:
                    return Convert.ToUInt16(Value | (Register1.Number << 8) | (Register2.Number << 4) | 0); // Label is set as zero
                case OpcodeType.TwoArg_RegReg:
                    return Convert.ToUInt16(Value | (Register1.Number << 4) | (Register2.Number << 0));
                case OpcodeType.TwoArg_RegImm8:
                    return Convert.ToUInt16(Value | (Register1.Number << 8) | (Imm8 << 0));
                case OpcodeType.TwoArg_RegImm4:
                    return Convert.ToUInt16(Value | (Register1.Number << 4) | (Imm4 << 0));
                case OpcodeType.OneArg_Reg:
                    return Convert.ToUInt16(Value | (Register1.Number << 0));
                case OpcodeType.OneArg_Imm15:
                    return Convert.ToUInt16(Value | Imm15);
                case OpcodeType.OneArg_Imm15label:
                    return Convert.ToUInt16(Value | 0); // Label is set as zero
                case OpcodeType.OneArg_Imm16:
                    return 0xffff;
                case OpcodeType.ZeroArg:
                    return Convert.ToUInt16(Value);
                default:
                    throw new NotImplementedException("OpcodeType not implemented [" + Type.ToString() + "]");
            }
        }
        #endregion
        #region public string GetNormalizedSourceLine(int labelPadding)
        public string GetNormalizedSourceLine(int labelPadding)
        {
            string lbl = (Label != null) ? Label.PadRight(labelPadding) : "".PadRight(labelPadding);

            switch (Type)
            {
                case OpcodeType.ThreeArg_RegRegReg:
                    return lbl + " " + Command + " " + Register1.Name + ", " + Register2.Name + ", " + Register3.Name;
                case OpcodeType.ThreeArg_RegRegImm4:
                    return lbl + " " + Command + " " + Register1.Name + ", " + Register2.Name + ", " + Imm4.ToString("X");
                case OpcodeType.ThreeArg_RegRegImm4label:
                    return lbl + " " + Command + " " + Register1.Name + ", " + Register2.Name + ", " + Imm4label;
                case OpcodeType.TwoArg_RegReg:
                    return lbl + " " + Command + " " + Register1.Name + ", " + Register2.Name;
                case OpcodeType.TwoArg_RegImm8:
                    return lbl + " " + Command + " " + Register1.Name + ", 0x" + Imm8.ToString("X").PadLeft(2, '0');
                case OpcodeType.TwoArg_RegImm4:
                    return lbl + " " + Command + " " + Register1.Name + ", 0x" + Imm4.ToString("X");
                case OpcodeType.OneArg_Reg:
                    return lbl + " " + Command + " " + Register1.Name;
                case OpcodeType.OneArg_Imm15:
                    return lbl + " " + Command + " " + Imm15.ToString("X");
                case OpcodeType.OneArg_Imm15label:
                    return lbl + " " + Command + " " + Imm15label;
                case OpcodeType.OneArg_Imm16:
                    return lbl + " " + Command + " " + Imm16.ToString("X");
                case OpcodeType.ZeroArg:
                    return lbl + " " + Command;
                default:
                    throw new NotImplementedException("OpcodeType not implemented [" + Type.ToString() + "]");
            }
        }
        #endregion
        #region public void SetLabel(string label)
        public void SetLabel(string label)
        {
            Label = label;
        }
        #endregion
        #region public void SetSourceLine(Line line)
        public void SetSourceLine(Line line)
        {
            SourceLine = line;
        }
        #endregion
        #region public void SetImm15labelAddress(int address)
        public void SetImm15labelAddress(int address)
        {
            _imm15 = address;
        }
        #endregion
        #region public object Clone()
        public object Clone()
        {
            Opcode op = new Opcode(Command, Value, ApplicableTypes, Pseudo);
            //op.SetLabel(Label);
            //op.SetSourceLine(SourceLine);
            return op;
        }
        #endregion


        #region public void Parse(string[] args)
        public void Parse(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            foreach (OpcodeType t in ApplicableTypes)
            {
                switch (t)
                {
                    case OpcodeType.ThreeArg_RegRegReg:
                        argMatch = CheckArguments_ThreeArg_RegRegReg(args);
                        break;
                    case OpcodeType.ThreeArg_RegRegImm4:
                        argMatch = CheckArguments_ThreeArg_RegRegImm4(args);
                        break;
                    case OpcodeType.ThreeArg_RegRegImm4label:
                        argMatch = CheckArguments_ThreeArg_RegRegImm4label(args);
                        break;
                    case OpcodeType.TwoArg_RegReg:
                        argMatch = CheckArguments_TwoArg_RegReg(args);
                        break;
                    case OpcodeType.TwoArg_RegImm16:
                        argMatch = CheckArguments_TwoArg_RegImm16(args);
                        break;
                    case OpcodeType.TwoArg_RegImm16label:
                        argMatch = CheckArguments_TwoArg_RegImm16label(args);
                        break;
                    case OpcodeType.TwoArg_RegImm8:
                        argMatch = CheckArguments_TwoArg_RegImm8(args);
                        break;
                    case OpcodeType.TwoArg_RegImm4:
                        argMatch = CheckArguments_TwoArg_RegImm4(args);
                        break;
                    case OpcodeType.OneArg_Reg:
                        argMatch = CheckArguments_OneArg_Reg(args);
                        break;
                    case OpcodeType.OneArg_Imm15:
                        argMatch = CheckArguments_OneArg_Imm15(args);
                        break;
                    case OpcodeType.OneArg_Imm15label:
                        argMatch = CheckArguments_OneArg_Imm15label(args);
                        break;
                    case OpcodeType.OneArg_Imm16:
                        argMatch = CheckArguments_OneArg_Imm16(args);
                        break;
                    case OpcodeType.ZeroArg:
                        argMatch = CheckArguments_ZeroArg(args);
                        break;
                    default:
                        throw new NotImplementedException("OpcodeType not implemented [" + t.ToString() + "]");
                }

                if (argMatch)
                {
                    Type = t;
                    break;
                }
            }

            if (!argMatch)
                throw new Exception("Arguments are incorrect");
        }
        #endregion


        // private check functions
        #region private bool CheckArguments_ThreeArg_RegRegReg(string[] args)
        private bool CheckArguments_ThreeArg_RegRegReg(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length
            if ((argMatch = CheckArgCount(argLength, 3)))
                if ((argMatch = CheckIsRegister(args[0], out _register1)))
                    if ((argMatch = CheckIsRegister(args[1], out _register2)))
                        argMatch = CheckIsRegister(args[2], out _register3);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_ThreeArg_RegRegImm4(string[] args)
        private bool CheckArguments_ThreeArg_RegRegImm4(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length
            if ((argMatch = CheckArgCount(argLength, 3)))
                if ((argMatch = CheckIsRegister(args[0], out _register1)))
                    if ((argMatch = CheckIsRegister(args[1], out _register2)))
                        argMatch = CheckIsImm4(args[2], out _imm4);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_ThreeArg_RegRegImm4label(string[] args)
        private bool CheckArguments_ThreeArg_RegRegImm4label(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length and register, label check is deferred
            if ((argMatch = CheckArgCount(argLength, 3)))
                if ((argMatch = CheckIsRegister(args[0], out _register1)))
                    if ((argMatch = CheckIsRegister(args[1], out _register2)))
                        argMatch = CheckIsLabel(args[2], out _imm4label);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_TwoArg_RegReg(string[] args)
        private bool CheckArguments_TwoArg_RegReg(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length
            if ((argMatch = CheckArgCount(argLength, 2)))
                if ((argMatch = CheckIsRegister(args[0], out _register1)))
                    argMatch = CheckIsRegister(args[1], out _register2);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_TwoArg_RegImm16(string[] args)
        private bool CheckArguments_TwoArg_RegImm16(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length
            if ((argMatch = CheckArgCount(argLength, 2)))
                if ((argMatch = CheckIsRegister(args[0], out _register1)))
                    argMatch = CheckIsImm16(args[1], out _imm16);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_TwoArg_RegImm16label(string[] args)
        private bool CheckArguments_TwoArg_RegImm16label(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length
            if ((argMatch = CheckArgCount(argLength, 2)))
                if ((argMatch = CheckIsRegister(args[0], out _register1)))
                    argMatch = CheckIsLabel(args[1], out _imm16label);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_TwoArg_RegImm8(string[] args)
        private bool CheckArguments_TwoArg_RegImm8(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length
            if ((argMatch = CheckArgCount(argLength, 2)))
                if ((argMatch = CheckIsRegister(args[0], out _register1)))
                    argMatch = CheckIsImm8(args[1], out _imm8);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_TwoArg_RegImm4(string[] args)
        private bool CheckArguments_TwoArg_RegImm4(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length
            if ((argMatch = CheckArgCount(argLength, 2)))
                if ((argMatch = CheckIsRegister(args[0], out _register1)))
                    argMatch = CheckIsImm4(args[1], out _imm4);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_OneArg_Reg(string[] args)
        private bool CheckArguments_OneArg_Reg(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length
            if ((argMatch = CheckArgCount(argLength, 1)))
                argMatch = CheckIsRegister(args[0], out _register1);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_OneArg_Imm15(string[] args)
        private bool CheckArguments_OneArg_Imm15(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length
            if ((argMatch = CheckArgCount(argLength, 1)))
                argMatch = CheckIsImm15(args[0], out _imm15);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_OneArg_Imm15label(string[] args)
        private bool CheckArguments_OneArg_Imm15label(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length only, label check is deferred
            if ((argMatch = CheckArgCount(argLength, 1)))
                argMatch = CheckIsLabel(args[0], out _imm15label);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_OneArg_Imm16(string[] args)
        private bool CheckArguments_OneArg_Imm16(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length
            if ((argMatch = CheckArgCount(argLength, 1)))
                argMatch = CheckIsImm16(args[0], out _imm16);

            return argMatch;
        }
        #endregion
        #region private bool CheckArguments_ZeroArg(string[] args)
        private bool CheckArguments_ZeroArg(string[] args)
        {
            int argLength = args.Length;
            bool argMatch = false;

            // check argument length
            argMatch = CheckArgCount(argLength, 0);

            return argMatch;
        }
        #endregion

        // private elementary check functions
        #region private bool CheckArgCount(int actual, int expected)
        private bool CheckArgCount(int actual, int expected)
        {
            return actual == expected;
        }
        #endregion
        #region private bool CheckIsRegister(string s, out Register r)
        private bool CheckIsRegister(string s, out Register r)
        {
            r = null;

            bool b = RegisterDictionary.Contains(s);
            if (b)
                r = RegisterDictionary.Get(s);

            return b;
        }
        #endregion
        #region private bool CheckIsImm4(string s, out int v)
        private bool CheckIsImm4(string s, out int v)
        {
            v = -1;

            if (string.IsNullOrEmpty(s))
                return false;

            bool b = GetIntValue(s, out v);
            return (b && v >= 0 && v < Math.Pow(2, 4));
        }
        #endregion
        #region private bool CheckIsImm8(string s, out int v)
        private bool CheckIsImm8(string s, out int v)
        {
            v = -1;

            if (string.IsNullOrEmpty(s))
                return false;

            bool b = GetIntValue(s, out v);
            return (b && v >= 0 && v < Math.Pow(2, 8));
        }
        #endregion
        #region private bool CheckIsImm15(string s, out int v)
        private bool CheckIsImm15(string s, out int v)
        {
            v = -1;

            if (string.IsNullOrEmpty(s))
                return false;

            bool b = GetIntValue(s, out v);
            return (b && v >= 0 && v < Math.Pow(2, 15));
        }
        #endregion
        #region private bool CheckIsImm16(string s, out int v)
        private bool CheckIsImm16(string s, out int v)
        {
            v = -1;

            if (string.IsNullOrEmpty(s))
                return false;

            bool b = GetIntValue(s, out v);
            return (b && v >= 0 && v < Math.Pow(2, 16));
        }
        #endregion
        #region private bool CheckIsLabel(string s, out string v)
        private bool CheckIsLabel(string s, out string v)
        {
            v = s;
            if (string.IsNullOrEmpty(s))
                return false;

            return true;
        }
        #endregion

        // private utility functions
        #region private bool GetIntValue(string s, out int i)
        private bool GetIntValue(string s, out int i)
        {
            s = s.ToLower();
            i = -1;

            if (s.Length > 1 && s.Substring(0, 2) == "0x")
            {
                try
                {
                    i = Convert.ToInt32(s, 16);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    i = Convert.ToInt32(s, 10);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}