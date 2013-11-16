using System;
using System.Drawing;
using System.Windows.Forms;
using SoC.Assembler;
using SoC.Emulator;
using SoC.Emulator.Events;
using System.Drawing.Imaging;

namespace SoC.Emulator
{
    public partial class EmulatorDisplay : Form
    {
        private double ZoomFactor = 1;
        private EmulatorMain emulator;
        private Bitmap bitmap;
        //Color[] palette = { Color.Black, Color.Red, Color.LightGray, Color.DarkBlue, Color.Red, Color.Purple, Color.Yellow, Color.White };
        Color[] palette = { Color.Black, Color.Red, Color.Blue, Color.Magenta, Color.Green, Color.Yellow, Color.Cyan, Color.White };

        public EmulatorDisplay(EmulatorMain emulator)
        {
            InitializeComponent();
            this.emulator = emulator;
        }

        // Event handlers
        #region private void EmulatorDisplay_Load(object sender, EventArgs e)
        private void EmulatorDisplay_Load(object sender, EventArgs e)
        {
            bitmap = new Bitmap(emulator.Display.GetLength(0), emulator.Display.GetLength(1), PixelFormat.Format32bppPArgb);

            for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
            {
                for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
                {
                    byte c = (byte)(emulator.Display[Xcount, Ycount] & 7);

                    bitmap.SetPixel(Xcount, Ycount, palette[c]);
                }
            }

            ClientSize = new Size(Convert.ToInt32(bitmap.Width * ZoomFactor), Convert.ToInt32(bitmap.Height * ZoomFactor));

            emulator.DisplayMemoryChanged += new DisplayMemoryChangedEventHandler(ShowOnScreen);

            pboxDisplay.Image = bitmap;
        }
        #endregion
        #region private void EmulatorDisplay_Resize(object sender, EventArgs e)
        private void EmulatorDisplay_Resize(object sender, EventArgs e)
        {
            if (ClientSize.Width > ClientSize.Height)
            {
                ZoomFactor = ClientSize.Height * 1.0 / bitmap.Height * 1.0;
            }
            else
            {
                ZoomFactor = ClientSize.Width * 1.0 / bitmap.Width * 1.0;
            }

            pboxDisplay.Left = 0;
            pboxDisplay.Top = 0;
            pboxDisplay.Width = Convert.ToInt32(bitmap.Width * ZoomFactor);
            pboxDisplay.Height = Convert.ToInt32(bitmap.Height * ZoomFactor);
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
            int c = e.NewColor;
            
            // Called on wait command to simulate VSync
            if (c == -1)
            {
                pboxDisplay.Invalidate();
                return;
            }
            else if (c < 0 || c > 7)  // sanity check
            {
                return;
            }
            else
            {
                bitmap.SetPixel(e.X, e.Y, palette[c]);
                //pboxDisplay.Invalidate(new Rectangle((int)(e.X * ZoomFactor - ZoomFactor), (int)(e.Y * ZoomFactor - ZoomFactor), (int)(2 * ZoomFactor), (int)(2 * ZoomFactor)));
            }
        }
    }
}
