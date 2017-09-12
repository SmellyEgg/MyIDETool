using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using xinLongIDE.Controller.dataDic;
using xinLongIDE.Controller.xinlongyuEnum;
using xinLongIDE.Model.Page;
using xinLongIDE.Model.returnJson;
using xinLongIDE.Model.Setting;
using xinLongIDE.View.MainForm;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class paintBoardManager
    {
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


        private List<frmPaintBoard> _listOfPaintBoards;

        private List<nodeObjectTransfer> _listOfOpenedPages;

        private paintBoardController _paintboardController;
        /// <summary>
        /// 当前激活窗体
        /// </summary>
        private frmPaintBoard _currentForm = null;
        public paintBoardManager(Form frmParent)
        {
            _listOfPaintBoards = new List<frmPaintBoard>();
            _listOfOpenedPages = new List<nodeObjectTransfer>();
            _paintboardController = new paintBoardController();
            //打开上次关闭时打开了的页面
            //ResumePreviousPage(frmParent);
        }

        /// <summary>
        /// 恢复上次打开的界面
        /// </summary>
        public void ResumePreviousPage(Form frmParent, string platForm)
        {
            string filePath = GetOpendPages(platForm);
            if (File.Exists(filePath))
            {
                List<nodeObjectTransfer> listpages = xmlController.ReadFromXmlFile<List<nodeObjectTransfer>>(filePath);
                if (!object.Equals(listpages, null) && listpages.Count > 0)
                {
                    foreach (nodeObjectTransfer obj in listpages)
                    {
                        this.AddPaintBoard(obj, frmParent);
                    }
                }
            }
            //恢复窗体状态
            string filePathState = GetOpenedPageState(platForm);
            if (File.Exists(filePath))
            {
                List<FormStatusSettings> listStatus = xmlController.ReadFromXmlFile<List<FormStatusSettings>>(filePathState);
                if (!object.Equals(listStatus, null) && listStatus.Count > 0)
                {
                    int index = 0;
                    foreach (FormStatusSettings obj in listStatus)
                    {
                        SetFormStatus(_listOfPaintBoards[index], obj);
                        index++;
                    }
                }
            }
        }

        private string GetOpendPages(string platForm)
        {
            if (platFormEnum.app.ToString().Equals(platForm))
            {
                return ConfigureFilePath.openedPages;
            }
            else
            {
                return ConfigureFilePath.openedPCPages;
            }
        }

        private string GetOpenedPageState(string platForm)
        {
            if (platFormEnum.app.ToString().Equals(platForm))
            {
                return ConfigureFilePath.openedPagesState;
            }
            else
            {
                return ConfigureFilePath.openedPCPagesState;
            }
        }

        private void SetFormStatus(Form frm, Model.Setting.FormStatusSettings settings)
        {
            Point newPoint = settings.pointConfig;
            Size newSize = settings.SizeConfig;
            frm.Size = newSize;
            frm.Location = newPoint;
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
            if (!object.Equals(obj, null) && !object.Equals(_currentForm, null))
            {
                if (!object.Equals(_currentForm.CurrentControl, null))
                {
                    ControlDetailForPage oldObj = _currentForm.CurrentControl.Tag as ControlDetailForPage;
                    _currentForm.SetControlProperty(obj);
                }
                
            }
        }

        public void AddPaintBoard(object obj, Form frmParent)
        {
            nodeObjectTransfer pageObj = obj as nodeObjectTransfer;
            //防止打开重复的界面
            int pageindex = _listOfOpenedPages.FindIndex(p => pageObj.PageId.Equals(p.PageId));
            if (pageindex != -1)
            {
                _listOfPaintBoards[pageindex].Activate();
                return;
            }
            frmPaintBoard frm = new frmPaintBoard();
            frm.MdiParent = frmParent;
            frm.Tag = pageObj.PageId;
            frm.ShowPageDetail(pageObj);
            frm.progressChange += ChangeProgressValue;
            frm.controlPropertyChange += Frm_controlPropertyChange;
            frm.pagePropertyChanged += Frm_pagePropertyChanged;
            frm.groupUpload += Frm_groupUpload;
            frm.Activated += Frm_Activated;
            frm.FormClosed += Frm_FormClosed;

            string tabName = pageObj.PageId.ToString() + "--" + pageObj.PageName;
            frm.Text = tabName;
            frm.BringToFront();
            frm.Show();
            _listOfPaintBoards.Add(frm);
            _listOfOpenedPages.Add(pageObj);
        }

        /// <summary>
        /// 上传组信息
        /// </summary>
        private void Frm_groupUpload()
        {
            //throw new System.NotImplementedException();
            groupUpload.Invoke();
        }

        private void Frm_pagePropertyChanged(object objOld, object objNew)
        {
            pagePropertyChanged.Invoke(objOld, objNew);
            //修改页面状态存储文件
            int pageIndex = _listOfOpenedPages.FindIndex(p => (objOld as basePageProperty).PageId == p.PageId);
            _listOfOpenedPages[pageIndex].PageId = (objNew as basePageProperty).PageId;
            _listOfOpenedPages[pageIndex].PageName = (objNew as basePageProperty).PageName;
        }

        private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmPaintBoard frm = sender as frmPaintBoard;
            int index = _listOfPaintBoards.IndexOf(frm);
            _listOfPaintBoards.RemoveAt(index);
            _listOfOpenedPages.RemoveAt(index);
        }

        private void Frm_Activated(object sender, System.EventArgs e)
        {
            _currentForm = sender as frmPaintBoard;
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
            await _paintboardController.SavePageDetail(pageid, pageObj.Plat_form);
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
            if (platFormEnum.app.ToString().Equals(pageObj.Plat_form))
            {
                pageInfo = _paintboardController.AddPreControlsForNewPage(pageObj.PageId, pageObj.PageName, pageObj.GroupId, pageObj.UserGroup);
            }
            else
            {
                pageInfo = _paintboardController.AddPreControlsForNewPageForPC(pageObj.PageId, pageObj.PageName, pageObj.GroupId, pageObj.UserGroup);
            }
            
            await _paintboardController.CreatePage(pageInfo);
        }

        /// <summary>
        /// 保存打开过的窗体
        /// </summary>
        public void SaveState(string platForm)
        {
            string fileOpenedPages = this.GetOpendPages(platForm);
            //保存已经打开了的界面
            if (File.Exists(fileOpenedPages))
            {
                File.Delete(fileOpenedPages);
            }
            string fileOpenedPagesState = this.GetOpenedPageState(platForm); 
            if (File.Exists(fileOpenedPagesState))
            {
                File.Delete(fileOpenedPagesState);
            }
            List<Model.Setting.FormStatusSettings> _lstFormStatus = new List<Model.Setting.FormStatusSettings>();
            foreach (frmPaintBoard frm in _listOfPaintBoards)
            {
                _lstFormStatus.Add(GetFormStatusSettingsEntity(frm));
            }
            if (_listOfOpenedPages.Count > 0)
            {
                xmlController.WriteToXmlFile<List<nodeObjectTransfer>>(fileOpenedPages, _listOfOpenedPages);
                xmlController.WriteToXmlFile<List<Model.Setting.FormStatusSettings>>(fileOpenedPagesState, _lstFormStatus);
            }
        }

        private Model.Setting.FormStatusSettings GetFormStatusSettingsEntity(Form frm)
        {
            Model.Setting.FormStatusSettings setting = new Model.Setting.FormStatusSettings();
            setting.SizeConfig = frm.Size;
            setting.pointConfig = new Point(frm.Location.X, frm.Location.Y);
            return setting;
        }

        public void Clear()
        {
            List<frmPaintBoard> tempList = new List<frmPaintBoard>();
            tempList = _listOfPaintBoards.Where(p => 1==1).ToList();
            foreach(frmPaintBoard frm in tempList)
            {
                frm.Close();
            }
        }
    }
}
