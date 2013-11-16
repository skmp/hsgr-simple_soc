namespace SoC.Utils
{
    partial class SpriteCalculator
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
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lstPalette = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(13, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(94, 18);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(72, 13);
            this.lblFile.TabIndex = 1;
            this.lblFile.Text = "No file loaded";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(12, 42);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCode.Size = new System.Drawing.Size(281, 268);
            this.txtCode.TabIndex = 2;
            // 
            // lstPalette
            // 
            this.lstPalette.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstPalette.Location = new System.Drawing.Point(300, 42);
            this.lstPalette.Name = "lstPalette";
            this.lstPalette.Size = new System.Drawing.Size(302, 268);
            this.lstPalette.TabIndex = 3;
            this.lstPalette.UseCompatibleStateImageBehavior = false;
            this.lstPalette.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Color";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Index";
            // 
            // SpriteCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 322);
            this.Controls.Add(this.lstPalette);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.btnLoad);
            this.Name = "SpriteCalculator";
            this.Text = "SpriteCalculator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.ListView lstPalette;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}