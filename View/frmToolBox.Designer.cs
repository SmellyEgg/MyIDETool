namespace xinLongIDE.View
{
    partial class frmToolBox
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
            this.lvwtoolboxitems = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lvwtoolboxitems
            // 
            this.lvwtoolboxitems.AllowDrop = true;
            this.lvwtoolboxitems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwtoolboxitems.Location = new System.Drawing.Point(0, -1);
            this.lvwtoolboxitems.Name = "lvwtoolboxitems";
            this.lvwtoolboxitems.Size = new System.Drawing.Size(269, 497);
            this.lvwtoolboxitems.TabIndex = 0;
            this.lvwtoolboxitems.UseCompatibleStateImageBehavior = false;
            this.lvwtoolboxitems.DragOver += new System.Windows.Forms.DragEventHandler(this.lvwtoolboxitems_DragOver);
            this.lvwtoolboxitems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvwtoolboxitems_MouseDown);
            // 
            // frmToolBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 497);
            this.ControlBox = false;
            this.Controls.Add(this.lvwtoolboxitems);
            this.Name = "frmToolBox";
            this.Text = "工具箱";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvwtoolboxitems;
    }
}