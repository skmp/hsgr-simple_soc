namespace SoC
{
    partial class SoC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoC));
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnAssemble = new System.Windows.Forms.Button();
            this.lstBinary = new System.Windows.Forms.ListView();
            this.colHAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHOpcode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHLine = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHError = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tctMain = new System.Windows.Forms.TabControl();
            this.tbpSource = new System.Windows.Forms.TabPage();
            this.tbpEmulator = new System.Windows.Forms.TabPage();
            this.lblProgramCounter = new System.Windows.Forms.Label();
            this.lstMemory = new System.Windows.Forms.ListView();
            this.colHMemoryName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHMemoryValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstRegister = new System.Windows.Forms.ListView();
            this.colHRegName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHRegValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.grpDebug = new System.Windows.Forms.GroupBox();
            this.btnDebugBreak = new System.Windows.Forms.Button();
            this.btnDebugRun = new System.Windows.Forms.Button();
            this.btnDebugDisplay = new System.Windows.Forms.Button();
            this.btnDebugStep = new System.Windows.Forms.Button();
            this.btnDebugReset = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.tctMain.SuspendLayout();
            this.tbpSource.SuspendLayout();
            this.tbpEmulator.SuspendLayout();
            this.grpActions.SuspendLayout();
            this.grpDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(6, 6);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCode.Size = new System.Drawing.Size(384, 484);
            this.txtCode.TabIndex = 0;
            this.txtCode.Text = resources.GetString("txtCode.Text");
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            // 
            // btnAssemble
            // 
            this.btnAssemble.Location = new System.Drawing.Point(27, 57);
            this.btnAssemble.Name = "btnAssemble";
            this.btnAssemble.Size = new System.Drawing.Size(75, 23);
            this.btnAssemble.TabIndex = 1;
            this.btnAssemble.Text = "Assemble";
            this.btnAssemble.UseVisualStyleBackColor = true;
            this.btnAssemble.Click += new System.EventHandler(this.btnAssemble_Click);
            // 
            // lstBinary
            // 
            this.lstBinary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHAddress,
            this.colHOpcode,
            this.colHLine,
            this.colHError});
            this.lstBinary.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lstBinary.Location = new System.Drawing.Point(6, 34);
            this.lstBinary.Name = "lstBinary";
            this.lstBinary.Size = new System.Drawing.Size(435, 452);
            this.lstBinary.TabIndex = 3;
            this.lstBinary.UseCompatibleStateImageBehavior = false;
            this.lstBinary.View = System.Windows.Forms.View.Details;
            // 
            // colHAddress
            // 
            this.colHAddress.Text = "Addr";
            // 
            // colHOpcode
            // 
            this.colHOpcode.Text = "Opcode";
            this.colHOpcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // colHLine
            // 
            this.colHLine.Text = "Line";
            this.colHLine.Width = 400;
            // 
            // colHError
            // 
            this.colHError.Text = "";
            this.colHError.Width = 300;
            // 
            // tctMain
            // 
            this.tctMain.Controls.Add(this.tbpSource);
            this.tctMain.Controls.Add(this.tbpEmulator);
            this.tctMain.Location = new System.Drawing.Point(144, 14);
            this.tctMain.Name = "tctMain";
            this.tctMain.SelectedIndex = 0;
            this.tctMain.Size = new System.Drawing.Size(743, 522);
            this.tctMain.TabIndex = 4;
            this.tctMain.SelectedIndexChanged += new System.EventHandler(this.tctMain_SelectedIndexChanged);
            // 
            // tbpSource
            // 
            this.tbpSource.Controls.Add(this.txtCode);
            this.tbpSource.Location = new System.Drawing.Point(4, 22);
            this.tbpSource.Name = "tbpSource";
            this.tbpSource.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSource.Size = new System.Drawing.Size(735, 496);
            this.tbpSource.TabIndex = 0;
            this.tbpSource.Text = "Source";
            this.tbpSource.UseVisualStyleBackColor = true;
            // 
            // tbpEmulator
            // 
            this.tbpEmulator.Controls.Add(this.lblProgramCounter);
            this.tbpEmulator.Controls.Add(this.lstMemory);
            this.tbpEmulator.Controls.Add(this.lstRegister);
            this.tbpEmulator.Controls.Add(this.lstBinary);
            this.tbpEmulator.Location = new System.Drawing.Point(4, 22);
            this.tbpEmulator.Name = "tbpEmulator";
            this.tbpEmulator.Padding = new System.Windows.Forms.Padding(3);
            this.tbpEmulator.Size = new System.Drawing.Size(735, 496);
            this.tbpEmulator.TabIndex = 1;
            this.tbpEmulator.Text = "Emulator";
            this.tbpEmulator.UseVisualStyleBackColor = true;
            // 
            // lblProgramCounter
            // 
            this.lblProgramCounter.AutoSize = true;
            this.lblProgramCounter.Location = new System.Drawing.Point(3, 10);
            this.lblProgramCounter.Name = "lblProgramCounter";
            this.lblProgramCounter.Size = new System.Drawing.Size(83, 13);
            this.lblProgramCounter.TabIndex = 6;
            this.lblProgramCounter.Text = "ProgramCounter";
            // 
            // lstMemory
            // 
            this.lstMemory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHMemoryName,
            this.colHMemoryValue});
            this.lstMemory.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstMemory.Location = new System.Drawing.Point(447, 34);
            this.lstMemory.Name = "lstMemory";
            this.lstMemory.Size = new System.Drawing.Size(129, 257);
            this.lstMemory.TabIndex = 5;
            this.lstMemory.UseCompatibleStateImageBehavior = false;
            this.lstMemory.View = System.Windows.Forms.View.Details;
            // 
            // colHMemoryName
            // 
            this.colHMemoryName.Text = "Addr";
            // 
            // colHMemoryValue
            // 
            this.colHMemoryValue.Text = "Value";
            // 
            // lstRegister
            // 
            this.lstRegister.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHRegName,
            this.colHRegValue});
            this.lstRegister.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstRegister.Location = new System.Drawing.Point(513, 34);
            this.lstRegister.Name = "lstRegister";
            this.lstRegister.Size = new System.Drawing.Size(207, 257);
            this.lstRegister.TabIndex = 4;
            this.lstRegister.UseCompatibleStateImageBehavior = false;
            this.lstRegister.View = System.Windows.Forms.View.Details;
            // 
            // colHRegName
            // 
            this.colHRegName.Text = "Reg";
            // 
            // colHRegValue
            // 
            this.colHRegValue.Text = "Value";
            this.colHRegValue.Width = 120;
            // 
            // grpActions
            // 
            this.grpActions.Controls.Add(this.btnSave);
            this.grpActions.Controls.Add(this.btnExport);
            this.grpActions.Controls.Add(this.grpDebug);
            this.grpActions.Controls.Add(this.btnLoad);
            this.grpActions.Controls.Add(this.btnAssemble);
            this.grpActions.Location = new System.Drawing.Point(13, 13);
            this.grpActions.Name = "grpActions";
            this.grpActions.Size = new System.Drawing.Size(125, 523);
            this.grpActions.TabIndex = 5;
            this.grpActions.TabStop = false;
            this.grpActions.Text = "Actions";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(26, 86);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(27, 115);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // grpDebug
            // 
            this.grpDebug.Controls.Add(this.btnDebugBreak);
            this.grpDebug.Controls.Add(this.btnDebugRun);
            this.grpDebug.Controls.Add(this.btnDebugDisplay);
            this.grpDebug.Controls.Add(this.btnDebugStep);
            this.grpDebug.Controls.Add(this.btnDebugReset);
            this.grpDebug.Location = new System.Drawing.Point(7, 175);
            this.grpDebug.Name = "grpDebug";
            this.grpDebug.Size = new System.Drawing.Size(112, 234);
            this.grpDebug.TabIndex = 3;
            this.grpDebug.TabStop = false;
            this.grpDebug.Text = "Debug";
            // 
            // btnDebugBreak
            // 
            this.btnDebugBreak.Location = new System.Drawing.Point(19, 106);
            this.btnDebugBreak.Name = "btnDebugBreak";
            this.btnDebugBreak.Size = new System.Drawing.Size(75, 23);
            this.btnDebugBreak.TabIndex = 8;
            this.btnDebugBreak.Text = "Break";
            this.btnDebugBreak.UseVisualStyleBackColor = true;
            this.btnDebugBreak.Click += new System.EventHandler(this.btnDebugBreak_Click);
            // 
            // btnDebugRun
            // 
            this.btnDebugRun.Location = new System.Drawing.Point(20, 77);
            this.btnDebugRun.Name = "btnDebugRun";
            this.btnDebugRun.Size = new System.Drawing.Size(75, 23);
            this.btnDebugRun.TabIndex = 7;
            this.btnDebugRun.Text = "Run";
            this.btnDebugRun.UseVisualStyleBackColor = true;
            this.btnDebugRun.Click += new System.EventHandler(this.btnDebugRun_Click);
            // 
            // btnDebugDisplay
            // 
            this.btnDebugDisplay.Location = new System.Drawing.Point(20, 205);
            this.btnDebugDisplay.Name = "btnDebugDisplay";
            this.btnDebugDisplay.Size = new System.Drawing.Size(75, 23);
            this.btnDebugDisplay.TabIndex = 6;
            this.btnDebugDisplay.Text = "Display";
            this.btnDebugDisplay.UseVisualStyleBackColor = true;
            this.btnDebugDisplay.Click += new System.EventHandler(this.btnDebugDisplay_Click);
            // 
            // btnDebugStep
            // 
            this.btnDebugStep.Location = new System.Drawing.Point(20, 48);
            this.btnDebugStep.Name = "btnDebugStep";
            this.btnDebugStep.Size = new System.Drawing.Size(75, 23);
            this.btnDebugStep.TabIndex = 5;
            this.btnDebugStep.Text = "Step";
            this.btnDebugStep.UseVisualStyleBackColor = true;
            this.btnDebugStep.Click += new System.EventHandler(this.btnDebugStep_Click);
            // 
            // btnDebugReset
            // 
            this.btnDebugReset.Location = new System.Drawing.Point(20, 19);
            this.btnDebugReset.Name = "btnDebugReset";
            this.btnDebugReset.Size = new System.Drawing.Size(75, 23);
            this.btnDebugReset.TabIndex = 4;
            this.btnDebugReset.Text = "Reset";
            this.btnDebugReset.UseVisualStyleBackColor = true;
            this.btnDebugReset.Click += new System.EventHandler(this.btnDebugReset_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(27, 28);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // SoC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 571);
            this.Controls.Add(this.grpActions);
            this.Controls.Add(this.tctMain);
            this.Name = "SoC";
            this.Text = "SoC";
            this.Load += new System.EventHandler(this.frmCPU_Load);
            this.Resize += new System.EventHandler(this.frmCPU_Resize);
            this.tctMain.ResumeLayout(false);
            this.tbpSource.ResumeLayout(false);
            this.tbpSource.PerformLayout();
            this.tbpEmulator.ResumeLayout(false);
            this.tbpEmulator.PerformLayout();
            this.grpActions.ResumeLayout(false);
            this.grpDebug.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnAssemble;
        private System.Windows.Forms.ListView lstBinary;
        private System.Windows.Forms.ColumnHeader colHAddress;
        private System.Windows.Forms.ColumnHeader colHOpcode;
        private System.Windows.Forms.ColumnHeader colHLine;
        private System.Windows.Forms.ColumnHeader colHError;
        private System.Windows.Forms.TabControl tctMain;
        private System.Windows.Forms.TabPage tbpSource;
        private System.Windows.Forms.TabPage tbpEmulator;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.GroupBox grpDebug;
        private System.Windows.Forms.Button btnDebugReset;
        private System.Windows.Forms.Button btnDebugStep;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ListView lstRegister;
        private System.Windows.Forms.ColumnHeader colHRegName;
        private System.Windows.Forms.ColumnHeader colHRegValue;
        private System.Windows.Forms.Button btnDebugDisplay;
        private System.Windows.Forms.ListView lstMemory;
        private System.Windows.Forms.ColumnHeader colHMemoryName;
        private System.Windows.Forms.ColumnHeader colHMemoryValue;
        private System.Windows.Forms.Button btnDebugBreak;
        private System.Windows.Forms.Button btnDebugRun;
        private System.Windows.Forms.Label lblProgramCounter;
        private System.Windows.Forms.Button btnSave;
    }
}

