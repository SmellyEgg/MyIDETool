namespace xinLongIDE.View.MainForm
{
    partial class frmMainIDE
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainIDE));
            this.pnlStatusBar = new System.Windows.Forms.Panel();
            this.prgStatus = new System.Windows.Forms.ProgressBar();
            this.tsrMainForm = new System.Windows.Forms.ToolStrip();
            this.tsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnUpload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tscmbType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.pnlStatusBar.SuspendLayout();
            this.tsrMainForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlStatusBar
            // 
            this.pnlStatusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlStatusBar.Controls.Add(this.prgStatus);
            this.pnlStatusBar.Location = new System.Drawing.Point(2, 749);
            this.pnlStatusBar.Name = "pnlStatusBar";
            this.pnlStatusBar.Size = new System.Drawing.Size(1477, 45);
            this.pnlStatusBar.TabIndex = 1;
            // 
            // prgStatus
            // 
            this.prgStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.prgStatus.Location = new System.Drawing.Point(1253, 13);
            this.prgStatus.Name = "prgStatus";
            this.prgStatus.Size = new System.Drawing.Size(214, 23);
            this.prgStatus.TabIndex = 0;
            // 
            // tsrMainForm
            // 
            this.tsrMainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSave,
            this.toolStripSeparator1,
            this.tsbtnUpload,
            this.toolStripSeparator2,
            this.toolStripButton1,
            this.toolStripSeparator4,
            this.tscmbType,
            this.toolStripSeparator3});
            this.tsrMainForm.Location = new System.Drawing.Point(0, 0);
            this.tsrMainForm.Name = "tsrMainForm";
            this.tsrMainForm.Size = new System.Drawing.Size(1481, 30);
            this.tsrMainForm.TabIndex = 2;
            this.tsrMainForm.Text = "toolStrip1";
            // 
            // tsbtnSave
            // 
            this.tsbtnSave.Image = global::xinLongIDE.Properties.Resources.toolStripSave;
            this.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSave.Name = "tsbtnSave";
            this.tsbtnSave.Size = new System.Drawing.Size(76, 27);
            this.tsbtnSave.Text = "全部保存";
            this.tsbtnSave.Click += new System.EventHandler(this.tsbtnSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            // 
            // tsbtnUpload
            // 
            this.tsbtnUpload.Image = global::xinLongIDE.Properties.Resources.toolStripUpload;
            this.tsbtnUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnUpload.Name = "tsbtnUpload";
            this.tsbtnUpload.Size = new System.Drawing.Size(76, 27);
            this.tsbtnUpload.Text = "全部上传";
            this.tsbtnUpload.Click += new System.EventHandler(this.tsbtnUpload_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 30);
            // 
            // tscmbType
            // 
            this.tscmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscmbType.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tscmbType.Items.AddRange(new object[] {
            "app",
            "pc"});
            this.tscmbType.Name = "tscmbType";
            this.tscmbType.Size = new System.Drawing.Size(75, 30);
            this.tscmbType.SelectedIndexChanged += new System.EventHandler(this.tscmbType_SelectedIndexChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(60, 27);
            this.toolStripButton1.Text = "清理缓存";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 30);
            // 
            // frmMainIDE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1481, 797);
            this.Controls.Add(this.tsrMainForm);
            this.Controls.Add(this.pnlStatusBar);
            this.Icon = global::xinLongIDE.Properties.Resources.ideico;
            this.IsMdiContainer = true;
            this.Name = "frmMainIDE";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "xinLongyu Studio";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainIDE_FormClosing);
            this.pnlStatusBar.ResumeLayout(false);
            this.tsrMainForm.ResumeLayout(false);
            this.tsrMainForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlStatusBar;
        private System.Windows.Forms.ProgressBar prgStatus;
        private System.Windows.Forms.ToolStrip tsrMainForm;
        private System.Windows.Forms.ToolStripButton tsbtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbtnUpload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox tscmbType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}