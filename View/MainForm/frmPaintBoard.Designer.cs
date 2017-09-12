  namespace xinLongIDE.View.MainForm
{
    partial class frmPaintBoard
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
            this.pnlPaintBoard = new System.Windows.Forms.Panel();
            this.pnlInvisible = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnUpload = new System.Windows.Forms.ToolStripButton();
            this.pnlNavigationBar = new System.Windows.Forms.Panel();
            this.lstViewVisibleControl = new System.Windows.Forms.ListView();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPaintBoard
            // 
            this.pnlPaintBoard.AllowDrop = true;
            this.pnlPaintBoard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlPaintBoard.AutoScroll = true;
            this.pnlPaintBoard.BackColor = System.Drawing.SystemColors.Info;
            this.pnlPaintBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPaintBoard.Location = new System.Drawing.Point(12, 77);
            this.pnlPaintBoard.Name = "pnlPaintBoard";
            this.pnlPaintBoard.Size = new System.Drawing.Size(375, 667);
            this.pnlPaintBoard.TabIndex = 0;
            this.pnlPaintBoard.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlPaintBoard_DragDrop);
            this.pnlPaintBoard.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlPaintBoard_DragEnter);
            // 
            // pnlInvisible
            // 
            this.pnlInvisible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInvisible.AutoScroll = true;
            this.pnlInvisible.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.pnlInvisible.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInvisible.Location = new System.Drawing.Point(12, 766);
            this.pnlInvisible.Name = "pnlInvisible";
            this.pnlInvisible.Size = new System.Drawing.Size(628, 152);
            this.pnlInvisible.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSave,
            this.toolStripSeparator1,
            this.tsbtnUpload});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(651, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnSave
            // 
            this.tsbtnSave.Image = global::xinLongIDE.Properties.Resources.toolStripSave;
            this.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSave.Name = "tsbtnSave";
            this.tsbtnSave.Size = new System.Drawing.Size(52, 22);
            this.tsbtnSave.Text = "保存";
            this.tsbtnSave.Click += new System.EventHandler(this.tsbtnSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnUpload
            // 
            this.tsbtnUpload.Image = global::xinLongIDE.Properties.Resources.toolStripUpload;
            this.tsbtnUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnUpload.Name = "tsbtnUpload";
            this.tsbtnUpload.Size = new System.Drawing.Size(52, 22);
            this.tsbtnUpload.Text = "上传";
            this.tsbtnUpload.Click += new System.EventHandler(this.tsbtnUpload_Click);
            // 
            // pnlNavigationBar
            // 
            this.pnlNavigationBar.AllowDrop = true;
            this.pnlNavigationBar.BackColor = System.Drawing.SystemColors.Info;
            this.pnlNavigationBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNavigationBar.Location = new System.Drawing.Point(12, 32);
            this.pnlNavigationBar.Name = "pnlNavigationBar";
            this.pnlNavigationBar.Size = new System.Drawing.Size(375, 44);
            this.pnlNavigationBar.TabIndex = 3;
            this.pnlNavigationBar.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlNavigationBar_DragDrop);
            this.pnlNavigationBar.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlNavigationBar_DragEnter);
            // 
            // lstViewVisibleControl
            // 
            this.lstViewVisibleControl.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstViewVisibleControl.Location = new System.Drawing.Point(408, 77);
            this.lstViewVisibleControl.Name = "lstViewVisibleControl";
            this.lstViewVisibleControl.Size = new System.Drawing.Size(231, 667);
            this.lstViewVisibleControl.TabIndex = 4;
            this.lstViewVisibleControl.UseCompatibleStateImageBehavior = false;
            this.lstViewVisibleControl.View = System.Windows.Forms.View.List;
            this.lstViewVisibleControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstViewVisibleControl_MouseDoubleClick);
            // 
            // frmPaintBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(651, 919);
            this.Controls.Add(this.lstViewVisibleControl);
            this.Controls.Add(this.pnlNavigationBar);
            this.Controls.Add(this.pnlPaintBoard);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pnlInvisible);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPaintBoard";
            this.Text = "画板";
            this.TopMost = true;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlPaintBoard;
        private System.Windows.Forms.Panel pnlInvisible;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbtnUpload;
        private System.Windows.Forms.Panel pnlNavigationBar;
        private System.Windows.Forms.ListView lstViewVisibleControl;
    }
}