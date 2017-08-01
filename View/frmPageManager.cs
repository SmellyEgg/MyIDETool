using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Controller;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Model.Page;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.View
{
    public partial class frmPageManager : Form
    {
        /// <summary>
        /// 委托与事件的定义
        /// </summary>
        /// <param name="id"></param>
        public delegate void delegateForSelectedNode(object page);
        public event delegateForSelectedNode nodeSelected;

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
        /// 当前组管理信息
        /// </summary>
        private pageGroupReturnData _currentGroupInfo;
        /// <summary>
        /// 当前的原始组信息
        /// </summary>
        private pageGroupReturnData _currentOriginalInfo;
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
            this.platForm = platform;
            if (string.IsNullOrEmpty(platForm))
            {
                Controller.Logging.Error("PlatForm is empty!");
                return;
            }
            //改成要么全部在线读取要么全部从本地读取
            _currentGroupInfo = await _pageconfigCmd.GetPageGroupInfo(platForm);
            if (!_pageconfigCmd.isReadLocal)
            {
                _currentOriginalInfo = _currentGroupInfo;
            }
            else
            {
                _currentOriginalInfo = await _pageconfigCmd.GetOriginalPageInfo(platForm);
            }
            this.prgPageLoad.Value = 30;
            if (!Object.Equals(_currentGroupInfo, null))
            {
                SetValueToTreeView(this.tvwOriginal, _currentOriginalInfo);
                SetValueToTreeView(this.tvwPageGroups, _currentGroupInfo);
                //将文件中的缓存添加到页面中来
                //_pageconfigCmd.SetCacheToTreeView(this.tvwPageGroups);
            }
            else
            {
                MessageBox.Show(_pageconfigCmd.errCode);
                return;
            }
        }

        private void SetValueToTreeView(TreeView tv, pageGroupReturnData obj)
        {
            tv.Nodes.Clear();
            foreach (pageGroupDetail pd in obj.data)
            {
                TreeNode parentNode = new TreeNode(pd.group_name);
                parentNode.Tag = pd.group_id;
                tv.Nodes.Add(parentNode);
                foreach (pageDetailForGroup pddsecond in pd.page_list)
                {
                    TreeNode childrenNode = new TreeNode(pddsecond.page_name);
                    childrenNode.Tag = pddsecond.page_id;
                    parentNode.Nodes.Add(childrenNode);
                }
            }
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
                string groupid = _pageconfigCmd.AddGroup(nodeText, platForm, _currentGroupInfo);
                Node.Tag = groupid;
                this.tvwPageGroups.Nodes.Add(Node);
            }
            else
            {
                int pageid = _pageconfigCmd.GetNewPageId(_currentGroupInfo);
                string groupId = _currentSelectedNode.Tag.ToString();
                _pageconfigCmd.AddPage(pageid, nodeText, groupId, PlatForm, _currentGroupInfo);
                Node.Tag = pageid;
                _currentSelectedNode.Nodes.Add(Node);
                //string groupId = object.Equals(_currentSelectedNode.Tag, null) ? string.Empty : _currentSelectedNode.Tag.ToString();
                //string groupId = _currentSelectedNode.Tag.ToString();
                //string groupName = _currentSelectedNode.Text;
                //_pageconfigCmd.AddPage(nodeText, groupId, PlatForm, groupName);
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
            int result = await _pageconfigCmd.SaveCache(_currentOriginalInfo, _currentGroupInfo);
            //if (result == 1)
            //{
            //    MessageBox.Show("保存成功！");
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.SaveCache();
        }

        private async void RefreshOriginalTree()
        {
            _pageconfigCmd.Refresh();
            pageGroupReturnData result = await _pageconfigCmd.GetOriginalPageInfo(platForm);
            SetValueToTreeView(this.tvwOriginal, result);
        }

        private void tsrbtnNewPage_Click(object sender, EventArgs e)
        {
            if (object.Equals(_currentSelectedNode, null) || !object.Equals(_currentSelectedNode.Parent, null))
            {
                MessageBox.Show("请先选择一个分组！");
                return;
            }

            Boolean isParent = false;
            AddNode(isParent);
        }

        private void tsrbtnNewGroup_Click(object sender, EventArgs e)
        {
             Boolean isParent = true;
            AddNode(isParent);
        }

        private void tsrRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshOriginalTree();
        }

        private void tvwPageGroups_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _currentSelectedNode = e.Node;
            //将结果委托到主窗体
            if (!object.Equals(_currentSelectedNode.Parent, null))
            {
                nodeObjectTransfer obj = new nodeObjectTransfer();
                obj.PageId = (int)_currentSelectedNode.Tag;
                obj.GroupId = _currentSelectedNode.Parent.Tag.ToString();
                obj.PageName = _currentSelectedNode.Text;
                //pageDetailForGroup obj = new pageDetailForGroup();
                //obj.page_id = (int)_currentSelectedNode.Tag;
                //obj.page_name = _currentSelectedNode.Text;
                nodeSelected.Invoke(obj);
            }
        }

        private void frmPageManager_Load(object sender, EventArgs e)
        {
        }
    }
}
