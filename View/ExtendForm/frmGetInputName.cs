using System;
using System.Windows.Forms;

namespace xinLongIDE.View.Control
{
    public partial class frmGetInputName : Form
    {

        public string createName = string.Empty;
        public string userGroup = string.Empty;

        public frmGetInputName()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Valid())
            {
                createName = this.txtName.Text.Trim();
                if (this.txtUserGroup.Visible)
                {
                    userGroup = this.txtUserGroup.Text.Trim();
                }
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private bool Valid()
        {
            if (string.IsNullOrEmpty(this.txtName.Text.Trim()))
            {
                MessageBox.Show("名称不能为空");
                return false;
            }
            else if (this.txtUserGroup.Visible)
            {
                if (string.IsNullOrEmpty(this.txtUserGroup.Text.Trim()))
                {
                    MessageBox.Show("用户组不能为空");
                    return false;
                }
            }
            return true;
        }

        public void SetUserGroup(bool isVisible)
        {
            this.label2.Visible = isVisible;
            this.txtUserGroup.Visible = isVisible;
        }


        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnOk.PerformClick();
            }
        }

        private void txtUserGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnOk.PerformClick();
            }
        }
    }
}
