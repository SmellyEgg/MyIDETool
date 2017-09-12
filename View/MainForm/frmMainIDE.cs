using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using xinLongIDE.Controller.CommonController;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Controller.dataDic;
using xinLongIDE.Controller.dataDic.xinlongyuEnum;
using xinLongIDE.Model.Page;
using xinLongIDE.View.ExtendForm;

namespace xinLongIDE.View.MainForm
{
    public partial class frmMainIDE : Form
    {
        public frmMainIDE()
        {
            InitializeComponent();
            Init();
            isOnline();
        }
        /// <summary>
        /// 是否在线
        /// </summary>
        private void isOnline()
        {
            ConnectionController cc = new ConnectionController();
            if (cc.TestConnect())
            {
                Status.isOnline = true;
            }
            else
            {
                Status.isOnline = false;
                MessageBox.Show("当前使用的是离线版本！");
            }
            cc.Dispose();
            cc = null;
        }
        frmPageManager _formPageManager;
        frmControlProperty _formControlProperty;
        frmToolBox _formToolBox;
        /// <summary>
        /// 画板容器
        /// </summary>
        paintBoardManager _formpaintBoardManager;

        private void Init()
        {
            if (this.tscmbType.SelectedIndex == -1)
            {
                this.tscmbType.SelectedIndex = 0;
            }
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
            frmWait.SetProgress(90);
            this.Visible = true;
            ResumeWindowsState();
            //默认设置为app配置界面
            
            frmWait.SetProgress(100);
            frmWait.Close();
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
            _formpaintBoardManager.SetControlProperty(obj);
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
            string platForm = this.tscmbType.SelectedItem.ToString();
            _formPageManager.SetPages(platForm);
            _formPageManager.MdiParent = this;
            _formPageManager.Show();
        }

        private void _formPageManager_pageCreated(object page)
        {
            _formpaintBoardManager.CreatePage(page);
        }

        private void _formPageManager_pageDownloaded(object obj)
        {
            //画板进行保存操作
            _formpaintBoardManager.SavePageDetail(obj);
        }

        /// <summary>
        /// 画板集合容器
        /// </summary>
        private void ShowPaintBoardContainer()
        {
            _formpaintBoardManager = new paintBoardManager(this);
            _formpaintBoardManager.progressChange += ChangeProgressValue;
            _formpaintBoardManager.controlPropertyChange += _formPaintBoard_controlPropertyChange;
            _formpaintBoardManager.pagePropertyChanged += _formpaintBoardManager_pagePropertyChanged;
            _formpaintBoardManager.groupUpload += _formpaintBoardManager_groupUpload;
        }

        private void _formpaintBoardManager_groupUpload()
        {
            this._formPageManager.Upload();
        }

        private void _formpaintBoardManager_pagePropertyChanged(object objOld, object objNew)
        {
            //throw new NotImplementedException();
            _formPageManager.ChangePageProperty(objOld, objNew);
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
            nodeObjectTransfer pageObj = obj as nodeObjectTransfer;
            pageObj.Plat_form = this.tscmbType.SelectedItem.ToString();
            _formpaintBoardManager.AddPaintBoard(pageObj, this);
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
            //保存画板状态
            _formpaintBoardManager.SaveState(this.tscmbType.SelectedItem.ToString());
        }
        /// <summary>
        /// 页面布局配置逻辑层
        /// </summary>
        private Controller.ConfigCMD.windowsStatusController _formStatusController = new Controller.ConfigCMD.windowsStatusController();

        private void ResumeWindowsState()
        {
            List<Model.Setting.FormStatusSettings> _lstFormStatus = new List<Model.Setting.FormStatusSettings>();
            _lstFormStatus = _formStatusController.GetFormSettings();
            if (!object.Equals(_lstFormStatus, null))
            {
                SetFormStatus(this, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.Z主窗体]);
                SetFormStatus(_formControlProperty, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.S属性窗体]);
                SetFormStatus(_formPageManager, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.Y页面管理窗体]);
                SetFormStatus(_formToolBox, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.G工具箱窗体]);
            }
            string platForm = this.tscmbType.SelectedItem.ToString();
            _formpaintBoardManager.ResumePreviousPage(this, platForm);
        }

        private void SetFormStatus(Form frm, Model.Setting.FormStatusSettings settings)
        {
            Point newPoint = settings.pointConfig;
            Size newSize = settings.SizeConfig;
            frm.Size = newSize;
            frm.Location = newPoint;
            //恢复上一次选择的平台，暂时为了方便，使用索引，后续应该保存文本
            int itemIndex = this.tscmbType.Items.IndexOf(settings.PlatForm);
            if (itemIndex != -1)
            {
                this.tscmbType.SelectedIndex = itemIndex;
            }
            
        }

        /// <summary>
        /// 保存窗体状态
        /// </summary>
        private void SaveFormStatus()
        {
            List<Model.Setting.FormStatusSettings> _lstFormStatus = new List<Model.Setting.FormStatusSettings>();
            _lstFormStatus.Add(GetFormStatusSettingsEntity(this));
            _lstFormStatus.Add(GetFormStatusSettingsEntity(_formControlProperty));
            _lstFormStatus.Add(GetFormStatusSettingsEntity(_formPageManager));
            _lstFormStatus.Add(GetFormStatusSettingsEntity(_formToolBox));
            _formStatusController.SaveFormStatusSettings(_lstFormStatus);
        }
        
        private Model.Setting.FormStatusSettings GetFormStatusSettingsEntity(Form frm)
        {
            Model.Setting.FormStatusSettings setting = new Model.Setting.FormStatusSettings();
            setting.SizeConfig = frm.Size;
            setting.pointConfig = new Point(frm.Location.X, frm.Location.Y);
            setting.PlatForm = this.tscmbType.SelectedItem.ToString();
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
            _formpaintBoardManager.SaveCache();
            MessageBox.Show("保存成功！");
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnUpload_Click(object sender, EventArgs e)
        {
            //
            _formPageManager.Upload();
            _formpaintBoardManager.Upload();
            MessageBox.Show("上传成功！");

        }

        private void tscmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!object.Equals(_formpaintBoardManager, null))
            {
                _formpaintBoardManager.Clear();
                _formPageManager.Clear();
                string platForm = this.tscmbType.SelectedItem.ToString();
                _formPageManager.SetPages(platForm);
                ShowPaintBoardContainer();
                _formpaintBoardManager.ResumePreviousPage(this, platForm);
            }
        }

        /// <summary>
        /// 调试用，用来清理一些临时的缓存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string groupFolder = System.Windows.Forms.Application.StartupPath + "\\GroupConfig";
            string pageFolder = ConfigureFilePath.PageDetailFolder;
            string windowsState = Application.StartupPath + @"\WindowsStatusConfig";

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(groupFolder);

            foreach (System.IO.FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (System.IO.DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            System.IO.DirectoryInfo di1 = new System.IO.DirectoryInfo(pageFolder);

            foreach (System.IO.FileInfo file in di1.GetFiles())
            {
                file.Delete();
            }
            foreach (System.IO.DirectoryInfo dir in di1.GetDirectories())
            {
                dir.Delete(true);
            }

            System.IO.DirectoryInfo di2 = new System.IO.DirectoryInfo(windowsState);

            foreach (System.IO.FileInfo file in di2.GetFiles())
            {
                file.Delete();
            }
            foreach (System.IO.DirectoryInfo dir in di2.GetDirectories())
            {
                dir.Delete(true);
            }

            MessageBox.Show("清理缓存成功！");
            //重启一下程序
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
