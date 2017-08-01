namespace xinLongIDE.View
{
    partial class frmPageManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPageManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsrbtnNewPage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsrbtnNewGroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsrRefresh = new System.Windows.Forms.ToolStripButton();
            this.tvwOriginal = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.prgPageLoad = new System.Windows.Forms.ProgressBar();
            this.tvwPageGroups = new System.Windows.Forms.TreeView();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.tvwOriginal);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.prgPageLoad);
            this.panel1.Controls.Add(this.tvwPageGroups);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(528, 689);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsrbtnNewPage,
            this.toolStripSeparator1,
            this.tsrbtnNewGroup,
            this.toolStripSeparator2,
            this.tsrRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(528, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsrbtnNewPage
            // 
            this.tsrbtnNewPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsrbtnNewPage.Image = ((System.Drawing.Image)(resources.GetObject("tsrbtnNewPage.Image")));
            this.tsrbtnNewPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnNewPage.Name = "tsrbtnNewPage";
            this.tsrbtnNewPage.Size = new System.Drawing.Size(48, 22);
            this.tsrbtnNewPage.Text = "新页面";
            this.tsrbtnNewPage.Click += new System.EventHandler(this.tsrbtnNewPage_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsrbtnNewGroup
            // 
            this.tsrbtnNewGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsrbtnNewGroup.Image = ((System.Drawing.Image)(resources.GetObject("tsrbtnNewGroup.Image")));
            this.tsrbtnNewGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnNewGroup.Name = "tsrbtnNewGroup";
            this.tsrbtnNewGroup.Size = new System.Drawing.Size(36, 22);
            this.tsrbtnNewGroup.Text = "新组";
            this.tsrbtnNewGroup.Click += new System.EventHandler(this.tsrbtnNewGroup_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsrRefresh
            // 
            this.tsrRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsrRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsrRefresh.Image")));
            this.tsrRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrRefresh.Name = "tsrRefresh";
            this.tsrRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsrRefresh.Text = "toolStripButton3";
            this.tsrRefresh.Click += new System.EventHandler(this.tsrRefresh_Click);
            // 
            // tvwOriginal
            // 
            this.tvwOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvwOriginal.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.tvwOriginal.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tvwOriginal.ItemHeight = 30;
            this.tvwOriginal.LineColor = System.Drawing.Color.LightSkyBlue;
            this.tvwOriginal.Location = new System.Drawing.Point(272, 48);
            this.tvwOriginal.Name = "tvwOriginal";
            this.tvwOriginal.Size = new System.Drawing.Size(251, 638);
            this.tvwOriginal.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(213, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = " 测试保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // prgPageLoad
            // 
            this.prgPageLoad.Location = new System.Drawing.Point(15, 653);
            this.prgPageLoad.Name = "prgPageLoad";
            this.prgPageLoad.Size = new System.Drawing.Size(100, 23);
            this.prgPageLoad.TabIndex = 3;
            this.prgPageLoad.Visible = false;
            // 
            // tvwPageGroups
            // 
            this.tvwPageGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvwPageGroups.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tvwPageGroups.ItemHeight = 30;
            this.tvwPageGroups.LineColor = System.Drawing.Color.LightSkyBlue;
            this.tvwPageGroups.Location = new System.Drawing.Point(3, 48);
            this.tvwPageGroups.Name = "tvwPageGroups";
            this.tvwPageGroups.Size = new System.Drawing.Size(251, 638);
            this.tvwPageGroups.TabIndex = 0;
            this.tvwPageGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwPageGroups_AfterSelect);
            this.tvwPageGroups.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwPageGroups_NodeMouseDoubleClick);
            // 
            // frmPageManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 688);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Name = "frmPageManager";
            this.Text = "页面管理";
            this.Load += new System.EventHandler(this.frmPageManager_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView tvwPageGroups;
        private System.Windows.Forms.ProgressBar prgPageLoad;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TreeView tvwOriginal;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsrbtnNewPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsrbtnNewGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsrRefresh;
    }
}