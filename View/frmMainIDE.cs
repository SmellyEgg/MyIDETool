using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Controller.dataDic;
using xinLongIDE.Properties;

namespace xinLongIDE.View
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
        frmPaintBoard _formPaintBoard;

        private void Init()
        {
            ShowPageToolbox();
            ShowPageControlProperty();
            ShowPageManager();
            ShowPagePaintBoard();
        }

        private void ShowPageToolbox()
        {
            _formToolBox = new frmToolBox();
            _formToolBox.MdiParent = this;
            _formToolBox.Show();
        }

        private void ShowPageControlProperty()
        {
            _formControlProperty = new frmControlProperty();
            _formControlProperty.MdiParent = this;
            _formControlProperty.Show();
        }

        /// <summary>
        /// 页面管理界面
        /// </summary>
        private void ShowPageManager()
        {
            _formPageManager = new frmPageManager();
            _formPageManager.nodeSelected += new View.frmPageManager.delegateForSelectedNode(ShowPageInfo);
            string platForm = "app";
            _formPageManager.SetPages(platForm);
            _formPageManager.MdiParent = this;
            _formPageManager.Show();
        }

        private void ShowPagePaintBoard()
        {
            _formPaintBoard = new frmPaintBoard();
            _formPaintBoard.MdiParent = this;
            _formPaintBoard.Show();
        }

        private void ShowPageInfo(object obj)
        {
            _formPaintBoard.ShowPageDetail(obj);
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
        private Controller.Settings.windowsStatusController _formStatusController = new Controller.Settings.windowsStatusController();

        private void frmMainIDE_Load(object sender, EventArgs e)
        {
            List<Model.Setting.FormStatusSettings> _lstFormStatus = new List<Model.Setting.FormStatusSettings>();
            _lstFormStatus = _formStatusController.GetFormSettings();
            if (!object.Equals(_lstFormStatus, null))
            {
                SetFormStatus(this, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.Z主窗体]);
                SetFormStatus(_formControlProperty, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.S属性窗体]);
                SetFormStatus(_formPageManager, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.Y页面管理窗体]);
                SetFormStatus(_formPaintBoard, _lstFormStatus[(int)windowsStatusEnum.enumOfIndexOfWindowsConfig.H画布窗体]);
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
            _lstFormStatus.Add(GetFormStatusSettingsEntity(_formPaintBoard));
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

    }
}
