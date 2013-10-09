using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SoC.BL.Entities;
using System.Reflection;
using SoC.BL;
using SoC.BL.Events;

namespace SoC
{
    public partial class SoC : Form
    {
        public SoC()
        {
            InitializeComponent();
        }

        // Event handlers
        #region private void SoC_Load(object sender, EventArgs e)
        private void SoC_Load(object sender, EventArgs e)
        {
            SoC_Resize(sender, e);

            var doubleBufferPropertyInfo = lstBinary.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferPropertyInfo.SetValue(lstBinary, true, null);
            doubleBufferPropertyInfo.SetValue(lstRegister, true, null);
            doubleBufferPropertyInfo.SetValue(lstMemory, true, null);
        }
        #endregion
        #region private void SoC_Resize(object sender, EventArgs e)
        private void SoC_Resize(object sender, EventArgs e)
        {
            int dx = 12, dy = 12;

            grpActions.Location = new Point(dx, dy);
            grpActions.Size = new Size(grpActions.Size.Width, ClientSize.Height - 2 * dy);

            tctMain.Location = new Point(grpActions.Location.X + grpActions.Size.Width + dx, dy);
            tctMain.Size = new Size(ClientSize.Width - 3 * dx - grpActions.Size.Width, ClientSize.Height - 2 * dy);

            txtCode.Location = new Point(dx, dy);
            txtCode.Size = new Size(tbpSource.Size.Width - 2 * dx, tbpSource.Size.Height - 2 * dy);


            lblProgramCounter.Location = new Point(dx, dy);
            lblProgramCounter.Size = new Size(tbpSource.Size.Width - 2 * dx, lblProgramCounter.Height);

            lstBinary.Location = new Point(dx, 2 * dy + lblProgramCounter.Height);
            lstBinary.Size = new Size(tbpEmulator.Size.Width - lstRegister.Size.Width - lstMemory.Size.Width - 4 * dx, tbpEmulator.Size.Height - 3 * dy - lblProgramCounter.Height);
            chkBinaryDisplay.Location = new Point(lstBinary.Location.X + lstBinary.Size.Width - chkBinaryDisplay.Width, dy);

            lstRegister.Location = new Point(dx + lstBinary.Size.Width + dx, lstBinary.Location.Y);
            lstRegister.Size = new Size(lstRegister.Size.Width, lstBinary.Size.Height);
            chkRegisterDisplay.Location = new Point(lstRegister.Location.X + lstRegister.Size.Width - chkRegisterDisplay.Width, dy);

            lstMemory.Location = new Point(dx + lstBinary.Size.Width + dx + lstRegister.Size.Width + dx, lstBinary.Location.Y);
            lstMemory.Size = new Size(lstMemory.Size.Width, lstBinary.Size.Height);
            chkMemoryDisplay.Location = new Point(lstMemory.Location.X + lstMemory.Size.Width - chkMemoryDisplay.Width, dy);
        }
        #endregion
        #region private void SoC_FormClosing(object sender, FormClosingEventArgs e)
        private void SoC_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (emulator != null)
                emulator.Break();
        }
        #endregion
        #region private void tctMain_SelectedIndexChanged(object sender, EventArgs e)
        private void tctMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            SoC_Resize(sender, e);
        }
        #endregion
        #region private void txtCode_TextChanged(object sender, EventArgs e)
        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            Memory = null;
        }
        #endregion

        // Load - Assemble - Save - Export - SpriteCalculator
        #region private void btnLoad_Click(object sender, EventArgs e)
        private void btnLoad_Click(object sender, EventArgs e)
        {
            tctMain.SelectedTab = tbpSource;
            Stream fileStream = null;
            OpenFileDialog ofdLoad = new OpenFileDialog();

            //ofdLoad.InitialDirectory = "c:\\";
            ofdLoad.Filter = "asm files (*.asm)|*.asm|bin files (*.bin)|*.bin";
            ofdLoad.FilterIndex = 1;
            ofdLoad.RestoreDirectory = true;

            if (ofdLoad.ShowDialog() == DialogResult.OK)
            {
                Source = null;
                Program = null;
                Memory = null;

                try
                {
                    if (ofdLoad.FileName.ToLower().EndsWith("asm"))
                    {
                        if ((fileStream = ofdLoad.OpenFile()) != null)
                        {
                            using (fileStream)
                            {
                                using (StreamReader reader = new StreamReader(fileStream))
                                {
                                    txtCode.Text = reader.ReadToEnd();
                                }
                            }
                        }
                        else
                            throw new Exception("Cannot open asm file. [" + ofdLoad.FileName + "]");
                    }
                    else if (ofdLoad.FileName.ToLower().EndsWith("bin"))
                    {
                        if ((fileStream = ofdLoad.OpenFile()) != null)
                        {
                            using (fileStream)
                            {
                                byte[] fileContents;

                                if (fileStream.Length != 16384)
                                {
                                    throw new Exception("File is not exactly 16384 bytes long");
                                }
                                using (BinaryReader reader = new BinaryReader(fileStream))
                                {
                                    fileContents = new byte[16384];
                                    reader.Read(fileContents, 0, 16384);
                                }
                                Memory = new ushort[8192];
                                for (int addr = 0; addr < 8192; addr++)
                                {
                                    Memory[addr] = (ushort)(fileContents[2 * addr] | (fileContents[2 * addr + 1] << 8));
                                }
                            }
                        }
                        else
                            throw new Exception("Cannot open asm file. [" + ofdLoad.FileName + "]");
                    }
                    else
                    {
                        throw new Exception("Unknown filetype. [" + ofdLoad.FileName + "]");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not read file from disk. Error: " + ex.Message);
                }

                InitializeEmulator();
            }
        }
        #endregion
        #region private void btnAssemble_Click(object sender, EventArgs e)
        private void btnAssemble_Click(object sender, EventArgs e)
        {
            InitializeEmulator();
        }
        #endregion
        #region private void btnSave_Click(object sender, EventArgs e)
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Memory == null)
            {
                MessageBox.Show("Please first assemble your source.");
                return;
            }

            Stream fileStream = null;
            SaveFileDialog sfd = new SaveFileDialog();

            //ofdLoad.InitialDirectory = "c:\\";
            sfd.Filter = "bin files (*.bin)|*.bin";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if ((fileStream = sfd.OpenFile()) != null)
                {
                    using (fileStream)
                    {
                        using (BinaryWriter writer = new BinaryWriter(fileStream))
                        {
                            int len = Math.Min(Memory.Length, 8192);
                            for (int addr = 0; addr < len; addr++)
                            {
                                writer.Write((byte)(Memory[addr] & 0xff));
                                writer.Write((byte)((Memory[addr]>>8) & 0xff));
                            }
                        }
                    }
                }
                else
                    throw new Exception("Cannot open bin file. [" + sfd.FileName + "]");
            }
        }
        #endregion
        #region private void btnExport_Click(object sender, EventArgs e)
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (Memory == null)
            {
                MessageBox.Show("Please first assemble your source.");
                return;
            }

            Stream fileStream = null;
            SaveFileDialog sfd = new SaveFileDialog();

            //ofdLoad.InitialDirectory = "c:\\";
            sfd.Filter = "coe files (*.coe)|*.coe|mif files (*.mif)|*.mif";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if ((fileStream = sfd.OpenFile()) != null)
                {
                    if (sfd.FileName.ToLower().EndsWith("mif"))
                        exportMifFile(fileStream);
                    else
                        exportCoeFile(fileStream);
                }
                else
                    throw new Exception("Cannot open bin file. [" + sfd.FileName + "]");
            }
        }
        #endregion
        #region private void btnSpriteCalculator_Click(object sender, EventArgs e)
        private void btnSpriteCalculator_Click(object sender, EventArgs e)
        {
            SpriteCalculator w = new SpriteCalculator();
            w.Show();
        }
        #endregion

        List<Line> Source = null;
        Dictionary<int, Opcode> Program = null;
        UInt16[] Memory = null;
        Emulator emulator = null;
        EmulatorDisplay display = null;

        #region private void btnDebugReset_Click(object sender, EventArgs e)
        private void btnDebugReset_Click(object sender, EventArgs e)
        {
            try
            {
                // reset the emulator state to the start options (probably has to be configurable)
                tctMain.SelectedTab = tbpEmulator;

                InitializeEmulator();
                emulator.Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region private void btnDebugStep_Click(object sender, EventArgs e)
        private void btnDebugStep_Click(object sender, EventArgs e)
        {
            try
            {
                if (emulator == null)
                    return;

                tctMain.SelectedTab = tbpEmulator;
                emulator.Step();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region private void btnDebugRun_Click(object sender, EventArgs e)
        private void btnDebugRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (emulator == null)
                    return;

                tctMain.SelectedTab = tbpEmulator;
                emulator.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region private void btnDebugBreak_Click(object sender, EventArgs e)
        private void btnDebugBreak_Click(object sender, EventArgs e)
        {
            try
            {
                if (emulator == null)
                    return;

                tctMain.SelectedTab = tbpEmulator;
                emulator.Break();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region private void btnDebugDisplay_Click(object sender, EventArgs e)
        private void btnDebugDisplay_Click(object sender, EventArgs e)
        {
            if (emulator == null)
                return;

            // show the display as a popup (always on top?)
            if (display == null || display.IsDisposed)
            {
                display = new EmulatorDisplay(emulator);
            }

            display.Show();
        }
        #endregion

        // Emulator events
        #region void emulator_RegisterChanged(object o, RegisterChangedEventArgs e)
        void emulator_RegisterChanged(object o, RegisterChangedEventArgs e)
        {
            if (!chkRegisterDisplay.Checked)
                return;

            e.Register.ListViewItem.SubItems[1].Text = e.Register.ValueString;
            lstRegister.Refresh();
        }
        #endregion
        #region void emulator_ProgramCounterChanged(object o, ProgramCounterChangedEventArgs e)
        void emulator_ProgramCounterChanged(object o, ProgramCounterChangedEventArgs e)
        {
            if (!chkBinaryDisplay.Checked)
                return;

            if (e.OldLine != null)
            {
                e.OldLine.ListViewItem[e.OldProgramCounter - e.OldLine.Opcodes[0].Address].BackColor = Color.White;
                e.OldLine.ListViewItem[e.OldProgramCounter - e.OldLine.Opcodes[0].Address].ForeColor = Color.Black;
            }
            if (e.NewLine != null)
            {
                e.NewLine.ListViewItem[e.NewProgramCounter - e.NewLine.Opcodes[0].Address].BackColor = Color.Green;
                e.NewLine.ListViewItem[e.NewProgramCounter - e.NewLine.Opcodes[0].Address].ForeColor = Color.White;

                e.NewLine.ListViewItem[e.NewProgramCounter - e.NewLine.Opcodes[0].Address].EnsureVisible();
            }

            lblProgramCounter.Text = "0x" + e.NewProgramCounter.ToString("X").PadLeft(4, '0');
        }
        #endregion

        // Helper functions
        private void InitializeEmulator()
        {
            // Assemble the program
            if (Memory == null)
            {
                AssemblerOutput asout = Assembler.Assemble(txtCode.Text);
                Source = asout.Source;
                Program = asout.Binary;
                Memory = asout.Memory;
            }

            if (Source != null)
                DisplaySource(Source);
            else if (Program != null)
                DisplayBinary(Program);
            else
            {
                Program = ConvertMemoryToProgram(Memory);
                DisplayBinary(Program);
            }

            tctMain.SelectedTab = tbpEmulator;

            emulator = new Emulator(Program);
            emulator.ProgramCounterChanged += new ProgramCounterChangedEventHandler(emulator_ProgramCounterChanged);
            emulator.RegisterChanged += new RegisterChangedEventHandler(emulator_RegisterChanged);
            emulator.Reset();

            DisplayRegisters(emulator.Register);
        }
        #region private void DisplaySource(List<Line> source)
        private void DisplaySource(List<Line> source)
        {
            lstBinary.Items.Clear();
            foreach (Line line in source)
                lstBinary.Items.AddRange(line.ListViewItem);
        }
        #endregion
        #region private void DisplayRegisters()
        private void DisplayRegisters(Register[] list)
        {
            lstRegister.Items.Clear();

            foreach (Register reg in list)
                lstRegister.Items.Add(reg.ListViewItem);
        }
        #endregion
        #region private void DisplayBinary(Dictionary<int, Line> binary)
        private void DisplayBinary(Dictionary<int, Opcode> binary)
        {
            lstBinary.Items.Clear();

            int labelPadding = 10;
            foreach (Opcode op in binary.Values)
                labelPadding = Math.Max(labelPadding, (op.Label != null) ? op.Label.Length : 0);

            foreach (int addr in binary.Keys)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems[0].Text = addr.ToString("X").PadLeft(4, '0');
                lvi.SubItems.Add("0x" + binary[addr].GetOpcodeValue().ToString("X").PadLeft(4, '0'));
                lvi.SubItems.Add(binary[addr].GetNormalizedSourceLine(labelPadding));
                if (binary[addr].SourceLine != null)
                {
                    lvi.SubItems.Add(binary[addr].SourceLine.ErrorMessage);
                    if (!String.IsNullOrEmpty(binary[addr].SourceLine.ErrorMessage))
                    {
                        lvi.BackColor = Color.Red;
                        lvi.ForeColor = Color.White;
                    }
                }

                lstBinary.Items.Add(lvi);
            }
        }
        #endregion
        #region private Dictionary<int, Opcode> ConvertMemoryToProgram(UInt16[] memory)
        private Dictionary<int, Opcode> ConvertMemoryToProgram(UInt16[] memory)
        {
            Dictionary<int, Opcode> binary = new Dictionary<int, Opcode>();

            for (int addr=0; addr<8192; addr++)
            {
                binary.Add(addr, OpcodeDictionary.Get(memory[addr]));
            }

            return binary;
        }
        #endregion
        #region private void exportMifFile(Stream fileStream)
        private void exportMifFile(Stream fileStream)
        {
            using (fileStream)
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine("DEPTH = 8192");
                    writer.WriteLine("WIDTH = 16");
                    writer.WriteLine("ADDRESS_RADIX = HEX");
                    writer.WriteLine("DATA_RADIX = HEX");
                    writer.WriteLine("CONTENT");
                    writer.WriteLine("BEGIN");
                    writer.Write("0 : ");
                    int len = 8192;
                    for (int addr = 0; addr < len; addr++)
                    {
                        writer.Write(Memory[addr].ToString("X") + " ");
                    }
                    writer.WriteLine(";");
                    writer.WriteLine("END");
                }
            }
        }
        #endregion
        #region private void exportCoeFile(Stream fileStream)
        private void exportCoeFile(Stream fileStream)
        {
            using (fileStream)
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine("MEMORY_INITIALIZATION_RADIX=16;");
                    writer.WriteLine("MEMORY_INITIALIZATION_VECTOR=");
                    int len = 8192;
                    for (int addr = 0; addr < len-1; addr++)
                    {
                        writer.WriteLine(Memory[addr].ToString("X") + ",");
                    }
                    writer.WriteLine(Memory[len-1].ToString("X") + ";");
                }
            }
        }
        #endregion
    }
}