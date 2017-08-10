using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using xinLongIDE.Controller.dataDic.xinlongyuEnum;
using xinLongIDE.View.ExtendForm;

namespace xinLongIDE.View.MainForm
{
    public partial class frmMainIDE : Form
    {
        public frmMainIDE()
        {
            InitializeComponent();
            Init();
        }

        frmPageManager _formPageManager;
        frmControlProperty _formControlProperty;
        frmToolBox _formToolBox;
        /// <summary>
        /// 画板容器
        /// </summary>
        frmTabForPaintBoard _formPaintBoardContainer;

        private void Init()
        {
            this.Visible = false;
            frmWaiting frmWait = new frmWaiting();
            frmWait.TopMost = true;
            frmWait.BringToFront();
            frmWait.Show();
            //这里加了一下进度条，不过好像由于本来就不耗时，所以没什么效果
            ShowPageToolbox();
            ShowPageControlProperty();
            frmWait.SetProgress(30);
            ShowPageManager();
            frmWait.SetProgress(60);
            ShowPaintBoardContainer();
            frmWait.SetProgress(100);
            frmWait.Close();
            this.Visible = true;
        }

        private void ShowPageToolbox()
        {
            _formToolBox = new frmToolBox();
            _formToolBox.MdiParent = this;
            _formToolBox.Show();
        }
        /// <summary>
        /// 控件属性
        /// </summary>
        private void ShowPageControlProperty()
        {
            _formControlProperty = new frmControlProperty();
            _formControlProperty.controlPropertyChange += _formControlProperty_controlPropertyChange;
            _formControlProperty.MdiParent = this;
            _formControlProperty.Show();
        }

        private void _formControlProperty_controlPropertyChange(object obj)
        {
            _formPaintBoardContainer.SetControlProperty(obj);
        }

        /// <summary>
        /// 页面管理界面
        /// </summary>
        private void ShowPageManager()
        {
            _formPageManager = new frmPageManager();
            _formPageManager.nodeSelected += new frmPageManager.delegateForSelectedNode(ShowPageInfo);
            _formPageManager.pageDownloaded += _formPageManager_pageDownloaded;
            _formPageManager.pageCreated += _formPageManager_pageCreated;
            string platForm = "app";
            _formPageManager.SetPages(platForm);
            _formPageManager.MdiParent = this;
            _formPageManager.Show();
        }

        private void _formPageManager_pageCreated(object page)
        {
            //_formPaintBoard.CreatePage(page);
            _formPaintBoardContainer.CreatePage(page);
        }

        private void _formPageManager_pageDownloaded(object obj)
        {
            //画板进行保存操作
            //_formPaintBoard.SavePageDetail(obj);
            _formPaintBoardContainer.SavePageDetail(obj);
        }

        /// <summary>
        /// 画板集合容器
        /// </summary>
        private void ShowPaintBoardContainer()
        {
            _formPaintBoardContainer = new frmTabForPaintBoard();
            _formPaintBoardContainer.progressChange += ChangeProgressValue;
            _formPaintBoardContainer.controlPropertyChange += _formPaintBoard_controlPropertyChange;
            _formPaintBoardContainer.MdiParent = this;
            _formPaintBoardContainer.Show();
        }

        private void _formPaintBoard_controlPropertyChange(object obj)
        {
            _formControlProperty.SetControlProperty(obj);
        }

        private void ChangeProgressValue(int value)
        {
            this.prgStatus.Value = value;
        }

        private void ShowPageInfo(object obj)
        {
            //_formPaintBoard.ShowPageDetail(obj);
            _formPaintBoardContainer.AddPaintBoard(obj);
        }

        #region 实现保存窗体上次状态的代码
        /// <summary>
        /// 页面关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMainIDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFormStatus();
        }
        /// <summary>
        /// 页面布局配置逻辑层
        /// </summary>
        private Controller.ConfigCMD.windowsStatusController _formStatusController = new Controller.ConfigCMD.windowsStatusController();

        private void frmMainIDE_Load(object sender, EventArgs e)
        {
            List<Model.Setting.FormStatusSettings> _lstFormStatus = new List<Model.Setting.FormStatusSettings>();
            _lstFormStatus = _formStatusController.GetFormSettings();
            if (!object.Equals(_lstFormStatus, null))
            {
                SetFormStatus(this, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.Z主窗体]);
                SetFormStatus(_formControlProperty, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.S属性窗体]);
                SetFormStatus(_formPageManager, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.Y页面管理窗体]);
                SetFormStatus(_formPaintBoardContainer, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.H画布窗体]);
                //SetFormStatus(_formPaintBoard, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.H画布窗体]);
                SetFormStatus(_formToolBox, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.G工具箱窗体]);
            }
            
        }

        private void SetFormStatus(Form frm, Model.Setting.FormStatusSettings settings)
        {
            Point newPoint = settings.pointConfig;
            Size newSize = settings.SizeConfig;
            frm.Size = newSize;
            frm.Location = newPoint;
        }

        private void SaveFormStatus()
        {
            List<Model.Setting.FormStatusSettings> _lstFormStatus = new List<Model.Setting.FormStatusSettings>();
            _lstFormStatus.Add(GetFormStatusSettingsEntity(this));
            _lstFormStatus.Add(GetFormStatusSettingsEntity(_formControlProperty));
            _lstFormStatus.Add(GetFormStatusSettingsEntity(_formPageManager));
            //_lstFormStatus.Add(GetFormStatusSettingsEntity(_formPaintBoard));
            _lstFormStatus.Add(GetFormStatusSettingsEntity(_formPaintBoardContainer));
            _lstFormStatus.Add(GetFormStatusSettingsEntity(_formToolBox));
            _formStatusController.SaveFormStatusSettings(_lstFormStatus);
        }
        
        private Model.Setting.FormStatusSettings GetFormStatusSettingsEntity(Form frm)
        {
            Model.Setting.FormStatusSettings setting = new Model.Setting.FormStatusSettings();
            setting.SizeConfig = frm.Size;
            setting.pointConfig = new Point(frm.Location.X, frm.Location.Y);
            return setting;
        }
        #endregion

        /// <summary>
        /// 保存缓存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            //画板缓存
            //_formPaintBoard.SaveCache();
            _formPaintBoardContainer.SaveCache();
            //组管理缓存
            //_formPageManager.SaveCache();
            MessageBox.Show("保存成功！");
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnUpload_Click(object sender, EventArgs e)
        {
            _formPageManager.Upload();
            _formPaintBoardContainer.Upload();
            MessageBox.Show("上传成功！");

        }
    }
}
