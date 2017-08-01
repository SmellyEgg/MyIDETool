﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using xinLongIDE.Controller;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Controller.dataDic;
using xinLongIDE.Controller.Settings;
using xinLongIDE.Model.Control;
using xinLongIDE.Model.Page;
using xinLongIDE.Model.returnJson;
using xinLongIDE.View.Control;

namespace xinLongIDE.View
{
    public partial class frmPaintBoard : Form
    {
        //委托与事件，用于实现进度条的回调
        public delegate void delegateForProgressBar(int value);
        public event delegateForProgressBar progressChange;

        //用于实现属性值改变
        public delegate void delegateForControlPropertyChanged(object obj);
        public event delegateForControlPropertyChanged controlPropertyChange;

        /// <summary>
        /// 逻辑层
        /// </summary>
        private paintBoardController _paintboardController;
        /// <summary>
        /// 右键菜单
        /// </summary>
        ContextMenu _cmMunu;
        /// <summary>
        /// 当前页面信息
        /// </summary>
        private pageDetailReturnData _currentPageInfo;
        /// <summary>
        /// 当前页面ID
        /// </summary>
        private int _currentPageId = -1;
        /// <summary>
        /// 当前页面ID
        /// </summary>
        public int CurrentPageId
        {
            get
            {
                return this._currentPageId;
            }
            set
            {
                this._currentPageId = value;
            }
        }

        private string _currentPageName = string.Empty;
        public frmPaintBoard()
        {
            InitializeComponent();
            _paintboardController = new paintBoardController();
            _cmMunu = this.GetContextMenu();
            //ControlMoverOrResizer.WorkType = ControlMoverOrResizer.MoveOrResize.MoveAndResize;
        }

        /// <summary>
        /// 解析页面信息
        /// </summary>
        /// <param name="obj"></param>
        public async void ShowPageDetail(object obj)
        {
            try
            {
                nodeObjectTransfer pageObj = obj as nodeObjectTransfer;
                //pageDetailForGroup pageObj = obj as pageDetailForGroup;
                _currentPageInfo = await _paintboardController.GetPageDetailInfo(pageObj.PageId);
                if (_paintboardController.errorCode == -2)
                {
                    MessageBox.Show("获取页面信息出错，请检查网络！");
                    progressChange.Invoke(100);
                    return;
                }
                if (object.Equals(_currentPageInfo.data, null))
                {
                    //throw new Exception("该页面内容为空");
                    //这里进行新建页面，清理之后添加预设元素就可以了
                    this.Clear();
                    _currentPageInfo = _paintboardController.AddPreControlsForNewPage(pageObj.PageId, pageObj.PageName, pageObj.GroupId);
                }
                progressChange.Invoke(50);
                //对页面进行解析
                this.Clear();
                _currentPageId = pageObj.PageId;
                _currentPageName = pageObj.PageName;
                this.SetBaseInfo(_currentPageInfo);
                progressChange.Invoke(75);
                this.SetPageDetailInfo(_currentPageInfo);
                progressChange.Invoke(100);
            }
            catch (Exception ex)
            {
                string error = "解析页面出错：" + ex.Message;
                Logging.Error(error);
                MessageBox.Show(error);
                progressChange.Invoke(100);
                return;
            }
        }

        /// <summary>
        /// 执行清理
        /// </summary>
        private void Clear()
        {
            this.pnlInvisible.Controls.Clear();
            this.pnlPaintBoard.Controls.Clear();
            this._lstControl.Clear();
            this._lstInvisibleControl.Clear();
            _navigationBarHeight = 0;
        }
        /// <summary>
        /// 设置窗体的基本信息
        /// </summary>
        /// <param name="obj"></param>
        private void SetBaseInfo(pageDetailReturnData obj)
        {
            dataForPageDetail dtObj = obj.data;
            if (!string.IsNullOrEmpty(dtObj.page_title))
            {
                this.Text = dtObj.page_title;
            }
            this.BackColor = System.Drawing.ColorTranslator.FromHtml(dtObj.page_background_color);
            this.Width = 335;
            this.Height = 750;
            dtObj = null;
        }
        /// <summary>
        /// 所有的控件
        /// </summary>
        List<ControlDetailForPage> _lstControl = new List<ControlDetailForPage>();
        /// <summary>
        /// json解析类
        /// </summary>
        Controller.ClassDecode _clsDecode = new Controller.ClassDecode();
        private void SetPageDetailInfo(pageDetailReturnData obj)
        {
            dataForPageDetail dtObj = obj.data;
            try
            {
                if (object.Equals(obj.data.control_list, null) || obj.data.control_list.Length < 2)
                {
                    throw new Exception("页面中缺少基本控件！");
                }
                _lstControl = Controller.ControlCaster.CastArrayToControl(obj.data.control_list);
                //draw navigationBar
                ControlDetailForPage navigationBarObj = _lstControl.Where(p => xinLongyuControlType.navigationBarType.Equals(p.ctrl_type)).ToList()[0];
                Panel parent = pnlPaintBoard;
                DrawControl(navigationBarObj, parent);
                _navigationBarHeight = navigationBarObj.d2;
                //draw page
                ControlDetailForPage pageObj = _lstControl.Where(p => xinLongyuControlType.pageType.Equals(p.ctrl_type)).ToList()[0];
                this.pnlPaintBoard.Tag = pageObj;
                if (string.IsNullOrEmpty(pageObj.d0))
                {
                    return;
                }
                List<int> pageControlStrList = _clsDecode.DecodeArray(pageObj.d0);
                foreach (int id in pageControlStrList)
                {
                    ControlDetailForPage control = _lstControl.Where(p => id == p.ctrl_id).ToList()[0];
                    Panel parentPanel = this.pnlPaintBoard;
                    DrawControl(control, parentPanel);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("加载页面信息出错:" + ex.Message);
                Logging.Error(ex.Message);
                return;
            }

        }
        /// <summary>
        /// 控件ID
        /// </summary>
        private int _randomControlName = 0;
        /// <summary>
        /// 导航栏高度
        /// </summary>
        private int _navigationBarHeight = 0;

        /// <summary>
        /// 递归绘制界面
        /// </summary>
        /// <param name="obj"></param>
        private void DrawControl(ControlDetailForPage obj, Panel parent)
        {
            if (xinLongyuControlType.textType.Equals(obj.ctrl_type))
            {
                xinLongyuText textControl = new xinLongyuText();
                textControl.BorderStyle = BorderStyle.FixedSingle;
                SetContronInfo(textControl, obj, parent);
                textControl.AutoSize = true;
                //textControl.Text = obj.d0.ToString();
            }
            else if (xinLongyuControlType.imgType.Equals(obj.ctrl_type))
            {
                xinLongyuPanel imgControl = new xinLongyuPanel();
                //imgControl.SizeMode = PictureBoxSizeMode.Zoom;
                imgControl.BackgroundImageLayout = ImageLayout.Stretch;
                imgControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                imgControl.BorderStyle = BorderStyle.FixedSingle;
                SetContronInfo(imgControl, obj, parent);
                imgControl.BackgroundImage = _paintboardController.GetImageByUrl(obj.d0.ToString());
            }
            else if (xinLongyuControlType.uploadImageType.Equals(obj.ctrl_type))
            {
                xinLongyuButton btnControl = new xinLongyuButton();
                btnControl.AutoSize = true;
                SetContronInfo(btnControl, obj, parent);
                btnControl.Text = "图片上传";
            }
            else if (xinLongyuControlType.buttonType.Equals(obj.ctrl_type))
            {
                xinLongyuButton btnControl = new xinLongyuButton();
                //btnControl.AutoSize = true;
                SetContronInfo(btnControl, obj, parent);
            }
            else if (xinLongyuControlType.inputType.Equals(obj.ctrl_type))
            {
                xinLongyuInput inputControl = new xinLongyuInput();
                inputControl.AutoSize = true;
                SetContronInfo(inputControl, obj, parent);
            }
            else if (xinLongyuControlType.rtfType.Equals(obj.ctrl_type))
            {
                xinLongyuRtf rtfControl = new xinLongyuRtf();
                rtfControl.AutoSize = true;
                SetContronInfo(rtfControl, obj, parent);
            }
            else if (xinLongyuControlType.timerType.Equals(obj.ctrl_type))
            {
                xinLongyuButton timerControl = new xinLongyuButton();
                timerControl.AutoSize = true;
                SetContronInfo(timerControl, obj, parent);
                timerControl.Text = "计时控件";
            }
            else if (xinLongyuControlType.listsType.Equals(obj.ctrl_type))
            {
                xinLongyuPictureBox listControl = new xinLongyuPictureBox();
                listControl.SizeMode = PictureBoxSizeMode.StretchImage;
                listControl.BorderStyle = BorderStyle.FixedSingle;
                SetContronInfo(listControl, obj, parent);
                listControl.Image = Properties.Resources.xinLongyuList;
            }
            else if (xinLongyuControlType.tabbarType.Equals(obj.ctrl_type))
            {
                xinLongyuPictureBox tabControl = new xinLongyuPictureBox();
                tabControl.SizeMode = PictureBoxSizeMode.StretchImage;
                tabControl.BorderStyle = BorderStyle.FixedSingle;
                SetContronInfo(tabControl, obj, parent);
                tabControl.Image = Properties.Resources.xinLongyuTabBar;
            }
            else if (xinLongyuControlType.webviewType.Equals(obj.ctrl_type))
            {
                xinLongyuWebView webviewControl = new xinLongyuWebView();
                webviewControl.SizeMode = PictureBoxSizeMode.StretchImage;
                webviewControl.BorderStyle = BorderStyle.FixedSingle;
                SetContronInfo(webviewControl, obj, parent);
                webviewControl.Image = Properties.Resources.webviewImage;
            }
            else if (xinLongyuControlType.superViewType.Equals(obj.ctrl_type) || xinLongyuControlType.navigationBarType.Equals(obj.ctrl_type))
            {
                xinLongyuPanel pnlControl = new xinLongyuPanel();
                pnlControl.BorderStyle = BorderStyle.Fixed3D;
                SetContronInfo(pnlControl, obj, parent);
                List<int> pageControlStrList = _clsDecode.DecodeArray(obj.d0.ToString());
                List<ControlDetailForPage> lstView = _lstControl.Where(p => pageControlStrList.Contains(p.ctrl_id)).OrderBy(p => p.ctrl_level).ToList();
                foreach(ControlDetailForPage controlObj in lstView)
                {
                    DrawControl(controlObj, pnlControl);
                }
            }
        }
        /// <summary>
        /// 设置控件所有信息
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="obj"></param>
        /// <param name="parent"></param>
        private void SetContronInfo(System.Windows.Forms.Control ct, ControlDetailForPage obj, Panel parent)
        {
            if (xinLongyuConverter.StringToBool(obj.d18))
            {
                SetControlBaseInfo(ct, obj, parent);
            }
            else
            {
                SetInvisibleControl(ct, obj, this.pnlInvisible);
            }
        }
        /// <summary>
        /// 隐性控件集合
        /// </summary>
        private List<System.Windows.Forms.Control> _lstInvisibleControl = new List<System.Windows.Forms.Control>();

        /// <summary>
        /// 设置隐性控件
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="obj"></param>
        /// <param name="parent"></param>
        private void SetInvisibleControl(System.Windows.Forms.Control ct, ControlDetailForPage obj, Panel parent)
        {
            if (_lstInvisibleControl.Count < 1)
            {
                obj.d1 = 50;
                obj.d2 = 25;
                obj.d3 = 10;
                obj.d4 = 50;
            }
            else
            {
                System.Windows.Forms.Control previousControl = _lstInvisibleControl[_lstInvisibleControl.Count - 1];
                obj.d3 = previousControl.Location.X + 10 + previousControl.Width;
                obj.d4 = _lstInvisibleControl[_lstInvisibleControl.Count - 1].Location.Y;
            }
            SetControlBaseInfo(ct, obj, parent);
            _lstInvisibleControl.Add(ct);
        }

        /// <summary>
        /// 设置可视化控件
        /// </summary>
        /// <param name="ct">绘制的控件</param>
        /// <param name="obj">控件类实体</param>
        /// <param name="pnl">所要绘制到的画板</param>
        private void SetVisibleControl(System.Windows.Forms.Control ct, ControlDetailForPage obj, Panel pnl)
        {
            SetControlBaseInfo(ct, obj, pnl);
        }

        /// <summary>
        /// 设置控件基本信息
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="obj"></param>
        /// <param name="pnl"></param>
        private void SetControlBaseInfo(System.Windows.Forms.Control ct, ControlDetailForPage obj, Panel pnl)
        {
            ct.Name = obj.ctrl_type + "_" + _randomControlName.ToString();
            if (pnl == this.pnlPaintBoard)
            {
                if (_paintboardController._isLocal)
                {
                    ct.Location = new Point(obj.d3, obj.d4);
                }
                else
                {
                    ct.Location = new Point(obj.d3, obj.d4 + _navigationBarHeight);
                }
            }
            else
            {
                ct.Location = new Point(obj.d3, obj.d4);
            }
            ct.Size = new System.Drawing.Size(obj.d1, obj.d2);
            ct.Tag = obj;
            ct.Text = obj.d0;
            if (string.IsNullOrEmpty(ct.Text))
            {
                ct.Text = obj.ctrl_type;
            }
            ct.ContextMenu = _cmMunu;
            //SetControlMovableAndResizable(ct);
            pnl.Controls.Add(ct);
            ct.BringToFront();
            ct.MouseDown += new MouseEventHandler(this.MouseDownForDrag);
            ct.MouseMove += new MouseEventHandler(this.MouseMoveForDrag);
            ct.SizeChanged += Ct_SizeChanged;
            _randomControlName++;

        }

        private void Ct_SizeChanged(object sender, EventArgs e)
        {
            ChangeControlProperty();
        }

        private void pnlPaintBoard_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void pnlPaintBoard_DragDrop(object sender, DragEventArgs e)
        {
            if (object.Equals(_currentPageInfo, null))
            {
                ShowBalloonTip("警告", "请先选择一个页面", ToolTipIcon.Info, 2000);
                return;
            }
            toolboxItem data = e.Data.GetData(typeof(toolboxItem)) as toolboxItem;
            Point NewPoint = new Point(0, 0);
            if (data.Visible)
            {
                NewPoint = this.pnlPaintBoard.PointToClient(new Point(e.X, e.Y));
            }
            ControlDetailForPage obj = _paintboardController.GetNewControlObj(_lstControl, data.ControlType, _currentPageId, NewPoint, data.Visible);
            _paintboardController.AddNewControlToPageD0(_lstControl, obj);
            this.DrawControl(obj, this.pnlPaintBoard);
        }

        /// <summary>
        /// 用于显示警告或者提示
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="icon"></param>
        /// <param name="timeout"></param>
        void ShowBalloonTip(string title, string content, ToolTipIcon icon, int timeout)
        {
            icnTips.BalloonTipTitle = title;
            icnTips.BalloonTipText = content;
            icnTips.BalloonTipIcon = icon;
            icnTips.ShowBalloonTip(timeout);
        }

        #region 与属性界面的交互
        private void ChangeControlProperty()
        {
            if (object.Equals(CurrentControl, null))
            {
                return;
            }
            _paintboardController.ChangeControlProperty(CurrentControl, _lstControl);
            controlPropertyChange.Invoke(CurrentControl.Tag);
        }

        public void SetControlProperty(Object obj)
        {
            _paintboardController.SetControlProperty(obj, CurrentControl);
        }
        #endregion

        #region 拖动控件事件
        private Point MouseDownLocation;
        private System.Windows.Forms.Control CurrentControl;
        private void MouseDownForDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
            CurrentControl = sender as System.Windows.Forms.Control;
            ChangeControlProperty();
        }

        private void MouseMoveForDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                System.Windows.Forms.Control parent = (sender as System.Windows.Forms.Control).Parent;
                System.Windows.Forms.Control children = (sender as System.Windows.Forms.Control);
                int leftDistance = e.X + children.Left - MouseDownLocation.X;
                int topDistance = e.Y + children.Top - MouseDownLocation.Y;
                int changeTag = 0;
                if (leftDistance >= 0 && (leftDistance + children.Width <= parent.Width))
                {
                    children.Left = leftDistance;
                    changeTag = 1;
                }
                if (topDistance >= 0 && (topDistance + children.Height <= parent.Height))
                {
                    children.Top = topDistance;
                    changeTag = 1;
                }
                if (changeTag != 0)
                {
                    ChangeControlProperty();
                }
            }
        }
        #endregion

        #region 鼠标右键菜单
        private ContextMenu GetContextMenu()
        {
            ContextMenu cm = new ContextMenu();
            MenuItem item_1 = new MenuItem("删除");
            item_1.Click += new EventHandler(ContextMunuHandle);
            MenuItem item_2 = new MenuItem("属性");
            item_2.Click += new EventHandler(ContextMunuHandle);
            MenuItem item_3 = new MenuItem("页面属性");
            item_3.Click += new EventHandler(ContextMunuHandle);
            cm.MenuItems.Add(item_1);
            cm.MenuItems.Add(item_2);
            cm.MenuItems.Add(item_3);
            return cm;
        }

        private void ContextMunuHandle(object sender, EventArgs e)
        {
            string text = (sender as MenuItem).Text;
            switch (text)
            {
                case "属性":
                    ChangeControlProperty();
                    break;
                case "删除":
                    DeleteControl();
                    break;
                case "页面属性":
                    CurrentControl = this.pnlPaintBoard;
                    ChangeControlProperty();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 删除控件
        /// </summary>
        private void DeleteControl()
        {
            //_lstControl.Remove(CurrentControl.Tag as ControlDetailForPage);
            if (this.pnlPaintBoard.Controls.Contains(CurrentControl))
            {
                this.pnlPaintBoard.Controls.Remove(CurrentControl);
            }
            else if (this.pnlInvisible.Controls.Contains(CurrentControl))
            {
                this.pnlInvisible.Controls.Remove(CurrentControl);
                _lstInvisibleControl.Remove(CurrentControl);
            }
            this._paintboardController.DeleteControlFromPageD0(_lstControl, CurrentControl.Tag as ControlDetailForPage);
            CurrentControl = null;
        }
        #endregion

        /// <summary>
        /// 保存缓存，供主窗体调用
        /// </summary>
        /// <returns></returns>
        public int SaveCache()
        {
            return _paintboardController.SaveCache(_currentPageInfo, _lstControl);
        }

        public int Upload()
        {
            return _paintboardController.Upload(_currentPageInfo, _lstControl);
        }

    }
}
