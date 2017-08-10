namespace xinLongIDE.View.MainForm
{
    partial class frmTabForPaintBoard
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
            this.tabPaintBoardContainer = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPaintBoardContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPaintBoardContainer
            // 
            this.tabPaintBoardContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPaintBoardContainer.Controls.Add(this.tabPage1);
            this.tabPaintBoardContainer.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabPaintBoardContainer.Location = new System.Drawing.Point(0, 0);
            this.tabPaintBoardContainer.Multiline = true;
            this.tabPaintBoardContainer.Name = "tabPaintBoardContainer";
            this.tabPaintBoardContainer.Padding = new System.Drawing.Point(21, 3);
            this.tabPaintBoardContainer.SelectedIndex = 0;
            this.tabPaintBoardContainer.Size = new System.Drawing.Size(1092, 673);
            this.tabPaintBoardContainer.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabPaintBoardContainer.TabIndex = 0;
            this.tabPaintBoardContainer.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabPaintBoardContainer_DrawItem);
            this.tabPaintBoardContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabPaintBoardContainer_MouseDown);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1084, 647);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // frmTabForPaintBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 670);
            this.Controls.Add(this.tabPaintBoardContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmTabForPaintBoard";
            this.Text = "frmTabForPaintBoard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTabForPaintBoard_FormClosing);
            this.tabPaintBoardContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabPaintBoardContainer;
        private System.Windows.Forms.TabPage tabPage1;
        //private System.Windows.Forms.TabPage tabpage1;
        //private System.Windows.Forms.TabPage tabPage2;
    }
}