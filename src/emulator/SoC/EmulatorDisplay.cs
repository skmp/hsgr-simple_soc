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
using System.Drawing.Imaging;

namespace SoC
{
    public partial class EmulatorDisplay : Form
    {
        private const int ZoomFactor = 1;
        // Reference to the emulator
        private Emulator emulator;
        // The display
        private Bitmap bitmap;

        Color[] palette = { Color.Black, Color.Blue, Color.Green, Color.Cyan, Color.Red, Color.Purple, Color.Yellow, Color.White };


        public EmulatorDisplay(Emulator emulator)
        {
            InitializeComponent();

            this.emulator = emulator;
        }

        // Event handlers
        #region private void EmulatorDisplay_Paint(object sender, PaintEventArgs e)
        private void EmulatorDisplay_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(e.ClipRectangle.X / ZoomFactor, e.ClipRectangle.Y / ZoomFactor, e.ClipRectangle.Width / ZoomFactor, e.ClipRectangle.Height / ZoomFactor);
            e.Graphics.DrawImage(bitmap, e.ClipRectangle, r, GraphicsUnit.Pixel);
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
            bitmap = new Bitmap(emulator.Display.GetLength(0), emulator.Display.GetLength(1), PixelFormat.Format32bppPArgb);

            // Set each pixel in myBitmap to black. 
            // Set the border to red.
            for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
            {
                for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
                {
                    byte c = emulator.Display[Xcount/ZoomFactor, Ycount/ZoomFactor];
                    // out of range colors set to black
                    if (c > 7)
                        c = 0;

                    bitmap.SetPixel(Xcount, Ycount, palette[c]);
                }
            }

            ClientSize = new Size(bitmap.Width * ZoomFactor, bitmap.Height * ZoomFactor);
            Refresh();

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
            int c = e.NewColor;
            // out of range colors set to black
            if (c > 7)
                c = 0;

            bitmap.SetPixel(e.X, e.Y, palette[c]);
            Invalidate(new Rectangle(e.X * ZoomFactor, e.Y * ZoomFactor, 1 * ZoomFactor, 1 * ZoomFactor));
        }
    }
}
