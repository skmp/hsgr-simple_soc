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
            this.tbpDevice = new System.Windows.Forms.TabPage();
            this.chkMemoryDisplay = new System.Windows.Forms.CheckBox();
            this.chkRegisterDisplay = new System.Windows.Forms.CheckBox();
            this.chkBinaryDisplay = new System.Windows.Forms.CheckBox();
            this.lblProgramCounter = new System.Windows.Forms.Label();
            this.lstMemory = new System.Windows.Forms.ListView();
            this.colHMemoryName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHMemoryValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstRegister = new System.Windows.Forms.ListView();
            this.colHRegName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHRegValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpUtils = new System.Windows.Forms.GroupBox();
            this.btnSpriteCalculator = new System.Windows.Forms.Button();
            this.grpDebug = new System.Windows.Forms.GroupBox();
            this.btnDeviceUpload = new System.Windows.Forms.Button();
            this.btnDeviceDisconnect = new System.Windows.Forms.Button();
            this.btnDeviceConnect = new System.Windows.Forms.Button();
            this.btnDeviceConfigure = new System.Windows.Forms.Button();
            this.btnDeviceReadAll = new System.Windows.Forms.Button();
            this.cmbDevices = new System.Windows.Forms.ComboBox();
            this.btnDeviceHalt = new System.Windows.Forms.Button();
            this.btnDeviceResume = new System.Windows.Forms.Button();
            this.btnDeviceDisplay = new System.Windows.Forms.Button();
            this.btnDeviceStep = new System.Windows.Forms.Button();
            this.btnDeviceReset = new System.Windows.Forms.Button();
            this.tctMain.SuspendLayout();
            this.tbpSource.SuspendLayout();
            this.tbpDevice.SuspendLayout();
            this.grpActions.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpUtils.SuspendLayout();
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
            this.btnAssemble.Location = new System.Drawing.Point(19, 48);
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
            this.tctMain.Controls.Add(this.tbpDevice);
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
            // tbpDevice
            // 
            this.tbpDevice.Controls.Add(this.chkMemoryDisplay);
            this.tbpDevice.Controls.Add(this.chkRegisterDisplay);
            this.tbpDevice.Controls.Add(this.chkBinaryDisplay);
            this.tbpDevice.Controls.Add(this.lblProgramCounter);
            this.tbpDevice.Controls.Add(this.lstMemory);
            this.tbpDevice.Controls.Add(this.lstRegister);
            this.tbpDevice.Controls.Add(this.lstBinary);
            this.tbpDevice.Location = new System.Drawing.Point(4, 22);
            this.tbpDevice.Name = "tbpDevice";
            this.tbpDevice.Padding = new System.Windows.Forms.Padding(3);
            this.tbpDevice.Size = new System.Drawing.Size(735, 496);
            this.tbpDevice.TabIndex = 1;
            this.tbpDevice.Text = "Emulator";
            this.tbpDevice.UseVisualStyleBackColor = true;
            // 
            // chkMemoryDisplay
            // 
            this.chkMemoryDisplay.AutoSize = true;
            this.chkMemoryDisplay.Checked = true;
            this.chkMemoryDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemoryDisplay.Location = new System.Drawing.Point(660, 9);
            this.chkMemoryDisplay.Name = "chkMemoryDisplay";
            this.chkMemoryDisplay.Size = new System.Drawing.Size(60, 17);
            this.chkMemoryDisplay.TabIndex = 9;
            this.chkMemoryDisplay.Text = "Display";
            this.chkMemoryDisplay.UseVisualStyleBackColor = true;
            // 
            // chkRegisterDisplay
            // 
            this.chkRegisterDisplay.AutoSize = true;
            this.chkRegisterDisplay.Checked = true;
            this.chkRegisterDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRegisterDisplay.Location = new System.Drawing.Point(513, 9);
            this.chkRegisterDisplay.Name = "chkRegisterDisplay";
            this.chkRegisterDisplay.Size = new System.Drawing.Size(60, 17);
            this.chkRegisterDisplay.TabIndex = 8;
            this.chkRegisterDisplay.Text = "Display";
            this.chkRegisterDisplay.UseVisualStyleBackColor = true;
            // 
            // chkBinaryDisplay
            // 
            this.chkBinaryDisplay.AutoSize = true;
            this.chkBinaryDisplay.Checked = true;
            this.chkBinaryDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBinaryDisplay.Location = new System.Drawing.Point(360, 10);
            this.chkBinaryDisplay.Name = "chkBinaryDisplay";
            this.chkBinaryDisplay.Size = new System.Drawing.Size(60, 17);
            this.chkBinaryDisplay.TabIndex = 7;
            this.chkBinaryDisplay.Text = "Display";
            this.chkBinaryDisplay.UseVisualStyleBackColor = true;
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
            this.grpActions.Controls.Add(this.groupBox1);
            this.grpActions.Controls.Add(this.grpUtils);
            this.grpActions.Controls.Add(this.grpDebug);
            this.grpActions.Location = new System.Drawing.Point(13, 13);
            this.grpActions.Name = "grpActions";
            this.grpActions.Size = new System.Drawing.Size(125, 666);
            this.grpActions.TabIndex = 5;
            this.grpActions.TabStop = false;
            this.grpActions.Text = "Actions";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLoad);
            this.groupBox1.Controls.Add(this.btnAssemble);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Location = new System.Drawing.Point(7, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(112, 138);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Assembler";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(19, 19);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(19, 106);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(19, 77);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpUtils
            // 
            this.grpUtils.Controls.Add(this.btnSpriteCalculator);
            this.grpUtils.Location = new System.Drawing.Point(7, 602);
            this.grpUtils.Name = "grpUtils";
            this.grpUtils.Size = new System.Drawing.Size(112, 58);
            this.grpUtils.TabIndex = 7;
            this.grpUtils.TabStop = false;
            this.grpUtils.Text = "Utils";
            // 
            // btnSpriteCalculator
            // 
            this.btnSpriteCalculator.Location = new System.Drawing.Point(19, 19);
            this.btnSpriteCalculator.Name = "btnSpriteCalculator";
            this.btnSpriteCalculator.Size = new System.Drawing.Size(75, 23);
            this.btnSpriteCalculator.TabIndex = 6;
            this.btnSpriteCalculator.Text = "Sprite Calc";
            this.btnSpriteCalculator.UseVisualStyleBackColor = true;
            this.btnSpriteCalculator.Click += new System.EventHandler(this.btnSpriteCalculator_Click);
            // 
            // grpDebug
            // 
            this.grpDebug.Controls.Add(this.btnDeviceUpload);
            this.grpDebug.Controls.Add(this.btnDeviceDisconnect);
            this.grpDebug.Controls.Add(this.btnDeviceConnect);
            this.grpDebug.Controls.Add(this.btnDeviceConfigure);
            this.grpDebug.Controls.Add(this.btnDeviceReadAll);
            this.grpDebug.Controls.Add(this.cmbDevices);
            this.grpDebug.Controls.Add(this.btnDeviceHalt);
            this.grpDebug.Controls.Add(this.btnDeviceResume);
            this.grpDebug.Controls.Add(this.btnDeviceDisplay);
            this.grpDebug.Controls.Add(this.btnDeviceStep);
            this.grpDebug.Controls.Add(this.btnDeviceReset);
            this.grpDebug.Location = new System.Drawing.Point(7, 191);
            this.grpDebug.Name = "grpDebug";
            this.grpDebug.Size = new System.Drawing.Size(112, 405);
            this.grpDebug.TabIndex = 3;
            this.grpDebug.TabStop = false;
            this.grpDebug.Text = "Device";
            // 
            // btnDeviceUpload
            // 
            this.btnDeviceUpload.Enabled = false;
            this.btnDeviceUpload.Location = new System.Drawing.Point(19, 292);
            this.btnDeviceUpload.Name = "btnDeviceUpload";
            this.btnDeviceUpload.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceUpload.TabIndex = 14;
            this.btnDeviceUpload.Text = "Upload";
            this.btnDeviceUpload.UseVisualStyleBackColor = true;
            this.btnDeviceUpload.Click += new System.EventHandler(this.btnDeviceUpload_Click);
            // 
            // btnDeviceDisconnect
            // 
            this.btnDeviceDisconnect.Enabled = false;
            this.btnDeviceDisconnect.Location = new System.Drawing.Point(19, 105);
            this.btnDeviceDisconnect.Name = "btnDeviceDisconnect";
            this.btnDeviceDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceDisconnect.TabIndex = 13;
            this.btnDeviceDisconnect.Text = "Disconnect";
            this.btnDeviceDisconnect.UseVisualStyleBackColor = true;
            this.btnDeviceDisconnect.Click += new System.EventHandler(this.btnDeviceDisconnect_Click);
            // 
            // btnDeviceConnect
            // 
            this.btnDeviceConnect.Location = new System.Drawing.Point(19, 76);
            this.btnDeviceConnect.Name = "btnDeviceConnect";
            this.btnDeviceConnect.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceConnect.TabIndex = 12;
            this.btnDeviceConnect.Text = "Connect";
            this.btnDeviceConnect.UseVisualStyleBackColor = true;
            this.btnDeviceConnect.Click += new System.EventHandler(this.btnDeviceConnect_Click);
            // 
            // btnDeviceConfigure
            // 
            this.btnDeviceConfigure.Location = new System.Drawing.Point(19, 47);
            this.btnDeviceConfigure.Name = "btnDeviceConfigure";
            this.btnDeviceConfigure.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceConfigure.TabIndex = 11;
            this.btnDeviceConfigure.Text = "Configure";
            this.btnDeviceConfigure.UseVisualStyleBackColor = true;
            this.btnDeviceConfigure.Click += new System.EventHandler(this.btnDeviceConfigure_Click);
            // 
            // btnDeviceReadAll
            // 
            this.btnDeviceReadAll.Enabled = false;
            this.btnDeviceReadAll.Location = new System.Drawing.Point(19, 321);
            this.btnDeviceReadAll.Name = "btnDeviceReadAll";
            this.btnDeviceReadAll.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceReadAll.TabIndex = 10;
            this.btnDeviceReadAll.Text = "Read All";
            this.btnDeviceReadAll.UseVisualStyleBackColor = true;
            this.btnDeviceReadAll.Click += new System.EventHandler(this.btnDeviceReadAll_Click);
            // 
            // cmbDevices
            // 
            this.cmbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevices.FormattingEnabled = true;
            this.cmbDevices.Location = new System.Drawing.Point(19, 20);
            this.cmbDevices.Name = "cmbDevices";
            this.cmbDevices.Size = new System.Drawing.Size(75, 21);
            this.cmbDevices.TabIndex = 9;
            this.cmbDevices.SelectedIndexChanged += new System.EventHandler(this.cmbDevices_SelectedIndexChanged);
            // 
            // btnDeviceHalt
            // 
            this.btnDeviceHalt.Enabled = false;
            this.btnDeviceHalt.Location = new System.Drawing.Point(19, 185);
            this.btnDeviceHalt.Name = "btnDeviceHalt";
            this.btnDeviceHalt.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceHalt.TabIndex = 8;
            this.btnDeviceHalt.Text = "Halt";
            this.btnDeviceHalt.UseVisualStyleBackColor = true;
            this.btnDeviceHalt.Click += new System.EventHandler(this.btnDeviceHalt_Click);
            // 
            // btnDeviceResume
            // 
            this.btnDeviceResume.Enabled = false;
            this.btnDeviceResume.Location = new System.Drawing.Point(19, 214);
            this.btnDeviceResume.Name = "btnDeviceResume";
            this.btnDeviceResume.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceResume.TabIndex = 7;
            this.btnDeviceResume.Text = "Resume";
            this.btnDeviceResume.UseVisualStyleBackColor = true;
            this.btnDeviceResume.Click += new System.EventHandler(this.btnDeviceResume_Click);
            // 
            // btnDeviceDisplay
            // 
            this.btnDeviceDisplay.Enabled = false;
            this.btnDeviceDisplay.Location = new System.Drawing.Point(19, 376);
            this.btnDeviceDisplay.Name = "btnDeviceDisplay";
            this.btnDeviceDisplay.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceDisplay.TabIndex = 6;
            this.btnDeviceDisplay.Text = "Display";
            this.btnDeviceDisplay.UseVisualStyleBackColor = true;
            this.btnDeviceDisplay.Click += new System.EventHandler(this.btnDeviceDisplay_Click);
            // 
            // btnDeviceStep
            // 
            this.btnDeviceStep.Enabled = false;
            this.btnDeviceStep.Location = new System.Drawing.Point(19, 243);
            this.btnDeviceStep.Name = "btnDeviceStep";
            this.btnDeviceStep.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceStep.TabIndex = 5;
            this.btnDeviceStep.Text = "Step";
            this.btnDeviceStep.UseVisualStyleBackColor = true;
            this.btnDeviceStep.Click += new System.EventHandler(this.btnDeviceStep_Click);
            // 
            // btnDeviceReset
            // 
            this.btnDeviceReset.Enabled = false;
            this.btnDeviceReset.Location = new System.Drawing.Point(19, 156);
            this.btnDeviceReset.Name = "btnDeviceReset";
            this.btnDeviceReset.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceReset.TabIndex = 4;
            this.btnDeviceReset.Text = "Reset";
            this.btnDeviceReset.UseVisualStyleBackColor = true;
            this.btnDeviceReset.Click += new System.EventHandler(this.btnDeviceReset_Click);
            // 
            // SoC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 691);
            this.Controls.Add(this.grpActions);
            this.Controls.Add(this.tctMain);
            this.Name = "SoC";
            this.Text = "SoC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SoC_FormClosing);
            this.Load += new System.EventHandler(this.SoC_Load);
            this.Resize += new System.EventHandler(this.SoC_Resize);
            this.tctMain.ResumeLayout(false);
            this.tbpSource.ResumeLayout(false);
            this.tbpSource.PerformLayout();
            this.tbpDevice.ResumeLayout(false);
            this.tbpDevice.PerformLayout();
            this.grpActions.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.grpUtils.ResumeLayout(false);
            this.grpDebug.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnAssemble;
        private System.Windows.Forms.ColumnHeader colHAddress;
        private System.Windows.Forms.ColumnHeader colHOpcode;
        private System.Windows.Forms.ColumnHeader colHLine;
        private System.Windows.Forms.ColumnHeader colHError;
        private System.Windows.Forms.TabControl tctMain;
        private System.Windows.Forms.TabPage tbpSource;
        private System.Windows.Forms.TabPage tbpDevice;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.GroupBox grpDebug;
        private System.Windows.Forms.Button btnDeviceReset;
        private System.Windows.Forms.Button btnDeviceStep;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ColumnHeader colHRegName;
        private System.Windows.Forms.ColumnHeader colHRegValue;
        private System.Windows.Forms.Button btnDeviceDisplay;
        private System.Windows.Forms.ColumnHeader colHMemoryName;
        private System.Windows.Forms.ColumnHeader colHMemoryValue;
        private System.Windows.Forms.Button btnDeviceHalt;
        private System.Windows.Forms.Button btnDeviceResume;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox grpUtils;
        private System.Windows.Forms.Button btnSpriteCalculator;
        private System.Windows.Forms.ComboBox cmbDevices;
        private System.Windows.Forms.Button btnDeviceReadAll;
        private System.Windows.Forms.Button btnDeviceConfigure;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDeviceDisconnect;
        private System.Windows.Forms.Button btnDeviceConnect;
        private System.Windows.Forms.Button btnDeviceUpload;
        public System.Windows.Forms.CheckBox chkBinaryDisplay;
        public System.Windows.Forms.Label lblProgramCounter;
        public System.Windows.Forms.ListView lstRegister;
        public System.Windows.Forms.ListView lstMemory;
        public System.Windows.Forms.CheckBox chkMemoryDisplay;
        public System.Windows.Forms.CheckBox chkRegisterDisplay;
        public System.Windows.Forms.ListView lstBinary;
    }
}

