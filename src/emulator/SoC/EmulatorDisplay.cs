using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SoC.BL;
using SoC.BL.Events;

namespace SoC
{
    public partial class EmulatorDisplay : Form
    {
        // Reference to the emulator
        private Emulator emulator;
        // The display
        private Bitmap display;

        public EmulatorDisplay(Emulator emulator)
        {
            InitializeComponent();

            this.emulator = emulator;
        }

        // Event handlers
        #region private void EmulatorDisplay_Paint(object sender, PaintEventArgs e)
        private void EmulatorDisplay_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(display, 0, 0, ClientSize.Width, ClientSize.Height);
        }
        #endregion
        #region private void EmulatorDisplay_Resize(object sender, EventArgs e)
        private void EmulatorDisplay_Resize(object sender, EventArgs e)
        {
            Refresh();
        }
        #endregion
        #region private void EmulatorDisplay_Load(object sender, EventArgs e)
        private void EmulatorDisplay_Load(object sender, EventArgs e)
        {
            // TODO: Get the display size from settings??
            display = new Bitmap(200, 200);

            // Set each pixel in myBitmap to black. 
            // Set the border to red.
            for (int Xcount = 0; Xcount < display.Width; Xcount++)
            {
                for (int Ycount = 0; Ycount < display.Height; Ycount++)
                {
                    if (Xcount == 0 || Ycount == 0 || Xcount == display.Width - 1 || Ycount == display.Height - 1)
                        display.SetPixel(Xcount, Ycount, Color.Red);
                    else
                        display.SetPixel(Xcount, Ycount, Color.Black);
                }
            }

            ClientSize = new Size(3 * display.Width, 3 * display.Height);

            emulator.DisplayMemoryChanged += new DisplayMemoryChangedEventHandler(ShowOnScreen);
        }
        #endregion
        #region private void EmulatorDisplay_FormClosing(object sender, FormClosingEventArgs e)
        private void EmulatorDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            emulator.DisplayMemoryChanged -= new DisplayMemoryChangedEventHandler(ShowOnScreen);
        }
        #endregion

        public void ShowOnScreen(object o, DisplayMemoryChangedEventArgs e)
        {
            display.SetPixel(e.X, e.Y, e.Color);
            Refresh();
        }
    }
}
