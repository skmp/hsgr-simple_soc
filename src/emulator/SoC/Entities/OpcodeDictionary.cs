using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.Entities
{
    public class OpcodeDictionary
    {
        private static Dictionary<string, Opcode> opcodeDictionary;

        static OpcodeDictionary()
        {
            List<Opcode> list = new List<Opcode>();
            list.Add(new Opcode(Command.j, 0x8000, new OpcodeType[] { OpcodeType.OneArg_Imm15, OpcodeType.OneArg_Imm15label }, false));
            list.Add(new Opcode(Command.draw, 0x0000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegReg }, false));
            list.Add(new Opcode(Command.movh, 0x1000, new OpcodeType[] { OpcodeType.TwoArg_RegImm8 }, false));
            list.Add(new Opcode(Command.movl, 0x2000, new OpcodeType[] { OpcodeType.TwoArg_RegImm8 }, false));
            list.Add(new Opcode(Command.beq, 0x3000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegImm4, OpcodeType.ThreeArg_RegRegImm4label }, false));
            list.Add(new Opcode(Command.bgt, 0x4000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegImm4, OpcodeType.ThreeArg_RegRegImm4label }, false));
            list.Add(new Opcode(Command.ba, 0x5000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegImm4, OpcodeType.ThreeArg_RegRegImm4label }, false));
            list.Add(new Opcode(Command.mov, 0x6000, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.add, 0x6100, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.sub, 0x6200, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.and, 0x6300, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.or, 0x6400, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.xor, 0x6500, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.shl, 0x6600, new OpcodeType[] { OpcodeType.TwoArg_RegImm4 }, false));
            list.Add(new Opcode(Command.shr, 0x6700, new OpcodeType[] { OpcodeType.TwoArg_RegImm4 }, false));
            list.Add(new Opcode(Command.not, 0x6800, new OpcodeType[] { OpcodeType.OneArg_Reg }, false));
            list.Add(new Opcode(Command.neg, 0x6900, new OpcodeType[] { OpcodeType.OneArg_Reg }, false));
            list.Add(new Opcode(Command.readm, 0x6a00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.writem, 0x6b00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode(Command.jr, 0x6c00, new OpcodeType[] { OpcodeType.OneArg_Reg }, false));
            list.Add(new Opcode(Command.wait, 0x6c10, new OpcodeType[] { OpcodeType.ZeroArg }, false));

            list.Add(new Opcode(Command.org, -1, new OpcodeType[] { OpcodeType.OneArg_Imm16 }, true));         // ORiGin
            list.Add(new Opcode(Command.li, -1, new OpcodeType[] { OpcodeType.TwoArg_RegImm16 }, true));       // LoadImmediate
            list.Add(new Opcode(Command.jrl, -1, new OpcodeType[] { OpcodeType.TwoArg_RegImm16, OpcodeType.TwoArg_RegImm16label }, true)); // JumpRegisterLabel
            
            opcodeDictionary = new Dictionary<string, Opcode>();
            foreach (Opcode op in list)
            {
                opcodeDictionary.Add(op.Command.ToString(), op);
            }
        }

        public static bool Contains(string s)
        {
            return opcodeDictionary.ContainsKey(s.ToLower());
        }

        public static Opcode Get(string s)
        {
            if (opcodeDictionary.ContainsKey(s.ToLower()))
                return (Opcode)opcodeDictionary[s.ToLower()].Clone();

            return null;
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
