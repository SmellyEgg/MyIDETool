using System;
using System.Collections.Generic;
using System.Linq;
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
            _currentOriginalInfo = new pageGroupReturnData();
            _currentOriginalInfo = await _pageconfigCmd.GetOriginalPageInfo(platForm);
            if (!object.Equals(_currentOriginalInfo, null))
            {
                SetValueToTreeView(this.tvwOriginal, _currentOriginalInfo);
            }
            _currentGroupInfo = new pageGroupReturnData();
            _currentGroupInfo = _pageconfigCmd.GetPageGroupInfoTemp(platform);
            if (!object.Equals(_currentGroupInfo, null))
            {
                SetValueToTreeView(this.tvwPageGroups, _currentGroupInfo);
            }
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
        private string GetCreateName(bool isChild)
        {
            Control.frmGetInputName frm = new Control.frmGetInputName();
            frm.SetUserGroup(isChild);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (isChild)
                {
                    return frm.createName + "," + frm.userGroup;
                }
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
            string resultStr = this.GetCreateName(!isParent);
            string nodeText = resultStr.Split(',')[0];
            if (string.IsNullOrEmpty(nodeText)) return;
            if (isNodeExists(_currentSelectedNode, nodeText, isParent))
            {
                MessageBox.Show("该节点已经存在！");
                return;
            }
            TreeNode Node = new TreeNode(nodeText);

            
            if (isParent)
            {
                if (object.Equals(_currentGroupInfo, null))
                {
                    _currentGroupInfo = new pageGroupReturnData();
                }
                int groupid = _pageconfigCmd.AddGroup(nodeText, platForm, _currentGroupInfo);
                Node.Tag = groupid;
                this.tvwPageGroups.Nodes.Add(Node);
            }
            else
            {
                if (object.Equals(_currentGroupInfo, null))
                {
                    _currentGroupInfo = new pageGroupReturnData();
                }
                if (object.Equals(_currentOriginalInfo, null))
                {
                    _currentOriginalInfo = new pageGroupReturnData();
                }
                int pageid = _pageconfigCmd.GetNewPageId(_currentGroupInfo, _currentOriginalInfo, this.platForm);
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
                obj.Plat_form = this.platForm;
                obj.UserGroup = resultStr.Split(',')[1];
                pageCreated.Invoke(obj);

            }
            this.SaveCache();
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
            //int result = await _pageconfigCmd.SaveCache(_currentOriginalInfo, _currentGroupInfo, this.platForm);
            //这里不使用异步，由于异步会导致组信息取到的还是上次的缓存
            _pageconfigCmd.SaveCache(_currentOriginalInfo, _currentGroupInfo, this.platForm);
        }

        public void Upload()
        {
            _pageconfigCmd.UploadGroupInfo(_currentGroupInfo, this.platForm);
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

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsrRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshOriginalTree();
        }

        /// <summary>
        /// 树节点双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                obj.Plat_form = this.platForm;
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
                MessageBox.Show("暂时只支持下载页面！");
                return;
            }
            int pageId = (int)clickedNode.Tag;
            int groupId = !object.Equals(clickedNode.Parent, null) ? (int)(clickedNode.Parent.Tag) : (int)clickedNode.Tag;
            //_currentOriginalInfo.data
            if (object.Equals(_currentGroupInfo, null))
            {
                _currentGroupInfo = new pageGroupReturnData();
            }
            int result = 0;
            if (!object.Equals(clickedNode.Parent, null))
            {
                result = _pageconfigCmd.DownLoadPage(pageId, groupId, _currentOriginalInfo, _currentGroupInfo);
            }
            else
            {
                result = _pageconfigCmd.DownLoadGroup(groupId, _currentOriginalInfo, _currentGroupInfo);
            }
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
                else
                {
                    List<pageGroupDetail> listGroup = new List<pageGroupDetail>();
                    listGroup.AddRange(_currentOriginalInfo.data);
                    pageGroupDetail group = listGroup.First(p => groupId == p.group_id);
                    foreach(pageDetailForGroup page in group.page_list)
                    {
                        nodeObjectTransfer obj = new nodeObjectTransfer();
                        obj.PageId = page.page_id;
                        obj.GroupId = groupId;
                        obj.PageName = page.page_name;
                        pageDownloaded.Invoke(obj);
                    }
                }
            }
        }

        /// <summary>
        /// 更改页面属性
        /// </summary>
        /// <param name="oldObj"></param>
        /// <param name="newObj"></param>
        public void ChangePageProperty(object oldObj, object newObj)
        {
            _pageconfigCmd.ChangePageProperty(oldObj, newObj, _currentGroupInfo);
            this.SaveCache();
            this.SetPages(this.platForm);
        }

        /// <summary>
        /// 清理树目录
        /// </summary>
        public void Clear()
        {
            this.tvwOriginal.Nodes.Clear();
            this.tvwPageGroups.Nodes.Clear();
        }

        private void tsbtnDeleteNode_Click(object sender, EventArgs e)
        {
            if (object.Equals(_currentSelectedNode, null))
            {
                MessageBox.Show("请先选择一个节点！");
                return;
            }

            //删除父节点的情况
            if (object.Equals(_currentSelectedNode.Parent, null))
            {
                //_currentGroupInfo.data
            }
            //删除子节点的情况
            else
            {

            }

        }
    }
}
