using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using SoC.Emulator;
using SoC.Emulator.Events;
using System.Drawing;
using SoC.Assembler.Entities;

namespace SoC.Device.Emulator
{
    class EmulatorDevice : IDevice
    {
        EmulatorMain emulator = null;
        EmulatorDisplay display = null;

        // IDevice implementation
        #region public string DeviceName
        public string DeviceName
        {
            get { return "Emulator"; }
        }
        #endregion
        #region public bool CanReset
        public bool CanReset
        {
            get { return true; }
        }
        #endregion
        #region public bool CanHalt
        public bool CanHalt
        {
            get { return true; }
        }
        #endregion
        #region public bool CanResume
        public bool CanResume
        {
            get { return true; }
        }
        #endregion
        #region public bool CanStep
        public bool CanStep
        {
            get { return true; }
        }
        #endregion
        #region public bool CanConfigure
        public bool CanConfigure
        {
            get { return false; }
        }
        #endregion
        #region public bool CanDisplay
        public bool CanDisplay
        {
            get { return true; }
        }
        #endregion
        #region public void Configure()
        public void Configure()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region public void Instantiate()
        public void Instantiate()
        {
            emulator = new EmulatorMain(SoC.Program);
            emulator.ProgramCounterChanged += new ProgramCounterChangedEventHandler(emulator_ProgramCounterChanged);
            emulator.RegisterChanged += new RegisterChangedEventHandler(emulator_RegisterChanged);
            emulator.Reset();
        }
        #endregion
        public byte WriteCommand(byte command)
        {
            switch (command)
            {
                case 0xb1: // Reset
                    //tctMain.SelectedTab = tbpDevice;
                    //InitializeEmulator();
                    emulator.Reset();
                    break;
                case 0xb2: // Halt
                    //tctMain.SelectedTab = tbpDevice;
                    emulator.Break();
                    break;
                case 0xb3: // Resume
                    //tctMain.SelectedTab = tbpDevice;
                    emulator.Run();
                    break;
                case 0xb4: // Step
                    //tctMain.SelectedTab = tbpDevice;
                    emulator.Step();
                    break;
            }

            return command;
        }
        #region public void Destroy()
        public void Destroy()
        {
            if (display != null)
            {
                display.Close();
                display = null;
            }

            emulator.Break();
            emulator = null;
        }
        #endregion
        #region public void ShowDisplay()
        public void ShowDisplay()
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
            if (!Program.MainWindow.chkRegisterDisplay.Checked)
                return;

            e.Register.ListViewItem.SubItems[1].Text = e.Register.ValueString;
            Program.MainWindow.lstRegister.Refresh();
        }
        #endregion
        #region void emulator_ProgramCounterChanged(object o, ProgramCounterChangedEventArgs e)
        void emulator_ProgramCounterChanged(object o, ProgramCounterChangedEventArgs e)
        {
            if (!Program.MainWindow.chkBinaryDisplay.Checked)
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

            Program.MainWindow.lblProgramCounter.Text = "0x" + e.NewProgramCounter.ToString("X").PadLeft(4, '0');
        }
        #endregion

        #region private void DisplayRegisters()
        private void DisplayRegisters(Register[] list)
        {
            Program.MainWindow.lstRegister.Items.Clear();

            foreach (Register reg in list)
                Program.MainWindow.lstRegister.Items.Add(reg.ListViewItem);
        }
        #endregion

        #region public override string ToString()
        public override string ToString()
        {
            return DeviceName;
        }
        #endregion
    }
}
