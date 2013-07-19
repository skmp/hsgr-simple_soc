using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SoC.Entities;
using System.Reflection;

namespace SoC
{
    public partial class SoC : Form
    {
        public SoC()
        {
            InitializeComponent();
        }

        // Event handlers
        #region private void frmCPU_Load(object sender, EventArgs e)
        private void frmCPU_Load(object sender, EventArgs e)
        {
            frmCPU_Resize(sender, e);

            var doubleBufferPropertyInfo = lstBinary.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferPropertyInfo.SetValue(lstBinary, true, null);
            doubleBufferPropertyInfo.SetValue(lstRegister, true, null);
            doubleBufferPropertyInfo.SetValue(lstMemory, true, null);
        }
        #endregion
        #region private void frmCPU_Resize(object sender, EventArgs e)
        private void frmCPU_Resize(object sender, EventArgs e)
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

            lstRegister.Location = new Point(dx + lstBinary.Size.Width + dx, dy);
            lstRegister.Size = new Size(lstRegister.Size.Width, tbpEmulator.Size.Height - 2 * dy);

            lstMemory.Location = new Point(dx + lstBinary.Size.Width + dx + lstRegister.Size.Width + dx, dy);
            lstMemory.Size = new Size(lstMemory.Size.Width, tbpEmulator.Size.Height - 2 * dy);
        }
        #endregion
        #region private void tctMain_SelectedIndexChanged(object sender, EventArgs e)
        private void tctMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmCPU_Resize(sender, e);
        }
        #endregion

        // Load a new source file
        #region private void btnLoad_Click(object sender, EventArgs e)
        private void btnLoad_Click(object sender, EventArgs e)
        {
            tctMain.SelectedTab = tbpSource;
            Stream fileStream = null;
            OpenFileDialog ofdLoad = new OpenFileDialog();

            //ofdLoad.InitialDirectory = "c:\\";
            ofdLoad.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofdLoad.FilterIndex = 2;
            ofdLoad.RestoreDirectory = true;

            if (ofdLoad.ShowDialog() == DialogResult.OK)
            {
                try
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        #endregion
        // Assemble the source file
        #region private void btnAssemble_Click(object sender, EventArgs e)
        private void btnAssemble_Click(object sender, EventArgs e)
        {
            InitializeEmulator();
        }
        #endregion
        // Export the assembled source
        private void btnExport_Click(object sender, EventArgs e)
        {
            // export the compiled program in a format suitable to be used in the fpga
        }

        List<Line> Source = null;
        Dictionary<int, Opcode> Program = null;
        Emulator emulator = null;
        EmulatorDisplay display = null;

        #region private void btnDebugReset_Click(object sender, EventArgs e)
        private void btnDebugReset_Click(object sender, EventArgs e)
        {
            // reset the emulator state to the start options (probably has to be configurable)
            tctMain.SelectedTab = tbpEmulator;

            InitializeEmulator();
            emulator.Reset();
        }
        #endregion
        #region private void btnDebugStep_Click(object sender, EventArgs e)
        private void btnDebugStep_Click(object sender, EventArgs e)
        {
            try
            {
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
            if (display == null)
            {
                display = new EmulatorDisplay(emulator);
            }

            display.Show();
        }
        #endregion

        // private helpers
        private void InitializeEmulator()
        {
            Assembler a = new Assembler();

            // Parse the program
            StringReader sr = new StringReader(txtCode.Text);
            Source = a.Parse(sr);
            sr.Close();

            // Assemble the program
            Program = a.Assemble(Source);
            DisplaySource(Source);

            tctMain.SelectedTab = tbpEmulator;

            emulator = new Emulator(Program);
            emulator.ProgramCounterChanged += new ProgramCounterChangedEventHandler(emulator_ProgramCounterChanged);
            emulator.RegisterChanged += new RegisterChangedEventHandler(emulator_RegisterChanged);
            emulator.Reset();

            DisplayRegisters(emulator.Register);

        }

        void emulator_RegisterChanged(object o, RegisterChangedEventArgs e)
        {
            e.Register.ListViewItem.SubItems[1].Text = e.Register.ValueString;
            lstRegister.Refresh();
        }

        void emulator_ProgramCounterChanged(object o, ProgramCounterChangedEventArgs e)
        {
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