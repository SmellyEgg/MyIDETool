using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xinLongIDE.View.Control
{
    public partial class frmGetInputName : Form
    {

        public string createName = string.Empty;

        public frmGetInputName()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Valid())
            {
                createName = this.txtName.Text.Trim();
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
            return true;
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnOk.PerformClick();
            }
        }
    }
}
