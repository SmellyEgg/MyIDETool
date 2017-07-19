using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.View
{
    public partial class frmPageManager : Form
    {
        private TreeNode _currentSelectedNode = null;

        pageConfigCMD _pageconfigCmd;
        public frmPageManager()
        {
            InitializeComponent();
            _pageconfigCmd = new pageConfigCMD();
        }

        public int ShowPages(pageGroupReturnData obj)
        {
           if (obj.data.Length < 1 || object.Equals(obj, null))
            {
                return 0;
            }
            this.tvwPageGroups.Nodes.Clear();
           foreach(pageGroupDetail pd in obj.data)
            {
                TreeNode parentNode = new TreeNode(pd.group_name);
                parentNode.Tag = pd.group_id;
                this.tvwPageGroups.Nodes.Add(parentNode);
                foreach(pageDetailForGroup pddsecond in pd.page_list)
                {
                    TreeNode childrenNode = new TreeNode(pddsecond.page_name);
                    childrenNode.Tag = pddsecond.page_id;
                    parentNode.Nodes.Add(childrenNode);
                }
            } 
            return 1;
        }

        private void btnNewPage_Click(object sender, EventArgs e)
        {
            if (!object.Equals(_currentSelectedNode.Parent, null) || object.Equals(_currentSelectedNode, null))
            {
                MessageBox.Show("请先选择一个分组！");
                return;
            }

            Boolean isParent = false;
            AddNode(isParent);

           

        }

        /// <summary>
        /// 获取输入的页面或组名称
        /// </summary>
        /// <returns></returns>
        private string GetCreateName()
        {
            Control.frmGetInputName frm = new Control.frmGetInputName();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                return frm.createName;
            }
            return string.Empty;
        }

        private void tvwPageGroups_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _currentSelectedNode = e.Node;
        }

        private void btnNewGroup_Click(object sender, EventArgs e)
        {
            Boolean isParent = true;
            AddNode(isParent);
        }

        private void AddNode(bool isParent)
        {
            string nodeText = this.GetCreateName();
            if (string.IsNullOrEmpty(nodeText)) return;

            TreeNode Node = new TreeNode(nodeText);
            if (isParent)
            {
                this.tvwPageGroups.Nodes.Add(Node);
            }
            else
            {
                _currentSelectedNode.Nodes.Add(Node);
                _pageconfigCmd.AddPage(nodeText, _currentSelectedNode.Tag.ToString(), "app");
            }
        }

    }
}
