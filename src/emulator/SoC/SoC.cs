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

        // Load - Assemble - Save
        #region private void btnLoad_Click(object sender, EventArgs e)
        private void btnLoad_Click(object sender, EventArgs e)
        {
            tctMain.SelectedTab = tbpSource;
            Stream fileStream = null;
            OpenFileDialog ofdLoad = new OpenFileDialog();

            //ofdLoad.InitialDirectory = "c:\\";
            ofdLoad.Filter = "asm files (*.asm)|*.asm|bin files (*.bin)|*.bin";
            ofdLoad.FilterIndex = 2;
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
                                if (fileStream.Length != 32768)
                                {
                                    throw new Exception("File is not exactly 32768 bytes long");
                                }
                                using (BinaryReader reader = new BinaryReader(fileStream))
                                {
                                    int bytes = reader.Read(Memory, 0, 32768);
                                    if (bytes != 32768)
                                    {
                                        throw new Exception("Could not read exactly 32768 bytes");
                                    }
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

        }
        #endregion

        // Export
        private void btnExport_Click(object sender, EventArgs e)
        {
            // export the compiled program in a format suitable to be used in the fpga
        }

        List<Line> Source = null;
        Dictionary<int, Opcode> Program = null;
        byte[] Memory = null;
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

        // private helpers
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
            else
                DisplayBinary(Program);

            tctMain.SelectedTab = tbpEmulator;

            emulator = new Emulator(Program);
            emulator.ProgramCounterChanged += new ProgramCounterChangedEventHandler(emulator_ProgramCounterChanged);
            emulator.RegisterChanged += new RegisterChangedEventHandler(emulator_RegisterChanged);
            emulator.Reset();

            DisplayRegisters(emulator.Register);
        }

        void emulator_RegisterChanged(object o, RegisterChangedEventArgs e)
        {
            if (!chkRegisterDisplay.Checked)
                return;

            e.Register.ListViewItem.SubItems[1].Text = e.Register.ValueString;
            lstRegister.Refresh();
        }
        void emulator_ProgramCounterChanged(object o, ProgramCounterChangedEventArgs e)
        {
            if (!chkBinaryDisplay.Checked)
                return;

            if (e.OldLine != null)
            {
                e.OldLine.ListViewItem[(e.OldProgramCounter - e.OldLine.Opcodes[0].Address) / 2].BackColor = Color.White;
                e.OldLine.ListViewItem[(e.OldProgramCounter - e.OldLine.Opcodes[0].Address) / 2].ForeColor = Color.Black;
            }
            if (e.NewLine != null)
            {
                e.NewLine.ListViewItem[(e.NewProgramCounter - e.NewLine.Opcodes[0].Address) / 2].BackColor = Color.Green;
                e.NewLine.ListViewItem[(e.NewProgramCounter - e.NewLine.Opcodes[0].Address) / 2].ForeColor = Color.White;

                e.NewLine.ListViewItem[(e.NewProgramCounter - e.NewLine.Opcodes[0].Address) / 2].EnsureVisible();
            }

            lblProgramCounter.Text = "0x" + e.NewProgramCounter.ToString("X").PadLeft(4, '0');
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

            int labelPadding = 8;
            foreach (Opcode op in binary.Values)
                labelPadding = Math.Max(labelPadding, (op.Label != null) ? op.Label.Length : 0); 

            foreach (int addr in binary.Keys)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.SubItems[0].Text = addr.ToString("X");
                lvi.SubItems.Add("0x" + binary[addr].GetOpcodeValue().ToString("X"));
                lvi.SubItems.Add(binary[addr].GetNormalizedSourceLine(labelPadding));
                lvi.SubItems.Add(binary[addr].SourceLine.ErrorMessage);
                if (!String.IsNullOrEmpty(binary[addr].SourceLine.ErrorMessage))
                {
                    lvi.BackColor = Color.Red;
                    lvi.ForeColor = Color.White;
                }

                lstBinary.Items.Add(lvi);
            }
        }
        #endregion
    }
}