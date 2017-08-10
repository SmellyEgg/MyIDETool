using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using xinLongIDE.Controller;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Controller.dataDic;
using xinLongIDE.Model.Page;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.View.MainForm
{
    public partial class frmTabForPaintBoard : Form
    {
        //委托与事件，用于实现进度条的回调
        public delegate void delegateForProgressBar(int value);
        public event delegateForProgressBar progressChange;

        //用于实现属性值改变
        public delegate void delegateForControlPropertyChanged(object obj);
        public event delegateForControlPropertyChanged controlPropertyChange;


        private List<frmPaintBoard> _listOfPaintBoards;

        private List<nodeObjectTransfer> _listOfOpenedPages;

        private paintBoardController _paintboardController;
        public frmTabForPaintBoard()
        {
            InitializeComponent();

            _listOfPaintBoards = new List<frmPaintBoard>();
            _listOfOpenedPages = new List<nodeObjectTransfer>();
            _paintboardController = new paintBoardController();
            //打开上次关闭时打开了的页面
            ResumePreviousPage();
        }

        /// <summary>
        /// 恢复上次打开的界面
        /// </summary>
        private void ResumePreviousPage()
        {
            string filePath = ConfigureFilePath.openedPages;
            if (File.Exists(filePath))
            {
                List<nodeObjectTransfer> list = new List<nodeObjectTransfer>();
                list = xmlController.ReadFromXmlFile<List<nodeObjectTransfer>>(filePath);
                if (!object.Equals(list, null) && list.Count > 0)
                {
                    foreach(nodeObjectTransfer obj in list)
                    {
                        this.AddPaintBoard(obj);
                    }
                }
            }
        }

        /// <summary>
        /// 保存缓存
        /// </summary>
        public void SaveCache()
        {
            if (!object.Equals(_listOfPaintBoards, null) && _listOfPaintBoards.Count > 0)
            {
                //增加进度条控制
                int value = 100 / _listOfPaintBoards.Count;
                foreach (frmPaintBoard frm in _listOfPaintBoards)
                {
                    frm.SaveCache();
                    progressChange.Invoke(value);
                    value += 100 / _listOfPaintBoards.Count;
                }
            }
        }

        /// <summary>
        /// 上传
        /// </summary>
        public void Upload()
        {
            if (!object.Equals(_listOfPaintBoards, null) && _listOfPaintBoards.Count > 0)
            {
                //增加进度条控制
                int value = 100 / _listOfPaintBoards.Count;
                foreach (frmPaintBoard frm in _listOfPaintBoards)
                {
                    frm.Upload();
                    progressChange.Invoke(value);
                    value += 100 / _listOfPaintBoards.Count;
                }
            }
        }

        public void SetControlProperty(object obj)
        {
            if (!object.Equals(obj, null) && this.tabPaintBoardContainer.SelectedIndex != -1)
            {
                _listOfPaintBoards[this.tabPaintBoardContainer.SelectedIndex].SetControlProperty(obj);
            }
        }

        public void AddPaintBoard(object obj)
        {
            nodeObjectTransfer pageObj = obj as nodeObjectTransfer;
            //防止打开重复的界面
            int pageindex = _listOfOpenedPages.FindIndex(p => pageObj.PageId.Equals(p.PageId));
            if (pageindex != -1)
            {
                this.tabPaintBoardContainer.SelectedIndex = pageindex;
                return;
            }
            frmPaintBoard frm = new frmPaintBoard();
            frm = new frmPaintBoard();
            frm.ShowPageDetail(pageObj);
            frm.progressChange += ChangeProgressValue;
            frm.controlPropertyChange += Frm_controlPropertyChange;
            frm.TopLevel = false;
            frm.Visible = true;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            string tabName = pageObj.PageId.ToString() + "--" + pageObj.PageName;
            if (_listOfPaintBoards.Count < 1 && this.tabPaintBoardContainer.TabPages.Count > 0)
            {
                TabPage tpg = this.tabPaintBoardContainer.TabPages[0];
                tpg.Text = tabName;
                tpg.Controls.Add(frm);
            }
            else
            {
                TabPage tpg = new TabPage(tabName);
                tpg.Controls.Add(frm);
                this.tabPaintBoardContainer.TabPages.Add(tpg);
            }
            _listOfPaintBoards.Add(frm);
            _listOfOpenedPages.Add(pageObj);
            this.tabPaintBoardContainer.SelectedIndex = _listOfPaintBoards.Count - 1;
        }

        private void Frm_controlPropertyChange(object obj)
        {
            controlPropertyChange.Invoke(obj);
        }

        private void ChangeProgressValue(int value)
        {
            progressChange.Invoke(value);
        }

        /// <summary>
        /// 缓存页面信息
        /// </summary>
        /// <param name="obj"></param>
        public async void SavePageDetail(object obj)
        {
            nodeObjectTransfer pageObj = obj as nodeObjectTransfer;
            int pageid = pageObj.PageId;
            await _paintboardController.SavePageDetail(pageid);
        }

        /// <summary>
        /// 创建页面
        /// </summary>
        /// <param name="obj"></param>
        public async void CreatePage(object obj)
        {
            nodeObjectTransfer pageObj = obj as nodeObjectTransfer;
            int pageid = pageObj.PageId;
            pageDetailReturnData pageInfo = new pageDetailReturnData();
            pageInfo = _paintboardController.AddPreControlsForNewPage(pageObj.PageId, pageObj.PageName, pageObj.GroupId);
            await _paintboardController.CreatePage(pageInfo);
        }

        private void tabPaintBoardContainer_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 4);
            e.Graphics.DrawString(this.tabPaintBoardContainer.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
            e.DrawFocusRectangle();
        }

        private void tabPaintBoardContainer_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < this.tabPaintBoardContainer.TabPages.Count; i++)
            {
                Rectangle r = tabPaintBoardContainer.GetTabRect(i);
                //Getting the position of the "x" mark.
                Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 4, 9, 7);
                if (closeButton.Contains(e.Location))
                {
                    if (MessageBox.Show("确定要关闭该选项卡?", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.tabPaintBoardContainer.TabPages.RemoveAt(i);
                        if (_listOfPaintBoards.Count > 0)
                        {
                            _listOfPaintBoards.RemoveAt(i);
                        }
                        if (_listOfOpenedPages.Count > 0)
                        {
                            _listOfOpenedPages.RemoveAt(i);
                        }
                        break;
                    }
                }
            }
        }

        private void frmTabForPaintBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            //保存已经打开了的界面
            if (File.Exists(ConfigureFilePath.openedPages))
            {
                File.Delete(ConfigureFilePath.openedPages);
            }
            xmlController.WriteToXmlFile<List<nodeObjectTransfer>>(ConfigureFilePath.openedPages, _listOfOpenedPages);
        }
    }
}
