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
            this.tabProperty = new System.Windows.Forms.TabControl();
            this.tabPageProperty = new System.Windows.Forms.TabPage();
            this.tabPageEvent = new System.Windows.Forms.TabPage();
            this.tabProperty.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabProperty
            // 
            this.tabProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabProperty.Controls.Add(this.tabPageProperty);
            this.tabProperty.Controls.Add(this.tabPageEvent);
            this.tabProperty.Location = new System.Drawing.Point(1, 0);
            this.tabProperty.Name = "tabProperty";
            this.tabProperty.SelectedIndex = 0;
            this.tabProperty.Size = new System.Drawing.Size(287, 725);
            this.tabProperty.TabIndex = 0;
            // 
            // tabPageProperty
            // 
            this.tabPageProperty.Location = new System.Drawing.Point(4, 22);
            this.tabPageProperty.Name = "tabPageProperty";
            this.tabPageProperty.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProperty.Size = new System.Drawing.Size(279, 699);
            this.tabPageProperty.TabIndex = 0;
            this.tabPageProperty.Text = "属性";
            this.tabPageProperty.UseVisualStyleBackColor = true;
            // 
            // tabPageEvent
            // 
            this.tabPageEvent.Location = new System.Drawing.Point(4, 22);
            this.tabPageEvent.Name = "tabPageEvent";
            this.tabPageEvent.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvent.Size = new System.Drawing.Size(279, 699);
            this.tabPageEvent.TabIndex = 1;
            this.tabPageEvent.Text = "事件";
            this.tabPageEvent.UseVisualStyleBackColor = true;
            // 
            // frmControlProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 722);
            this.ControlBox = false;
            this.Controls.Add(this.tabProperty);
            this.Name = "frmControlProperty";
            this.Text = "属性";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmControlProperty_FormClosing);
            this.Load += new System.EventHandler(this.frmControlProperty_Load);
            this.tabProperty.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabProperty;
        private System.Windows.Forms.TabPage tabPageProperty;
        private System.Windows.Forms.TabPage tabPageEvent;
    }
}