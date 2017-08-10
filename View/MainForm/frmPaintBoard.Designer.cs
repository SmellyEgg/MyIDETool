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
            this.SuspendLayout();
            // 
            // pnlPaintBoard
            // 
            this.pnlPaintBoard.AllowDrop = true;
            this.pnlPaintBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPaintBoard.BackColor = System.Drawing.SystemColors.Info;
            this.pnlPaintBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPaintBoard.Location = new System.Drawing.Point(0, 0);
            this.pnlPaintBoard.Name = "pnlPaintBoard";
            this.pnlPaintBoard.Size = new System.Drawing.Size(379, 509);
            this.pnlPaintBoard.TabIndex = 0;
            this.pnlPaintBoard.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlPaintBoard_DragDrop);
            this.pnlPaintBoard.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlPaintBoard_DragEnter);
            // 
            // pnlInvisible
            // 
            this.pnlInvisible.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInvisible.AutoScroll = true;
            this.pnlInvisible.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.pnlInvisible.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInvisible.Location = new System.Drawing.Point(0, 515);
            this.pnlInvisible.Name = "pnlInvisible";
            this.pnlInvisible.Size = new System.Drawing.Size(379, 125);
            this.pnlInvisible.TabIndex = 1;
            // 
            // frmPaintBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 639);
            this.ControlBox = false;
            this.Controls.Add(this.pnlInvisible);
            this.Controls.Add(this.pnlPaintBoard);
            this.Name = "frmPaintBoard";
            this.Text = "画板";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPaintBoard;
        private System.Windows.Forms.Panel pnlInvisible;
    }
}