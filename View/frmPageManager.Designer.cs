﻿namespace xinLongIDE.View
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
            this.tvwPageGroups = new System.Windows.Forms.TreeView();
            this.btnNewPage = new System.Windows.Forms.Button();
            this.btnNewGroup = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnNewGroup);
            this.panel1.Controls.Add(this.btnNewPage);
            this.panel1.Controls.Add(this.tvwPageGroups);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(316, 689);
            this.panel1.TabIndex = 0;
            // 
            // tvwPageGroups
            // 
            this.tvwPageGroups.Location = new System.Drawing.Point(3, 94);
            this.tvwPageGroups.Name = "tvwPageGroups";
            this.tvwPageGroups.Size = new System.Drawing.Size(313, 595);
            this.tvwPageGroups.TabIndex = 0;
            this.tvwPageGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwPageGroups_AfterSelect);
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
    }
}