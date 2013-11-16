using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace SoC.Utils
{
    public partial class SpriteCalculator : Form
    {
        public SpriteCalculator()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdLoad = new OpenFileDialog();

            //ofdLoad.InitialDirectory = "c:\\";
            ofdLoad.Filter = "bmp files (*.bmp)|*.bmp|jpg files (*.jpg)|*.jpg";
            ofdLoad.FilterIndex = 1;
            ofdLoad.RestoreDirectory = true;

            if (ofdLoad.ShowDialog() == DialogResult.OK)
            {
                lblFile.Text = ofdLoad.FileName;
                Image i = Image.FromFile(ofdLoad.FileName);
                Bitmap b = (Bitmap) new Bitmap (i);

                StringBuilder sb = new StringBuilder();

                sb.Append(Path.GetFileNameWithoutExtension(ofdLoad.FileName) + " DW " + b.Width.ToString() + "  ; Width\r\n");
                sb.Append("DW " + b.Height.ToString() + "  ; Height\r\n");
                sb.Append("\r\n");

                for (int r = 0; r < b.Height; r++)
                {
                    for (int c = 0; c < b.Width; c++)
                    {
                        AddPaletteEntry(b.GetPixel(c, r));
                        sb.Append("DW " + GetPaletteIndex(b.GetPixel(c, r)) + "\r\n");
                    }
                }
                sb.Append("\r\n");
                sb.Append("\r\n");

                txtCode.Text += sb.ToString();
            }
        }

        int maxIndex = -1;
        private void AddPaletteEntry(Color c)
        {
            if (GetPaletteIndex(c) != -1)
                return;

            ListViewItem lvi = new ListViewItem();
            lvi.SubItems[0].Text = c.ToArgb().ToString("X");

            maxIndex++;
            lvi.SubItems.Add(maxIndex.ToString());

            lstPalette.Items.Add(lvi);
        }

        private int GetPaletteIndex(Color c)
        {
            int idx = -1;
            ListViewItem lvi;

            for (int i = 0; i < lstPalette.Items.Count; i++)
            {
                lvi = lstPalette.Items[i];
                if (lvi.Text == c.ToArgb().ToString("X"))
                {
                    idx = Convert.ToInt32(lvi.SubItems[1].Text);
                    break;
                }
            }

            return idx;
        }
    }
}
