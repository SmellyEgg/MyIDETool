namespace xinLongIDE.View
{
    partial class frmControlProperty
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
            this.pnlProperty = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlProperty
            // 
            this.pnlProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProperty.AutoScroll = true;
            this.pnlProperty.Location = new System.Drawing.Point(2, 0);
            this.pnlProperty.Name = "pnlProperty";
            this.pnlProperty.Size = new System.Drawing.Size(285, 722);
            this.pnlProperty.TabIndex = 22;
            // 
            // frmControlProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 722);
            this.ControlBox = false;
            this.Controls.Add(this.pnlProperty);
            this.Name = "frmControlProperty";
            this.Text = "属性";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmControlProperty_FormClosing);
            this.Load += new System.EventHandler(this.frmControlProperty_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlProperty;
    }
}