using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SoC.BL.Entities
{
    public class OpcodeDictionary
    {
        public static Dictionary<string, Opcode> Dictionary;

        static OpcodeDictionary()
        {
            List<Opcode> list = new List<Opcode>();
            list.Add(new Opcode(Command.j, 0x0000, 0x8000, new OpcodeType[] { OpcodeType.OneArg_Imm15, OpcodeType.OneArg_Imm15label }, false));
            list.Add(new Opcode(Command.draw, 0x8000, 0xF000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegReg }, false));
            list.Add(new Opcode(Command.movh, 0x9000, 0xF000, new OpcodeType[] { OpcodeType.TwoArg_RegImm8 }, false));
            list.Add(new Opcode(Command.movl, 0xA000, 0xF000, new OpcodeType[] { OpcodeType.TwoArg_RegImm8 }, false));
            list.Add(new Opcode(Command.beq, 0xB000, 0xF000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegImm4, OpcodeType.ThreeArg_RegRegImm4label }, false));
            list.Add(new Opcode(Command.bgt, 0xC000, 0xF000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegImm4, OpcodeType.ThreeArg_RegRegImm4label }, false));
            list.Add(new Opcode(Command.ba, 0xD000, 0xF000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegImm4, OpcodeType.ThreeArg_RegRegImm4label }, false));
            list.Add(new Opcode(Command.mov, 0xE000, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.add, 0xE100, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.sub, 0xE200, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.neg, 0xE300, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.and, 0xE400, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.or, 0xE500, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.xor, 0xE600, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.not, 0xE700, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.shl, 0xE800, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegImm4 }, false));
            list.Add(new Opcode(Command.shr, 0xE900, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegImm4 }, false));
            list.Add(new Opcode(Command.sar, 0xEA00, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegImm4 }, false));
            list.Add(new Opcode(Command.read_16, 0xEB00, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.write_16, 0xEC00, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.addi, 0xED00, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegImm4 }, false));
            list.Add(new Opcode(Command.subi, 0xEE00, 0xFF00, new OpcodeType[] { OpcodeType.TwoArg_RegImm4 }, false));

            list.Add(new Opcode(Command.jr, 0xF000, 0xFFF0, new OpcodeType[] { OpcodeType.OneArg_Reg }, false));
            list.Add(new Opcode(Command.wait, 0xF010, 0xFFF0, new OpcodeType[] { OpcodeType.OneArg_Imm4 }, false));

            list.Add(new Opcode(Command.org, -1, 0, new OpcodeType[] { OpcodeType.OneArg_Imm16 }, true));         // ORiGin
            list.Add(new Opcode(Command.li, -1, 0, new OpcodeType[] { OpcodeType.TwoArg_RegImm16, OpcodeType.TwoArg_RegImm16label }, true));       // LoadImmediate
            list.Add(new Opcode(Command.jrl, -1, 0, new OpcodeType[] { OpcodeType.TwoArg_RegImm16, OpcodeType.TwoArg_RegImm16label }, true)); // JumpRegisterLabel
            list.Add(new Opcode(Command.dw, -1, 0, new OpcodeType[] { OpcodeType.OneArg_Data16 }, false)); // DataWord

            Dictionary = new Dictionary<string, Opcode>();
            foreach (Opcode op in list)
            {
                Dictionary.Add(op.Command.ToString(), op);
            }
        }

        public static bool Contains(string s)
        {
            return Dictionary.ContainsKey(s.ToLower());
        }

        public static Opcode Get(string s)
        {
            if (Dictionary.ContainsKey(s.ToLower()))
                return (Opcode)Dictionary[s.ToLower()].Clone();

            return null;
        }

        public static Opcode Get(UInt16 v)
        {
            Opcode o = null;
            foreach (Opcode op in Dictionary.Values)
            {
                if (op.Value == (v & op.Mask))
                {
                    o = (Opcode)op.Clone();
                    o.SetOpcodeValue(v);
                    break;
                }
            }

            return o;
        }

        public static Opcode Get(string[] token)
        {
            // if no tokens, then it is an empty line
            if (token == null || token.Length <= 0)
                return null;

            // argument start, length into token array
            int argStart = 0;
            int argLength = token.Length;

            // the label if it exists
            string label = null;

            // check if first or second token is a command
            Opcode op = Get(token[0]);
            if (op == null && token.Length > 1)
            {
                label = token[0];
                op = Get(token[1]);
                argStart++;
                argLength--;
            }
            argStart++;
            argLength--;

            // no command could be identified
            if (op == null)
                throw new Exception("Unknown command [" + token[0] + "]");

            // at this point command has been identified

            // set label (can be null)
            op.SetLabel(label);

            // get/parse arguments
            if (argLength > 0)
            {
                string[] args = new string[argLength];
                Array.Copy(token, argStart, args, 0, argLength);
                op.Parse(args);
            }
            return op;
        }
    }
}
