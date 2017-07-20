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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNewGroup = new System.Windows.Forms.Button();
            this.btnNewPage = new System.Windows.Forms.Button();
            this.tvwPageGroups = new System.Windows.Forms.TreeView();
            this.prgPageLoad = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.prgPageLoad);
            this.panel1.Controls.Add(this.btnNewGroup);
            this.panel1.Controls.Add(this.btnNewPage);
            this.panel1.Controls.Add(this.tvwPageGroups);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(316, 689);
            this.panel1.TabIndex = 0;
            // 
            // btnNewGroup
            // 
            this.btnNewGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewGroup.Location = new System.Drawing.Point(171, 26);
            this.btnNewGroup.Name = "btnNewGroup";
            this.btnNewGroup.Size = new System.Drawing.Size(83, 52);
            this.btnNewGroup.TabIndex = 2;
            this.btnNewGroup.Text = "新建组";
            this.btnNewGroup.UseVisualStyleBackColor = true;
            this.btnNewGroup.Click += new System.EventHandler(this.btnNewGroup_Click);
            // 
            // btnNewPage
            // 
            this.btnNewPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewPage.Location = new System.Drawing.Point(28, 26);
            this.btnNewPage.Name = "btnNewPage";
            this.btnNewPage.Size = new System.Drawing.Size(87, 52);
            this.btnNewPage.TabIndex = 1;
            this.btnNewPage.Text = "新建页面";
            this.btnNewPage.UseVisualStyleBackColor = true;
            this.btnNewPage.Click += new System.EventHandler(this.btnNewPage_Click);
            // 
            // tvwPageGroups
            // 
            this.tvwPageGroups.Location = new System.Drawing.Point(3, 94);
            this.tvwPageGroups.Name = "tvwPageGroups";
            this.tvwPageGroups.Size = new System.Drawing.Size(313, 592);
            this.tvwPageGroups.TabIndex = 0;
            this.tvwPageGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwPageGroups_AfterSelect);
            // 
            // prgPageLoad
            // 
            this.prgPageLoad.Location = new System.Drawing.Point(15, 653);
            this.prgPageLoad.Name = "prgPageLoad";
            this.prgPageLoad.Size = new System.Drawing.Size(100, 23);
            this.prgPageLoad.TabIndex = 3;
            this.prgPageLoad.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(28, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = " 测试保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(171, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "测试上传";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmPageManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 688);
            this.Controls.Add(this.panel1);
            this.Name = "frmPageManager";
            this.Text = "页面管理";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView tvwPageGroups;
        private System.Windows.Forms.Button btnNewGroup;
        private System.Windows.Forms.Button btnNewPage;
        private System.Windows.Forms.ProgressBar prgPageLoad;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}