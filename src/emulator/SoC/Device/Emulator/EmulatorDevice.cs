using System;
using System.IO.Ports;
using SoC.Emulator;
using SoC.Emulator.Events;
using System.Drawing;
using SoC.Assembler.Entities;
using System.Windows.Forms;

namespace SoC.Device.Emulator
{
    class EmulatorDevice : IDevice
    {
        EmulatorMain emulator = null;
        EmulatorDisplay display = null;
        private int data = 0;
        private int address = 0;

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

            DisplayRegisters(emulator.Register);

            emulator.ProgramCounterChanged += new ProgramCounterChangedEventHandler(emulator_ProgramCounterChanged);
            emulator.RegisterChanged += new RegisterChangedEventHandler(emulator_RegisterChanged);
            emulator.Reset();
        }
        #endregion
        #region public byte WriteCommand(byte command)
        public byte WriteCommand(byte command)
        {
            byte response = command;

            switch (command)
            {
                case 0x80:
                    response = (byte)(address & 0x000f);
                    break;
                case 0x81:
                    response = (byte)(0x10 | ((address >> 4) & 0x000f));
                    break;
                case 0x82:
                    response = (byte)(0x20 | ((address >> 8) & 0x000f));
                    break;
                case 0x83:
                    response = (byte)(0x30 | ((address >> 12) & 0x000f));
                    break;

                case 0x84:
                    response = (byte)(0x40 | (data & 0x000f));
                    break;
                case 0x85:
                    response = (byte)(0x50 | ((data >> 4) & 0x000f));
                    break;
                case 0x86:
                    response = (byte)(0x60 | ((data >> 8) & 0x000f));
                    break;
                case 0x87:
                    response = (byte)(0x70 | ((data >> 12) & 0x000f));
                    break;

                case 0x88: // Read mame to data
                    data = emulator.Memory[address];
                    break;
                case 0x89: // Write data to mem
                    emulator.Memory[address] = (ushort)data;
                    break;

                case 0xb0: // Reset = 0
                    //emulator.Reset();
                    break;
                case 0xb1: // Reset = 1
                    emulator.Reset();
                    break;
                case 0xb2: // Halt
                    emulator.Break();
                    break;
                case 0xb3: // Resume
                    emulator.Run();
                    break;
                case 0xb4: // Step
                    emulator.Step();
                    break;

                case 0xb5:
                    break;
                case 0xb6:
                    data = emulator.ProgramCounter;
                    break;

                default:
                    if (command >= 0x00 && command <= 0x7f)
                    {
                        if ((command & 0xf0) == 0x00)
                            address |= (byte)(command & 0x000f);
                        if ((command & 0xf0) == 0x10)
                            address |= (byte)((command & 0x000f) << 4);
                        if ((command & 0xf0) == 0x20)
                            address |= (byte)((command & 0x000f) << 8);
                        if ((command & 0xf0) == 0x30)
                            address |= (byte)((command & 0x000f) << 12);

                        break;
                    }
                    if (command >= 0x40 && command <= 0x7f)
                    {
                        if ((command & 0xf0) == 0x40)
                            data |= (byte)(command & 0x000f);
                        if ((command & 0xf0) == 0x50)
                            data |= (byte)((command & 0x000f) << 4);
                        if ((command & 0xf0) == 0x60)
                            data |= (byte)((command & 0x000f) << 8);
                        if ((command & 0xf0) == 0x70)
                            data |= (byte)((command & 0x000f) << 12);

                        break;
                    }
                    if (command >= 0x90 && command <= 0x9f)
                    {
                        emulator.Register[command & 0x0f].Value.UValue = (ushort)data;

                        break;
                    }
                    if (command >= 0xa0 && command <= 0xaf)
                    {
                        data = emulator.Register[command & 0x0f].Value.UValue;

                        break;
                    }

                    throw new Exception("Unknown command: 0x" + command.ToString("X"));
            }

            return response;
        }
        #endregion
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

            //SoC.Program[e.OldProgramCounter]
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