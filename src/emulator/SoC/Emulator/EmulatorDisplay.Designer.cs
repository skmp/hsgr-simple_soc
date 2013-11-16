namespace SoC.Emulator
{
    partial class EmulatorDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pboxDisplay = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pboxDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // pboxDisplay
            // 
            this.pboxDisplay.Location = new System.Drawing.Point(0, 0);
            this.pboxDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.pboxDisplay.Name = "pboxDisplay";
            this.pboxDisplay.Size = new System.Drawing.Size(200, 200);
            this.pboxDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pboxDisplay.TabIndex = 0;
            this.pboxDisplay.TabStop = false;
            // 
            // EmulatorDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.pboxDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "EmulatorDisplay";
            this.Text = "EmulatorDisplay";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EmulatorDisplay_FormClosing);
            this.Load += new System.EventHandler(this.EmulatorDisplay_Load);
            this.Resize += new System.EventHandler(this.EmulatorDisplay_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pboxDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pboxDisplay;

    }
}