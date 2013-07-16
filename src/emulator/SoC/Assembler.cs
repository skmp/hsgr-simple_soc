using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SoC.Entities;
using System.Collections;


namespace SoC
{
    public class Assembler
    {
        private int address = 0;

        #region public List<Line> Parse(TextReader reader)
        public List<Line> Parse(TextReader reader)
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
        #region public Dictionary<int, Line> Assemble(List<Line> source)
        public Dictionary<int, Opcode> Assemble(List<Line> source)
        {
            Dictionary<int, Opcode> binary = new Dictionary<int, Opcode>();
            Opcode op = null;
            RomItem[] ria = null;

            address = 0;
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
                        address++;
                    }
                    else // Handle Assembler pseudo instructions
                    {
                        ria = HandlePseudoInstruction(op);
                    }

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

            ResolveLabels(binary);

            return binary;
        }
        #endregion

        // Private helpers
        #region private Opcode AssembleLine(Line line)
        private Opcode AssembleLine(Line line)
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
        #region private void ResolveLabels(Dictionary<int, Opcode> binary)
        private void ResolveLabels(Dictionary<int, Opcode> binary)
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
                        binary[addr].Type == OpcodeType.TwoArg_RegImm8_label16h || binary[addr].Type == OpcodeType.TwoArg_RegImm8_label16l)
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
                        case OpcodeType.TwoArg_RegImm8_label16h:
                            ri.Opcode.Imm8 = labels[ri.Opcode.Imm16label] >> 8;
                            ri.Opcode.Type = OpcodeType.TwoArg_RegImm8;
                            break;
                        case OpcodeType.TwoArg_RegImm8_label16l:
                            ri.Opcode.Imm8 = labels[ri.Opcode.Imm16label] & 0xFF;
                            ri.Opcode.Type = OpcodeType.TwoArg_RegImm8;
                            break;
                    }
                }
                catch (KeyNotFoundException)
                {
                    ri.Opcode.SourceLine.SetError("Label [" + ri.Opcode.Imm15label + "] cannot be found");
                }
            }

            // resolve imm15labels
            foreach (RomItem ri in argImm15Labels)
            {
                try
                {
                    ri.Opcode.Imm15 = labels[ri.Opcode.Imm15label];
                    ri.Opcode.Type = OpcodeType.OneArg_Imm15;
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

                    if (ri.Opcode.Command == "ba" && diff >= 0)
                    {
                        ri.Opcode.SourceLine.SetError("Label [" + ri.Opcode.Imm4label + "] must be before the current address");
                    }
                    else if (ri.Opcode.Command != "ba" && diff <= 0)
                    {
                        ri.Opcode.SourceLine.SetError("Label [" + ri.Opcode.Imm4label + "] must be after the current address");
                    }
                    else if (diff < -15 || diff > 15)
                    {
                        ri.Opcode.SourceLine.SetError("Label [" + ri.Opcode.Imm4label + "] is too far from the current address");
                    }
                    else
                    {
                        ri.Opcode.Imm4 = Math.Abs(diff);
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
        #region private RomItem[] HandlePseudoInstruction(Opcode op)
        private RomItem[] HandlePseudoInstruction(Opcode op)
        {
            ArrayList arl = new ArrayList();
            RomItem ri;

            if (op.Command == "org")     // ORiGin
            {
                address = op.Imm16;
            }
            else if (op.Command == "li") // Load Immediate
            {
                ri = new RomItem(address, OpcodeDictionary.Get("movh"));
                ri.Opcode.Register1 = op.Register1;
                ri.Opcode.Imm8 = op.Imm16 >> 8;
                ri.Opcode.Type = OpcodeType.TwoArg_RegImm8;
                ri.Opcode.SetSourceLine(op.SourceLine);
                arl.Add(ri);
                address++;

                ri = new RomItem(address, OpcodeDictionary.Get("movl"));
                ri.Opcode.Register1 = op.Register1;
                ri.Opcode.Imm8 = op.Imm16 & 0xFF;
                ri.Opcode.Type = OpcodeType.TwoArg_RegImm8;
                ri.Opcode.SetSourceLine(op.SourceLine);
                arl.Add(ri);
                address++;
            }
            else if (op.Command == "jrl") // JumpRegisterLabel
            {
                ri = new RomItem(address, OpcodeDictionary.Get("movh"));
                ri.Opcode.Register1 = op.Register1;
                //ri.Opcode.Imm8 = op.Imm16 >> 8;
                ri.Opcode.Imm16label = op.Imm16label;
                ri.Opcode.Type = OpcodeType.TwoArg_RegImm8_label16h;
                ri.Opcode.SetSourceLine(op.SourceLine);
                arl.Add(ri);
                address++;

                ri = new RomItem(address, OpcodeDictionary.Get("movl"));
                ri.Opcode.Register1 = op.Register1;
                //ri.Opcode.Imm8 = op.Imm16 & 0xFF;
                ri.Opcode.Imm16label = op.Imm16label;
                ri.Opcode.Type = OpcodeType.TwoArg_RegImm8_label16l;
                ri.Opcode.SetSourceLine(op.SourceLine);
                arl.Add(ri);
                address++;

                ri = new RomItem(address, OpcodeDictionary.Get("jr"));
                ri.Opcode.Register1 = op.Register1;
                ri.Opcode.Type = OpcodeType.OneArg_Reg;
                ri.Opcode.SetSourceLine(op.SourceLine);
                arl.Add(ri);
                address++;
            }

            return (RomItem[])arl.ToArray(typeof(RomItem));
        }
        #endregion
    }
}
