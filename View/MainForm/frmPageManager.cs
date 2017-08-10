using System;
using System.Windows.Forms;
using xinLongIDE.Controller.CommonController;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Model.Page;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.View.MainForm
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
        /// 上传之后委托主窗体更新page信息
        /// </summary>
        /// <param name="page"></param>
        public delegate void delegateForPageDownloaded(object page);
        public event delegateForPageDownloaded pageDownloaded;

        /// <summary>
        /// 页面创建委托
        /// </summary>
        /// <param name="page"></param>
        public delegate void delegateForPageCreated(object page);
        public event delegateForPageDownloaded pageCreated;

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
            _currentGroupInfo = new pageGroupReturnData();
            _currentOriginalInfo = new pageGroupReturnData();
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
                Logging.Error("PlatForm is empty!");
                return;
            }
            _currentOriginalInfo = await _pageconfigCmd.GetOriginalPageInfo(platForm);
            if (!object.Equals(_currentOriginalInfo, null))
            {
                SetValueToTreeView(this.tvwOriginal, _currentOriginalInfo);
            }
            else
            {
                MessageBox.Show("无法连接服务，正在使用离线版本！");
            }
            _currentGroupInfo = _pageconfigCmd.GetPageGroupInfoTemp();
            if (!object.Equals(_currentGroupInfo, null))
            {
                SetValueToTreeView(this.tvwPageGroups, _currentGroupInfo);
            }
            //改成要么全部在线读取要么全部从本地读取
            //_currentGroupInfo = await _pageconfigCmd.GetPageGroupInfo(platForm);
            //if (_pageconfigCmd.isOffLine)
            //{
            //    MessageBox.Show("当前使用的为离线版本!");
            //    if (Object.Equals(_currentGroupInfo, null))
            //    {
            //        _currentGroupInfo = new pageGroupReturnData();
            //        return;
            //    }
            //}
            //if (!_pageconfigCmd.isReadLocal)
            //{
            //    _currentOriginalInfo = _currentGroupInfo;
            //}
            //else
            //{
            //    _currentOriginalInfo = await _pageconfigCmd.GetOriginalPageInfo(platForm);
            //}
            //this.prgPageLoad.Value = 30;
            //if (!Object.Equals(_currentGroupInfo, null))
            //{
            //    SetValueToTreeView(this.tvwOriginal, _currentOriginalInfo);
            //    //SetValueToTreeView(this.tvwPageGroups, _currentGroupInfo);
            //    //将文件中的缓存添加到页面中来
            //    //_pageconfigCmd.SetCacheToTreeView(this.tvwPageGroups);
            //}
            //else
            //{
            //    MessageBox.Show(_pageconfigCmd.errCode);
            //    return;
            //}
        }

        /// <summary>
        /// 设置树控件
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="obj"></param>
        private void SetValueToTreeView(TreeView tv, pageGroupReturnData obj)
        {
            tv.Nodes.Clear();
            foreach (pageGroupDetail pd in obj.data)
            {
                TreeNode parentNode = new TreeNode(pd.group_name);
                parentNode.Tag = pd.group_id;
                tv.Nodes.Add(parentNode);
                if (!object.Equals(pd.page_list, null))
                {
                    foreach (pageDetailForGroup pddsecond in pd.page_list)
                    {
                        TreeNode childrenNode = new TreeNode(pddsecond.page_name);
                        childrenNode.Tag = pddsecond.page_id;
                        parentNode.Nodes.Add(childrenNode);
                    }
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
                int groupid = _pageconfigCmd.AddGroup(nodeText, platForm, _currentGroupInfo);
                Node.Tag = groupid;
                this.tvwPageGroups.Nodes.Add(Node);
            }
            else
            {
                int pageid = _pageconfigCmd.GetNewPageId(_currentGroupInfo, _currentOriginalInfo);
                int groupId = (int)_currentSelectedNode.Tag;
                string groupName = _currentSelectedNode.Text;
                _pageconfigCmd.AddPage(pageid, nodeText, groupId, groupName, PlatForm, _currentGroupInfo);
                Node.Tag = pageid;
                _currentSelectedNode.Nodes.Add(Node);
                //需要触发，让那边新建一个页面
                nodeObjectTransfer obj = new nodeObjectTransfer();
                obj.PageId = pageid;
                obj.GroupId = groupId;
                obj.PageName = nodeText;
                pageCreated.Invoke(obj);

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
                foreach (TreeNode node in this.tvwPageGroups.Nodes)
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
        }

        public void Upload()
        {
            _pageconfigCmd.UploadGroupInfo(_currentGroupInfo);
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
                obj.GroupId = (int)_currentSelectedNode.Parent.Tag;
                obj.PageName = _currentSelectedNode.Text;
                nodeSelected.Invoke(obj);
            }
        }

        private void frmPageManager_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 原始树队列下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwOriginal_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //将界面下载到本地
            TreeNode clickedNode = e.Node;
            if (object.Equals(clickedNode, null))
            {
                return;
            }
            int pageId = (int)clickedNode.Tag;
            int groupId = !object.Equals(clickedNode.Parent, null) ? (int)(clickedNode.Parent.Tag) : -1;
            //_currentOriginalInfo.data
            if (object.Equals(_currentGroupInfo, null))
            {
                _currentGroupInfo = new pageGroupReturnData();
            }
            int result = _pageconfigCmd.DownLoadPage(pageId, groupId, _currentOriginalInfo, _currentGroupInfo);
            if (result == 1)
            {
                this.SetValueToTreeView(this.tvwPageGroups, _currentGroupInfo);
                this.SaveCache();
                //缓存一下该ID的页面信息,这一步应该让主窗体通知画板去做这一步
                if (!object.Equals(clickedNode.Parent, null))
                {
                    nodeObjectTransfer obj = new nodeObjectTransfer();
                    obj.PageId = (int)clickedNode.Tag;
                    obj.GroupId = (int)clickedNode.Parent.Tag;
                    obj.PageName = clickedNode.Text;
                    pageDownloaded.Invoke(obj);
                }
            }
        }


    }
}
