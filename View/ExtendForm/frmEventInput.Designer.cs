namespace xinLongIDE.View.ExtendForm
{
    partial class frmEventInput
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
            this.rtxEvent = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lstEventList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // rtxEvent
            // 
            this.rtxEvent.Location = new System.Drawing.Point(21, 51);
            this.rtxEvent.Name = "rtxEvent";
            this.rtxEvent.Size = new System.Drawing.Size(470, 275);
            this.rtxEvent.TabIndex = 0;
            this.rtxEvent.Text = "";
            this.rtxEvent.TextChanged += new System.EventHandler(this.rtxEvent_TextChanged);
            this.rtxEvent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtxEvent_KeyDown);
            this.rtxEvent.KeyUp += new System.Windows.Forms.KeyEventHandler(this.rtxEvent_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "在集合中输入事件（以回车键作为分隔）";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(306, 352);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(416, 352);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(268, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "(快捷组合键：ctrl + s)";
            // 
            // lstEventList
            // 
            this.lstEventList.FormattingEnabled = true;
            this.lstEventList.ItemHeight = 12;
            this.lstEventList.Location = new System.Drawing.Point(103, 61);
            this.lstEventList.Name = "lstEventList";
            this.lstEventList.Size = new System.Drawing.Size(173, 256);
            this.lstEventList.TabIndex = 5;
            this.lstEventList.Visible = false;
            this.lstEventList.DoubleClick += new System.EventHandler(this.lstEventList_DoubleClick);
            this.lstEventList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstEventList_KeyDown);
            // 
            // frmEventInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 420);
            this.Controls.Add(this.lstEventList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtxEvent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmEventInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "事件触发编辑器";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEventInput_FormClosed);
            this.LocationChanged += new System.EventHandler(this.frmEventInput_LocationChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxEvent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstEventList;
    }
}