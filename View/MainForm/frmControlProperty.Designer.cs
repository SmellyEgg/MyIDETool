namespace xinLongIDE.View.MainForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbgProperty = new System.Windows.Forms.TabPage();
            this.tbgEvent = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tbgProperty);
            this.tabControl1.Controls.Add(this.tbgEvent);
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(287, 718);
            this.tabControl1.TabIndex = 0;
            // 
            // tbgProperty
            // 
            this.tbgProperty.AutoScroll = true;
            this.tbgProperty.Location = new System.Drawing.Point(4, 22);
            this.tbgProperty.Name = "tbgProperty";
            this.tbgProperty.Padding = new System.Windows.Forms.Padding(3);
            this.tbgProperty.Size = new System.Drawing.Size(279, 692);
            this.tbgProperty.TabIndex = 0;
            this.tbgProperty.Text = "属性";
            this.tbgProperty.UseVisualStyleBackColor = true;
            // 
            // tbgEvent
            // 
            this.tbgEvent.AutoScroll = true;
            this.tbgEvent.Location = new System.Drawing.Point(4, 22);
            this.tbgEvent.Name = "tbgEvent";
            this.tbgEvent.Padding = new System.Windows.Forms.Padding(3);
            this.tbgEvent.Size = new System.Drawing.Size(279, 695);
            this.tbgEvent.TabIndex = 1;
            this.tbgEvent.Text = "事件";
            this.tbgEvent.UseVisualStyleBackColor = true;
            // 
            // frmControlProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 722);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Name = "frmControlProperty";
            this.Text = "属性";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbgProperty;
        private System.Windows.Forms.TabPage tbgEvent;
    }
}