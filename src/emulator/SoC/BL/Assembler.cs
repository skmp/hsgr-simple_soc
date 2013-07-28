using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using SoC.BL.Entities;


namespace SoC.BL
{
    public class Assembler
    {
        #region public static AssemblerOutput Assemble(String source)
        public static AssemblerOutput Assemble(String source)
        {
            using (StringReader sr = new StringReader(source))
            {
                return Assemble(sr);
            }
        }
        #endregion
        #region public static AssemblerOutput Assemble(TextReader reader)
        public static AssemblerOutput Assemble(TextReader reader)
        {
            List<Line> source = Parse(reader);
            Dictionary<int, Opcode> binary = Assemble(source);

            AssemblerOutput output = new AssemblerOutput(source, binary);
            return output;
        }
        #endregion

        // Private helpers
        #region private static void IncreaseAddress(ref int addr)
        private static void IncreaseAddress(ref int addr)
        {
            // 16 bit commands on a 8bit memory
            addr += 2;
        }
        #endregion
        #region private static List<Line> Parse(TextReader reader)
        private static List<Line> Parse(TextReader reader)
        {
            string strLine;
            int lineNumber = -1;
            Line line;
            List<Line> source = new List<Line>();

            lineNumber = 1;
            while ((strLine = reader.ReadLine()) != null)
            {
                line = new Line(strLine, lineNumber++);
                source.Add(line);
            }

            return source;
        }
        #endregion

        // 1st pass
        #region private static Dictionary<int, Line> Assemble(List<Line> source)
        private static Dictionary<int, Opcode> Assemble(List<Line> source)
        {
            Dictionary<int, Opcode> binary = new Dictionary<int, Opcode>();
            Opcode op = null;
            RomItem[] ria = null;
            int address = 0;
            
            // 1st pass [Parse each command]
            foreach (Line line in source)
            {
                op = AssembleLine(line);

                if (op != null)
                {
                    // regular instruction
                    if (!op.Pseudo)
                    {
                        ria = new RomItem[1];
                        ria[0] = new RomItem(address, op);
                        IncreaseAddress(ref address);
                    }
                    else // Handle Assembler pseudo instructions
                    {
                        ria = HandlePseudoInstruction(ref address, op);
                    }

                    // 2nd pass [Resolve each Pseudo command]
                    foreach (RomItem ri in ria)
                    {
                        line.AddRomItem(ri);
                        try
                        {
                            binary.Add(ri.Address, ri.Opcode);
                        }
                        catch
                        {
                            line.SetError("Address is overlapping with other code");
                        }
                    }
                }
            }

            // 3rd pass [Resolve labels]
            ResolveLabels(binary);

            return binary;
        }
        #endregion
        #region private static Opcode AssembleLine(Line line)
        private static Opcode AssembleLine(Line line)
        {
            Opcode op = null;
            string[] token = null;

            try
            {
                token = line.Parse();
                op = OpcodeDictionary.Get(token);
                if (op != null)
                {
                    op.SetSourceLine(line);
                }
            }
            catch (Exception ex)
            {
                line.SetError(ex.Message);
                //throw new Exception(ex.Message, ex);
            }

            return op;
        }
        #endregion

        // 2nd pass
        #region private static RomItem[] HandlePseudoInstruction(ref int address, Opcode op)
        private static RomItem[] HandlePseudoInstruction(ref int address, Opcode op)
        {
            ArrayList arl = new ArrayList();
            RomItem ri;

            if (op.Command == Command.org)     // ORiGin
            {
                address = op.Imm16;
                if (!String.IsNullOrEmpty(op.Label))
                    op.SourceLine.SetError("ORG command is not allowed to have a label");
            }
            else if (op.Command == Command.li) // Load Immediate
            {
                ri = new RomItem(address, OpcodeDictionary.Get("movh"));
                ri.Opcode.Register1 = op.Register1;
                ri.Opcode.Imm8 = op.Imm16 >> 8;
                ri.Opcode.Type = OpcodeType.TwoArg_RegImm8;
                ri.Opcode.Label = op.Label;
                ri.Opcode.SetSourceLine(op.SourceLine);
                arl.Add(ri);
                IncreaseAddress(ref address);

                ri = new RomItem(address, OpcodeDictionary.Get("movl"));
                ri.Opcode.Register1 = op.Register1;
                ri.Opcode.Imm8 = op.Imm16 & 0xFF;
                ri.Opcode.Type = OpcodeType.TwoArg_RegImm8;
                ri.Opcode.SetSourceLine(op.SourceLine);
                arl.Add(ri);
                IncreaseAddress(ref address);
            }
            else if (op.Command == Command.jrl) // JumpRegisterLabel
            {
                ri = new RomItem(address, OpcodeDictionary.Get("movh"));
                ri.Opcode.Register1 = op.Register1;
                //ri.Opcode.Imm8 = op.Imm16 >> 8;
                ri.Opcode.Imm16label = op.Imm16label;
                ri.Opcode.Type = OpcodeType.TwoArg_RegImm16label_h;
                ri.Opcode.Label = op.Label;
                ri.Opcode.SetSourceLine(op.SourceLine);
                arl.Add(ri);
                IncreaseAddress(ref address);

                ri = new RomItem(address, OpcodeDictionary.Get("movl"));
                ri.Opcode.Register1 = op.Register1;
                //ri.Opcode.Imm8 = op.Imm16 & 0xFF;
                ri.Opcode.Imm16label = op.Imm16label;
                ri.Opcode.Type = OpcodeType.TwoArg_RegImm16label_l;
                ri.Opcode.SetSourceLine(op.SourceLine);
                arl.Add(ri);
                IncreaseAddress(ref address);

                ri = new RomItem(address, OpcodeDictionary.Get("jr"));
                ri.Opcode.Register1 = op.Register1;
                ri.Opcode.Type = OpcodeType.OneArg_Reg;
                ri.Opcode.SetSourceLine(op.SourceLine);
                arl.Add(ri);
                IncreaseAddress(ref address);
            }

            return (RomItem[])arl.ToArray(typeof(RomItem));
        }
        #endregion

        // 3rd pass
        #region private static void ResolveLabels(Dictionary<int, Opcode> binary)
        private static void ResolveLabels(Dictionary<int, Opcode> binary)
        {
            Dictionary<string, int> labels = new Dictionary<string, int>();
            List<RomItem> argImm16Labels = new List<RomItem>();
            List<RomItem> argImm15Labels = new List<RomItem>();
            List<RomItem> argImm4Labels = new List<RomItem>();

            // build dictionary with all labels
            foreach (int addr in binary.Keys)
            {
                if (binary[addr].Label != null)
                    labels.Add(binary[addr].Label, addr);

                if (binary[addr].Type == OpcodeType.TwoArg_RegImm16label ||
                        binary[addr].Type == OpcodeType.TwoArg_RegImm16label_h || binary[addr].Type == OpcodeType.TwoArg_RegImm16label_l)
                    argImm16Labels.Add(new RomItem(addr, binary[addr]));
                if (binary[addr].Type == OpcodeType.OneArg_Imm15label)
                    argImm15Labels.Add(new RomItem(addr, binary[addr]));
                if (binary[addr].Type == OpcodeType.ThreeArg_RegRegImm4label)
                    argImm4Labels.Add(new RomItem(addr, binary[addr]));
            }

            // resolve imm16labels
            foreach (RomItem ri in argImm16Labels)
            {
                try
                {
                    switch (ri.Opcode.Type)
                    {
                        case OpcodeType.TwoArg_RegImm16label:
                            ri.Opcode.Imm16 = labels[ri.Opcode.Imm16label];
                            ri.Opcode.Type = OpcodeType.TwoArg_RegImm16;
                            break;
                        case OpcodeType.TwoArg_RegImm16label_h:
                            ri.Opcode.Imm8 = labels[ri.Opcode.Imm16label] >> 8;
                            ri.Opcode.Type = OpcodeType.TwoArg_RegImm8;
                            break;
                        case OpcodeType.TwoArg_RegImm16label_l:
                            ri.Opcode.Imm8 = labels[ri.Opcode.Imm16label] & 0xFF;
                            ri.Opcode.Type = OpcodeType.TwoArg_RegImm8;
                            break;
                    }
                }
                catch (KeyNotFoundException)
                {
                    ri.Opcode.SourceLine.SetError("Label [" + ri.Opcode.Imm16label + "] cannot be found");
                }
            }

            // resolve imm15labels
            foreach (RomItem ri in argImm15Labels)
            {
                try
                {
                    int address = labels[ri.Opcode.Imm15label];

                    if (address > Math.Pow(2, 15))
                    {
                        ri.Opcode.SourceLine.SetError("Label [" + ri.Opcode.Imm15label + "] is too far to jump to");
                    }
                    else
                    {
                        ri.Opcode.Imm15 = address;
                        ri.Opcode.Type = OpcodeType.OneArg_Imm15;
                    }
                }
                catch (KeyNotFoundException)
                {
                    ri.Opcode.SourceLine.SetError("Label [" + ri.Opcode.Imm15label + "] cannot be found");
                }
            }

            // resolve imm4labels
            foreach (RomItem ri in argImm4Labels)
            {
                try
                {
                    int diff = labels[ri.Opcode.Imm4label] - ri.Address;

                    if (diff < -8 || diff > 7)
                    {
                        ri.Opcode.SourceLine.SetError("Label [" + ri.Opcode.Imm4label + "] is too far from the current address");
                    }
                    else
                    {
                        ri.Opcode.Imm4 = diff;
                        ri.Opcode.Type = OpcodeType.ThreeArg_RegRegImm4;
                    }
                }
                catch (KeyNotFoundException)
                {
                    ri.Opcode.SourceLine.SetError("Label [" + ri.Opcode.Imm4label + "] cannot be found");
                }
            }
        }
        #endregion
    }
}
