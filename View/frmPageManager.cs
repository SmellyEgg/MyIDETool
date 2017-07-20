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
        /// <summary>
        /// 当前选择的树节点
        /// </summary>
        private TreeNode _currentSelectedNode = null;
        /// <summary>
        /// 平台
        /// </summary>
        private string platForm = "app";
        /// <summary>
        /// 页面配置业务层
        /// </summary>
        pageConfigCMD _pageconfigCmd;
        public string PlatForm
        {
            get
            {
                return platForm;
            }

            set
            {
                platForm = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public frmPageManager()
        {
            InitializeComponent();
            _pageconfigCmd = new pageConfigCMD();
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="platform"></param>
        public async void SetPages(string platform)
        {
            if (string.IsNullOrEmpty(platform))
            {
                return;
            }
            this.tvwPageGroups.Nodes.Clear();
            this.prgPageLoad.Visible = true;
            pageGroupReturnData request = await _pageconfigCmd.GetPageGroupInfo(platform);
            this.prgPageLoad.Value = 30;
            if (!Object.Equals(request, null))
            {
                foreach (pageGroupDetail pd in request.data)
                {
                    TreeNode parentNode = new TreeNode(pd.group_name);
                    parentNode.Tag = pd.group_id;
                    this.tvwPageGroups.Nodes.Add(parentNode);
                    foreach (pageDetailForGroup pddsecond in pd.page_list)
                    {
                        TreeNode childrenNode = new TreeNode(pddsecond.page_name);
                        childrenNode.Tag = pddsecond.page_id;
                        parentNode.Nodes.Add(childrenNode);
                    }
                }
            }
            else
            {
                MessageBox.Show(_pageconfigCmd.errCode);
                this.prgPageLoad.Visible = false;
                return;
            }
            this.prgPageLoad.Value = 60;
            //将文件中的缓存添加到页面中来
            _pageconfigCmd.SetCacheToTreeView(this.tvwPageGroups);
            this.prgPageLoad.Value = 60;
            this.prgPageLoad.Visible = false;
        }

        /// <summary>
        /// 新建页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewPage_Click(object sender, EventArgs e)
        {
            if (object.Equals(_currentSelectedNode, null) || !object.Equals(_currentSelectedNode.Parent, null))
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

        /// <summary>
        /// 获取选择的焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwPageGroups_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _currentSelectedNode = e.Node;
        }

        /// <summary>
        /// 新建分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewGroup_Click(object sender, EventArgs e)
        {
            Boolean isParent = true;
            AddNode(isParent);
        }

        /// <summary>
        /// 新增节点
        /// </summary>
        /// <param name="isParent"></param>
        private void AddNode(bool isParent)
        {
            string nodeText = this.GetCreateName();
            if (string.IsNullOrEmpty(nodeText)) return;
            if (isNodeExists(_currentSelectedNode, nodeText, isParent))
            {
                MessageBox.Show("该节点已经存在！");
                return;
            }
            TreeNode Node = new TreeNode(nodeText);
            if (isParent)
            {
                this.tvwPageGroups.Nodes.Add(Node);
                _pageconfigCmd.AddGroup(nodeText, platForm);
            }
            else
            {
                _currentSelectedNode.Nodes.Add(Node);
                string groupId = object.Equals(_currentSelectedNode.Tag, null) ? string.Empty : _currentSelectedNode.Tag.ToString();
                string groupName = _currentSelectedNode.Text;
                _pageconfigCmd.AddPage(nodeText, groupId, PlatForm, groupName);
            }
        }

        /// <summary>
        /// 树节点名称是否已存在
        /// </summary>
        /// <param name="Node"></param>
        /// <param name="newNodeText"></param>
        /// <returns></returns>
        private Boolean isNodeExists(TreeNode Node, string newNodeText, Boolean isParent)
        {
            if (!isParent)
            {
                foreach (TreeNode obj in Node.Nodes)
                {
                    if (newNodeText.Equals(obj.Text))
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach(TreeNode node in this.tvwPageGroups.Nodes)
                {
                    if (newNodeText.Equals(node.Text))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 保存缓存
        /// </summary>
        public async void SaveCache()
        {
            int result = await _pageconfigCmd.SaveCache();
            if (result == 1)
            {
                MessageBox.Show("保存成功！");
            }
        }

        /// <summary>
        /// 上传
        /// </summary>
        public async void Upload()
        {
            int result = await _pageconfigCmd.Upload();
            if (result == 1)
            {
                MessageBox.Show("上传成功！");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.SaveCache();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Upload();
        }
    }
}
