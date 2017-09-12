using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using xinLongIDE.Controller.CommonController;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Controller.dataDic;
using xinLongIDE.Controller.xinlongyuEnum;
using xinLongIDE.Model.Control;
using xinLongIDE.Model.Page;
using xinLongIDE.Model.returnJson;
using xinLongIDE.View.Control;

namespace xinLongIDE.View.MainForm
{
    public partial class frmPaintBoard : Form
    {
        //委托与事件，用于实现进度条的回调
        public delegate void delegateForProgressBar(int value);
        public event delegateForProgressBar progressChange;

        //用于实现属性值改变
        public delegate void delegateForControlPropertyChanged(object obj);
        public event delegateForControlPropertyChanged controlPropertyChange;

        //用于实现页面ID以及页面名称改变
        public delegate void delegateForPagePropertyChanged(object objOld, object objNew);
        public event delegateForPagePropertyChanged pagePropertyChanged;

        //用于委托页面管理对组信息进行上传
        public delegate void delegateForGroupUpload();
        public event delegateForGroupUpload groupUpload;

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
        /// <summary>
        /// 当前页面名称
        /// </summary>
        private string _currentPageName = string.Empty;
        /// <summary>
        /// 构造函数
        /// </summary>
        public frmPaintBoard()
        {
            InitializeComponent();
            _paintboardController = new paintBoardController();
            _cmMunu = this.GetContextMenu();
            AddContextMenuForPaintBoard();
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
                _currentPageInfo = await _paintboardController.GetPageDetailInfo(pageObj.PageId, pageObj.Plat_form);
                if (_paintboardController.errorCode == -2)
                {
                    MessageBox.Show("获取页面信息出错，请检查网络！");
                    progressChange.Invoke(100);
                    return;
                }

                progressChange.Invoke(50);
                //对页面进行解析
                this.Clear();
                _currentPageId = pageObj.PageId;
                _currentPageName = pageObj.PageName;

                _currentPageInfo.data.page_id = pageObj.PageId;
                _currentPageInfo.data.group_id = pageObj.GroupId;

                this.SetBaseInfo(_currentPageInfo, pageObj.Plat_form);
                progressChange.Invoke(75);
                this.SetPageDetailInfo(_currentPageInfo, pageObj.Plat_form);
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
            this._listFormControl.Clear();
            this._lstInvisibleControl.Clear();
            //_navigationBarHeight = 0;
        }
        /// <summary>
        /// 设置窗体的基本信息
        /// </summary>
        /// <param name="obj"></param>
        private void SetBaseInfo(pageDetailReturnData obj, string plat_form)
        {
            dataForPageDetail dtObj = obj.data;
            if (!string.IsNullOrEmpty(dtObj.page_title))
            {
                this.Text = dtObj.page_title;
            }
            this.BackColor = System.Drawing.ColorTranslator.FromHtml(dtObj.page_background_color);
            if ("app".Equals(plat_form))
            {
                this.pnlPaintBoard.Width = 375;
                this.pnlPaintBoard.Height = 667;
                this.pnlNavigationBar.Visible = true;
            }
            else
            {
                this.pnlPaintBoard.Width = 1366;
                this.pnlPaintBoard.Height = 768;
                this.pnlNavigationBar.Visible = false;
            }
            this.lstViewVisibleControl.Location = new Point(this.pnlPaintBoard.Location.X + this.pnlPaintBoard.Width + 10, this.lstViewVisibleControl.Location.Y);
            this.pnlInvisible.Location = new Point(this.pnlInvisible.Location.X, this.pnlPaintBoard.Location.Y + this.pnlPaintBoard.Height + 20);
            dtObj = null;
        }
        /// <summary>
        /// 所有的控件实体
        /// </summary>
        List<ControlDetailForPage> _lstControl = new List<ControlDetailForPage>();
        /// <summary>
        /// 所有的窗体控件
        /// </summary>
        List<System.Windows.Forms.Control> _listFormControl = new List<System.Windows.Forms.Control>();
        /// <summary>
        /// json解析类
        /// </summary>
        ClassDecode _clsDecode = new ClassDecode();
        private void SetPageDetailInfo(pageDetailReturnData obj, string platForm)
        {
            dataForPageDetail dtObj = obj.data;
            try
            {
                if (object.Equals(obj.data.control_list, null))
                {
                    //当页面中没有控件时，直接跳过(这部分时针对pc的情况)
                    return;
                }
                _lstControl = ControlCaster.CastArrayToControl(obj.data.control_list);
                _lstControl = _lstControl.OrderBy(p => p.ctrl_level).ToList();
                if (object.Equals(_lstControl, null) || _lstControl.Count < 1)
                {
                    return;
                }
                if (platFormEnum.app.ToString().Equals(platForm))
                {
                    if (object.Equals(obj.data.control_list, null) || obj.data.control_list.Length < 2)
                    {
                        throw new Exception("页面中缺少基本控件！");
                    }
                }
                if (object.Equals(_lstControl, null) || _lstControl.Count < 1)
                {
                    return;
                }
                //辅助控件列表
                this.SetConsistentControlListbox();

                ControlDetailForPage pageObj = _lstControl.Where(p => xinLongyuControlType.pageType.Equals(p.ctrl_type)).ToList()[0];
                this.pnlPaintBoard.Tag = pageObj;
                this.pnlPaintBoard.Click += (s, e) => { CurrentControl = this.pnlPaintBoard; this.ChangeControlProperty(); };
                this.pnlPaintBoard.MouseDown += MouseDownForDrag;
                this._listFormControl.Add(pnlPaintBoard);
                if (!string.IsNullOrEmpty(pageObj.d0))
                {

                    List<int> pageControlStrList = _clsDecode.DecodeArray(pageObj.d0);
                    foreach (int id in pageControlStrList)
                    {
                        if (!_lstControl.Any(p => id == p.ctrl_id))
                        {
                            continue;
                        }
                        ControlDetailForPage control = _lstControl.Where(p => id == p.ctrl_id).First();
                        Panel parentPanel = this.pnlPaintBoard;
                        DrawControl(control, parentPanel);
                    }
                }
                //如果配置的是app的话才需要生成navigationBar
                if (platFormEnum.app.ToString().Equals(platForm))
                {
                    ControlDetailForPage navigationBarObj = _lstControl.First(p => xinLongyuControlType.navigationBarType.Equals(p.ctrl_type));
                    this.pnlNavigationBar.Tag = navigationBarObj;
                    this.pnlNavigationBar.Click += (s, e) => { CurrentControl = this.pnlNavigationBar; this.ChangeControlProperty(); };
                    _listFormControl.Add(this.pnlNavigationBar);
                    Panel parent = pnlNavigationBar;
                    if (!string.IsNullOrEmpty(navigationBarObj.d0))
                    {
                        List<int> controlList = _clsDecode.DecodeArray(navigationBarObj.d0);
                        List<ControlDetailForPage> lstView = _lstControl.Where(p => controlList.Contains(p.ctrl_id)).OrderBy(p => p.ctrl_level).ToList();
                        foreach (ControlDetailForPage controlObj in lstView)
                        {
                            DrawControl(controlObj, parent);
                        }
                    }
                }
                //pnlNavigationBar.BringToFront();
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
        /// 递归绘制界面
        /// </summary>
        /// <param name="obj"></param>
        private Panel DrawControl(ControlDetailForPage obj, System.Windows.Forms.Control parent)
        {
            if (!toolboxController.IsVisible(obj.ctrl_type))
            {
                xinLongyuButton btnControl = new xinLongyuButton();
                string text = toolboxController.GetControlTypeChineseName(obj.ctrl_type);
                this.SetButtonTypeControl(btnControl, obj, parent, text);
            }
            else if (xinLongyuControlType.textType.Equals(obj.ctrl_type) || xinLongyuControlType.GridColumnName.Equals(obj.ctrl_type))
            {
                xinLongyuButton textControl = new xinLongyuButton();
                //textControl.Enabled = false;
                SetContronInfo(textControl, obj, parent);
            }
            else if (xinLongyuControlType.imgType.Equals(obj.ctrl_type))
            {
                xinLongyuImg imgControl = new xinLongyuImg();
                Image img = _paintboardController.GetImageByUrl(obj.d0.ToString());
                this.SetPictureTypeControl(imgControl, obj, parent, img);
            }
            else if (xinLongyuControlType.uploadImageType.Equals(obj.ctrl_type))
            {
                xinLongyuButton btnControl = new xinLongyuButton();
                string text = "图片上传";
                this.SetButtonTypeControl(btnControl, obj, parent, text);
            }
            else if (xinLongyuControlType.buttonType.Equals(obj.ctrl_type))
            {
                xinLongyuButton btnControl = new xinLongyuButton();
                SetContronInfo(btnControl, obj, parent);
            }
            else if (xinLongyuControlType.inputType.Equals(obj.ctrl_type) || xinLongyuControlType.rtfType.Equals(obj.ctrl_type))
            {
                xinLongyuInput inputControl = new xinLongyuInput();
                inputControl.Multiline = true;
                SetContronInfo(inputControl, obj, parent);
            }
            else if (xinLongyuControlType.rtfType.Equals(obj.ctrl_type))
            {
                xinLongyuRtf rtfControl = new xinLongyuRtf();
                SetContronInfo(rtfControl, obj, parent);
            }
            else if (xinLongyuControlType.timerType.Equals(obj.ctrl_type))
            {
                xinLongyuButton timerControl = new xinLongyuButton();
                SetContronInfo(timerControl, obj, parent);
                timerControl.Text = "计时控件";
            }
            else if (xinLongyuControlType.tabbarType.Equals(obj.ctrl_type))
            {
                xinLongyuPictureBox tabControl = new xinLongyuPictureBox();
                Image img = Properties.Resources.xinLongyuTabBar;
                this.SetPictureTypeControl(tabControl, obj, parent, img);
            }
            else if (xinLongyuControlType.webviewType.Equals(obj.ctrl_type))
            {
                xinLongyuWebView webviewControl = new xinLongyuWebView();
                Image img = Properties.Resources.webviewImage;
                this.SetPictureTypeControl(webviewControl, obj, parent, img);
            }
            else if (xinLongyuControlType.numSelectorType.Equals(obj.ctrl_type))
            {
                xinLongyuImg numSelectorControl = new xinLongyuImg();
                Image img = Properties.Resources.xinlongNumSelector;
                this.SetPictureTypeControl(numSelectorControl, obj, parent, img);
            }
            else if (xinLongyuControlType.RatingBarType.ToLower().Equals(obj.ctrl_type.ToLower()))
            {
                xinLongyuImg ratingbarControl = new xinLongyuImg();
                Image img = Properties.Resources.xinlongyuRatingBar;
                this.SetPictureTypeControl(ratingbarControl, obj, parent, img);
            }
            else if (_paintboardController._fatherControlList.Contains(obj.ctrl_type))
            {
                System.Windows.Forms.Control pnlControl = new xinLongyuPanel();
                if (!xinLongyuControlType.navigationBarType.Equals(obj.ctrl_type))
                {
                    (pnlControl as xinLongyuPanel).BorderStyle = BorderStyle.FixedSingle;
                    SetContronInfo(pnlControl, obj, parent);
                }
                else
                {
                    pnlControl = this.pnlNavigationBar;
                }
                string controllist = xinLongyuControlType.superViewType.Equals(obj.ctrl_type) ? obj.d0 : obj.d17;
                if (!string.IsNullOrEmpty(controllist))
                {
                    List<int> pageControlStrList = _clsDecode.DecodeArray(controllist);
                    List<ControlDetailForPage> lstView = _lstControl.Where(p => pageControlStrList.Contains(p.ctrl_id)).OrderBy(p => p.ctrl_level).ToList();
                    foreach (ControlDetailForPage controlObj in lstView)
                    {
                        DrawControl(controlObj, pnlControl);
                    }
                }
                SetConsistentControlListbox();
                return pnlControl as Panel;
            }
            else
            {
                if (xinLongyuConverter.StringToBool(obj.d18))
                {
                    xinLongyuImg listControl = new xinLongyuImg();
                    Image img = Properties.Resources.defaultImg;
                    this.SetPictureTypeControl(listControl, obj, parent, img);
                }
                else
                {
                    xinLongyuButton btnControl = new xinLongyuButton();
                    string text = obj.d0;
                    this.SetButtonTypeControl(btnControl, obj, parent, text);
                }
            }
            //辅助控件列表
            SetConsistentControlListbox();
            return null;
        }

        /// <summary>
        /// 设置辅助控件列表
        /// </summary>
        private void SetConsistentControlListbox()
        {
            //List<System.Windows.Forms.Control> listControlToShow = _listFormControl.Where(p => xinLongyuConverter.StringToBool((p.Tag as ControlDetailForPage).d18)).ToList();
            //this.lstViewVisibleControl.Items.Clear();
            //foreach (System.Windows.Forms.Control ct in listControlToShow)
            //{
            //    ControlDetailForPage ctobj = ct.Tag as ControlDetailForPage;
            //    //这里后续再用名称来做标识
            //    ListViewItem newItem = new ListViewItem(ctobj.ctrl_id + "-" + ctobj.ctrl_type);
            //    newItem.Tag = ct;
            //    this.lstViewVisibleControl.Items.Add(newItem);
            //    //ListBoxItemObj item = new ListBoxItemObj(ctobj.ctrl_id + "-" + ctobj.ctrl_type + "-" + ctobj.d0, ct);
            //    //lstVisibleControl.Items.Add(item);
            //}

            //改为显示所有的控件
            this.lstViewVisibleControl.Items.Clear();
            _lstControl = _lstControl.OrderBy(p => p.ctrl_id).ToList();
            foreach (ControlDetailForPage ctobj in this._lstControl)
            {
                ListViewItem newItem = new ListViewItem(ctobj.ctrl_id + "-" + ctobj.ctrl_type);
                newItem.Tag = ctobj.ctrl_id;
                this.lstViewVisibleControl.Items.Add(newItem);
            }
        }

        /// <summary>
        /// 设置图片类控件
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="obj"></param>
        /// <param name="pnl"></param>
        private void SetPictureTypeControl(PictureBox ct, ControlDetailForPage obj, System.Windows.Forms.Control pnl, Image img)
        {
            ct.SizeMode = PictureBoxSizeMode.StretchImage;
            ct.BorderStyle = BorderStyle.FixedSingle;
            SetContronInfo(ct, obj, pnl);
            if (!object.Equals(img, null))
            {
                ct.Image = img;
            }
        }

        private void SetButtonTypeControl(Button btn, ControlDetailForPage obj, System.Windows.Forms.Control pnl, String text)
        {
            //btn.AutoSize = true;
            SetContronInfo(btn, obj, pnl);
            btn.Text = text;
        }

        /// <summary>
        /// 设置控件所有信息
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="obj"></param>
        /// <param name="parent"></param>
        private void SetContronInfo(System.Windows.Forms.Control ct, ControlDetailForPage obj, System.Windows.Forms.Control parent)
        {
            //if (xinLongyuConverter.StringToBool(obj.d18))
            //这里改成按照控件类型判断显隐性
            if (toolboxController.IsVisible(obj.ctrl_type))
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
            //obj.ctrl_type = xinLongyuControlType.buttonType;
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
        private void SetControlBaseInfo(System.Windows.Forms.Control ct, ControlDetailForPage obj, System.Windows.Forms.Control pnl)
        {
            ct.Name = obj.ctrl_type + "_" + _randomControlName.ToString();
            ct.Location = new Point(obj.d3, obj.d4);
            ct.Text = obj.d0;
            if (toolboxController.IsVisible(obj.ctrl_type))
            {
                ct.Size = new System.Drawing.Size(obj.d1, obj.d2);
            }
            else
            {
                ct.Size = new Size(50, 50);
            }
            ct.Tag = obj;

            if (string.IsNullOrEmpty(ct.Text.Trim()))
            {
                ct.Text = obj.ctrl_type;
            }
            ct.ContextMenu = _cmMunu;
            pnl.Controls.Add(ct);
            ct.BringToFront();
            ct.MouseDown += new MouseEventHandler(this.MouseDownForDrag);
            ct.MouseMove += new MouseEventHandler(this.MouseMoveForDrag);
            ct.SizeChanged += Ct_SizeChanged;
            _randomControlName++;
            _listFormControl.Add(ct);
        }

        /// <summary>
        /// 控件尺寸改变后触发属性窗体的实时显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MessageBox.Show("警告", "请先选择一个页面");
                return;
            }
            toolboxItem data = e.Data.GetData(typeof(toolboxItem)) as toolboxItem;
            AddToolBoxItem(data, e, this.pnlPaintBoard);
        }

        private void AddToolBoxItem(toolboxItem data, DragEventArgs e, Panel targetPanel)
        {
            Point NewPoint = new Point(0, 0);
            Point originalPoint = new Point(e.X, e.Y);
            if (data.Visible)
            {
                NewPoint = targetPanel.PointToClient(new Point(e.X, e.Y));
            }
            ControlDetailForPage obj = _paintboardController.GetNewControlObj(_lstControl, data.ControlType, _currentPageId, NewPoint, data.Visible);

            System.Windows.Forms.Control fatherControl = _paintboardController.AddNewControlToPageD0(_lstControl, _listFormControl, obj, originalPoint, targetPanel, false);
            System.Windows.Forms.Control paintBoard = this.pnlPaintBoard;
            if (!object.Equals(fatherControl, null))
            {
                paintBoard = fatherControl;
            }
            int maxCtrlLevel = 0;

            foreach (System.Windows.Forms.Control ct in paintBoard.Controls)
            {
                if ((ct.Tag as ControlDetailForPage).ctrl_level > maxCtrlLevel)
                {
                    maxCtrlLevel = (ct.Tag as ControlDetailForPage).ctrl_level;
                }
            }

            maxCtrlLevel++;
            if (paintBoard.Controls.Count < 1)
            {
                maxCtrlLevel = 0;
            }
            obj.ctrl_level = maxCtrlLevel;

            this.DrawControl(obj, paintBoard);
        }

        #region 与属性界面的交互
        /// <summary>
        /// 关联属性界面
        /// </summary>
        private void ChangeControlProperty()
        {
            if (object.Equals(CurrentControl, null))
            {
                return;
            }
            _paintboardController.ChangeControlProperty(CurrentControl, _lstControl);
            controlPropertyChange.Invoke(CurrentControl.Tag);
            //这里再添加一个页面属性的委托
            basePageProperty pagePropertyObj = ConvertPageInfoToBaseProperty();
            controlPropertyChange.Invoke(pagePropertyObj);

        }

        private basePageProperty ConvertPageInfoToBaseProperty()
        {
            basePageProperty obj = new basePageProperty();
            obj.PageId = _currentPageInfo.data.page_id;
            obj.PageName = _currentPageInfo.data.page_name;
            obj.PageWidth = _currentPageInfo.data.page_width;
            obj.PageHeight = _currentPageInfo.data.page_height;
            obj.UserGroup = _currentPageInfo.data.user_group;
            return obj;
        }

        /// <summary>
        /// 关联属性界面
        /// </summary>
        /// <param name="obj"></param>
        public void SetControlProperty(Object obj)
        {
            if (obj is ControlDetailForPage)
            {
                _paintboardController.SetControlProperty(obj, CurrentControl, _listFormControl, _lstControl);
            }
            else if (obj is basePageProperty)
            {
                //原先的页面信息
                basePageProperty originalPageProperty = this.ConvertPageInfoToBaseProperty();
                _paintboardController.SetPageProperty(obj, _currentPageInfo, _lstControl, _listFormControl, this);
                if (originalPageProperty.PageId != (obj as basePageProperty).PageId || !originalPageProperty.PageName.Equals((obj as basePageProperty).PageName))
                {
                    pagePropertyChanged.Invoke(originalPageProperty, obj);
                }
            }
        }
        #endregion

        #region 拖动控件事件
        private Point MouseDownLocation;
        public System.Windows.Forms.Control CurrentControl;
        private Point _currentCuosorPoint;

        private void MouseDownForDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
            else if (e.Button == MouseButtons.Right)
            {
                _currentCuosorPoint = Cursor.Position;
            }
            CurrentControl = sender as System.Windows.Forms.Control;
            CurrentControl.Focus();
            //CurrentControl.BringToFront();
            ChangeControlProperty();
        }



        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMoveForDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                System.Windows.Forms.Control parent = (sender as System.Windows.Forms.Control).Parent;
                System.Windows.Forms.Control children = (sender as System.Windows.Forms.Control);
                int leftDistance = e.X - MouseDownLocation.X + children.Location.X;
                int topDistance = e.Y - MouseDownLocation.Y + children.Location.Y;
                int changeTag = 0;
                if (leftDistance >= 0 && (leftDistance <= parent.Width))
                {
                    children.Left = e.X + children.Left - MouseDownLocation.X;
                    changeTag = 1;
                }
                if (topDistance >= 0 && (topDistance <= parent.Height))
                {
                    children.Top = e.Y + children.Top - MouseDownLocation.Y;
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
        /// <summary>
        /// 右键菜单
        /// </summary>
        /// <returns></returns>
        private ContextMenu GetContextMenu()
        {
            ContextMenu cm = new ContextMenu();
            MenuItem item_4 = new MenuItem("复制");
            item_4.Click += new EventHandler(ContextMunuHandle);
            MenuItem item_5 = new MenuItem("黏贴");
            item_5.Click += new EventHandler(ContextMunuHandle);
            MenuItem item_1 = new MenuItem("删除");
            item_1.Click += new EventHandler(ContextMunuHandle);
            MenuItem item_2 = new MenuItem("属性");
            item_2.Click += new EventHandler(ContextMunuHandle);
            MenuItem item_3 = new MenuItem("页面属性");
            item_3.Click += new EventHandler(ContextMunuHandle);
            cm.MenuItems.Add(item_1);
            cm.MenuItems.Add(item_2);
            cm.MenuItems.Add(item_3);
            cm.MenuItems.Add(item_4);
            cm.MenuItems.Add(item_5);
            return cm;
        }
        

        /// <summary>
        /// 为画板增加右键黏贴控件的菜单
        /// </summary>
        private void AddContextMenuForPaintBoard()
        {
            ContextMenu cm = new ContextMenu();
            MenuItem item_4 = new MenuItem("黏贴");
            item_4.Click += ContextMunuHandle;
            cm.MenuItems.Add(item_4);
            this.pnlPaintBoard.ContextMenu = cm;
        }

        /// <summary>
        /// 这里用于处理控件右键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                case "复制":
                    if (!object.Equals(CurrentControl, null))
                    {
                        _paintboardController.CopyControl(CurrentControl, _lstControl);
                    }
                    break;
                case "黏贴":
                    if (!object.Equals(CurrentControl, null))
                    {
                        ControlDetailForPage target =  _paintboardController.PasteControl(_currentCuosorPoint, CurrentControl, _lstControl, _listFormControl);
                        this.DrawControl(target, CurrentControl);
                    }
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
            this._paintboardController.DeleteControlFromPageD0(_lstControl, _listFormControl, CurrentControl);
            //
            CurrentControl = null;
            //
            this.SetConsistentControlListbox();
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

        /// <summary>
        /// 把缓存上传上去
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<int> Upload()
        {
            //先上传一下组信息
            groupUpload.Invoke();
            return await _paintboardController.Upload(_currentPageInfo, _lstControl);
        }

        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            if (SaveCache() == 1)
            {
                MessageBox.Show("保存成功!");
            }
            else
            {
                MessageBox.Show("保存失败！");
            }
        }

        private async void tsbtnUpload_Click(object sender, EventArgs e)
        {
            if (await Upload() == 1)
            {

                MessageBox.Show("上传成功!");
            }
            else
            {
                MessageBox.Show("上传失败！");
            }
            //await Upload();
            //MessageBox.Show("上传成功!");
        }

        private void lstViewVisibleControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //System.Windows.Forms.Control ct = this.lstViewVisibleControl.SelectedItems[0].Tag as System.Windows.Forms.Control;
            int controlId = (int)this.lstViewVisibleControl.SelectedItems[0].Tag;
            int controlIndex = _listFormControl.FindIndex(p => (p.Tag as ControlDetailForPage).ctrl_id == controlId);
            if (controlIndex != -1)
            {
                CurrentControl = _listFormControl[controlIndex];
                CurrentControl.Focus();
                this.ChangeControlProperty();
            }
            else
            {
                controlPropertyChange.Invoke(_lstControl.First(p => controlId == p.ctrl_id));
                //这里再添加一个页面属性的委托
                basePageProperty pagePropertyObj = ConvertPageInfoToBaseProperty();
                controlPropertyChange.Invoke(pagePropertyObj);
            }
        }

        private void pnlNavigationBar_DragDrop(object sender, DragEventArgs e)
        {
            if (object.Equals(_currentPageInfo, null))
            {
                MessageBox.Show("警告", "请先选择一个页面");
                return;
            }
            toolboxItem data = e.Data.GetData(typeof(toolboxItem)) as toolboxItem;
            AddToolBoxItem(data, e, this.pnlNavigationBar);
        }

        private void pnlNavigationBar_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
    }
}
