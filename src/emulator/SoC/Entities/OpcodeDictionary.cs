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
            list.Add(new Opcode("j", 0x8000, new OpcodeType[] { OpcodeType.OneArg_Imm15, OpcodeType.OneArg_Imm15label }, false));
            list.Add(new Opcode("draw", 0x0000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegReg }, false));
            list.Add(new Opcode("movh", 0x1000, new OpcodeType[] { OpcodeType.TwoArg_RegImm8 }, false));
            list.Add(new Opcode("movl", 0x2000, new OpcodeType[] { OpcodeType.TwoArg_RegImm8 }, false));
            list.Add(new Opcode("beq", 0x3000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegImm4, OpcodeType.ThreeArg_RegRegImm4label }, false));
            list.Add(new Opcode("bgt", 0x4000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegImm4, OpcodeType.ThreeArg_RegRegImm4label }, false));
            list.Add(new Opcode("ba", 0x5000, new OpcodeType[] { OpcodeType.ThreeArg_RegRegImm4, OpcodeType.ThreeArg_RegRegImm4label }, false));
            list.Add(new Opcode("mov", 0x6000, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode("add", 0x6100, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode("sub", 0x6200, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode("and", 0x6300, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode("or", 0x6400, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode("xor", 0x6500, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode("shl", 0x6600, new OpcodeType[] { OpcodeType.TwoArg_RegImm4 }, false));
            list.Add(new Opcode("shr", 0x6700, new OpcodeType[] { OpcodeType.TwoArg_RegImm4 }, false));
            list.Add(new Opcode("not", 0x6800, new OpcodeType[] { OpcodeType.OneArg_Reg }, false));
            list.Add(new Opcode("neg", 0x6900, new OpcodeType[] { OpcodeType.OneArg_Reg }, false));
            list.Add(new Opcode("readm", 0x6a00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode("writem", 0x6b00, new OpcodeType[] { OpcodeType.TwoArg_RegReg }, false));
            list.Add(new Opcode("jr", 0x6c00, new OpcodeType[] { OpcodeType.OneArg_Reg }, false));
            list.Add(new Opcode("wait", 0x6c10, new OpcodeType[] { OpcodeType.ZeroArg }, false));

            list.Add(new Opcode("org", -1, new OpcodeType[] { OpcodeType.OneArg_Imm16 }, true));         // ORiGin
            list.Add(new Opcode("li", -1, new OpcodeType[] { OpcodeType.TwoArg_RegImm16 }, true));       // LoadImmediate
            list.Add(new Opcode("jrl", -1, new OpcodeType[] { OpcodeType.TwoArg_RegImm16, OpcodeType.TwoArg_RegImm16label }, true)); // JumpRegisterLabel
            
            opcodeDictionary = new Dictionary<string, Opcode>();
            foreach (Opcode op in list)
            {
                opcodeDictionary.Add(op.Command, op);
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
