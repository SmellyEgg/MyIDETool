using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Controller.CommonController;
using xinLongIDE.Controller.dataDic;
using xinLongIDE.Model.Page;
using xinLongIDE.Model.requestJson;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class paintBoardController
    {
        private BaseController _bcController;
        /// <summary>
        /// 错误编码
        /// -1代表常规错误
        /// -2代表网络错误
        /// </summary>
        public int errorCode = 1;

        public bool _isLocal = false;

        private List<pageDetailReturnData> _localPageInfo;

        ClassDecode clsDecode;

        public paintBoardController()
        {
            _bcController = new BaseController();
            _localPageInfo = new List<pageDetailReturnData>();
            clsDecode = new ClassDecode();
        }

        /// <summary>
        /// 获取页面细节信息
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public Task<pageDetailReturnData> GetPageDetailInfo(int pageId, string platform)
        {
            return Task.Run(() =>
            {
                if (pageId == -1)
                {
                    return null;
                }
                string filepath = GetFullPathName(pageId.ToString());
                //_isLocal = false;
                if (File.Exists(filepath))
                {
                    // _isLocal = true;
                    return ReadLocalPage(filepath);
                }
                else
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// 缓存页面信息
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public Task<int> SavePageDetail(int pageId, string platform)
        {
            return Task.Run(() =>
            {
                string filepath = GetFullPathName(pageId.ToString());
                if (File.Exists(filepath))
                {
                    return 1;
                }
                int time = 0;
                pageDetailRequest requestObj = new pageDetailRequest(pageId, time, platform);
                pageDetailReturnData pd = null;
                try
                {
                    pd = _bcController.GetPageDetail(requestObj);
                    if (!object.Equals(pd, null) && !object.Equals(pd.data, null))
                    {
                        this.CachePageInfo(pd);
                    }
                }
                catch (WebException ex)
                {
                    this.errorCode = -2;
                    Logging.Error(ex.Message);
                }

                return 1;
            });
        }

        /// <summary>
        /// 新建页面
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public Task<int> CreatePage(pageDetailReturnData pageInfo)
        {
            return Task.Run(() =>
            {
                this.CachePageInfo(pageInfo);
                return 1;
            });
        }

        /// <summary>
        /// 获取页面配置本地路径
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        private string GetFullPathName(string pageid)
        {
            string filepath = ConfigureFilePath.PageDetailFolder + "\\" + pageid + ".xml";
            return filepath;
        }

        private string GetFullPathNameForImage(string url)
        {
            if (!Directory.Exists(@ConfigureFilePath.localImageFolder))
            {
                Directory.CreateDirectory(@ConfigureFilePath.localImageFolder);
            }
            string filepath = @ConfigureFilePath.localImageFolder + "\\" + url + ".bmp";
            return filepath;
        }

        /// <summary>
        /// 读取本地缓存中的页面
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private pageDetailReturnData ReadLocalPage(string filepath)
        {
            try
            {
                pageDetailReturnData obj = xmlController.ReadFromXmlFile<pageDetailReturnData>(filepath);
                return obj;
            }
            catch (System.Exception ex)
            {
                Logging.Error("获取本地缓存页面信息出错:" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 缓存页面信息
        /// </summary>
        /// <param name="obj"></param>
        private void CachePageInfo(pageDetailReturnData obj)
        {
            string filepath = this.GetFullPathName(obj.data.page_id.ToString());
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            if (!object.Equals(obj.data.control_list, null))
            {
                GetSerizableObjectArray(obj.data.control_list);
            }
            xmlController.WriteToXmlFile<pageDetailReturnData>(filepath, obj);
        }

        private void GetSerizableObjectArray(object[][] obj)
        {
            for (int i = 0; i < obj.Length; i++)
            {
                for (int j = 0; j < obj[i].Length; j++)
                {
                    if (!object.Equals(obj[i][j], null))
                    {
                        obj[i][j] = obj[i][j].ToString();
                    }

                }
            }
        }

        /// <summary>
        /// 根据url获取网络图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Image GetImageByUrl(string url)
        {
            //这里由于暂时未加域名，所以这里拼上测试域名
            if (!url.StartsWith("http") && !url.StartsWith("/"))
            {
                return Properties.Resources.defaultImg;
            }
            if (!url.StartsWith("http"))
            {
                url = @"http://192.168.1.157" + url;
            }
            Image localImage = this.GetLocalImage(url);
            if (!object.Equals(localImage, null))
            {
                return localImage;
            }
            if (string.IsNullOrEmpty(url))
            {
                return Properties.Resources.defaultImg;
            }
            try
            {
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(url);
                using (var ms = new System.IO.MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms);
                    webClient.Dispose();
                    webClient = null;
                    imageBytes = null;
                    if (!object.Equals(image, null))
                    {
                        image.Save(this.GetFullPathNameForImage(this.GetUniqueImageUrl(url)), ImageFormat.Bmp);
                    }
                    return image;
                }
            }
            catch (Exception ex)
            {
                Logging.Error("保存本地图片出错：" + ex.Message);
                return Properties.Resources.defaultImg;
            }
        }

        private string GetUniqueImageUrl(string url)
        {
            string pattern = @"[^a-z&&^0-9]";
            return System.Text.RegularExpressions.Regex.Replace(url, pattern, string.Empty);
        }

        /// <summary>
        /// 获取本地图片
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ctrlid"></param>
        /// <returns></returns>
        public Image GetLocalImage(string url)
        {
            string filePath = this.GetFullPathNameForImage(this.GetUniqueImageUrl(url));
            if (File.Exists(filePath))
            {
                Image image = Image.FromFile(filePath);
                Bitmap bmp = new Bitmap(image);
                return bmp as Image;
                //return Image.FromFile(filePath);
                //using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                //{
                //    using (Image original = Image.FromStream(fs))
                //    {
                //        return original;
                //    }
                //}
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置控件样式
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ct"></param>
        public void SetControlProperty(Object obj, Control ct, List<Control> listControl, List<ControlDetailForPage> listControlObj)
        {
            ControlDetailForPage controlproperty = obj as ControlDetailForPage;
            ControlDetailForPage oldControlProperty = ct.Tag as ControlDetailForPage;
            //不在页面上的控件的处理
            if (listControl.FindIndex(p => (p.Tag as ControlDetailForPage).ctrl_id == controlproperty.ctrl_id) == -1)
            {
                int objIndex = listControlObj.FindIndex(p => controlproperty.ctrl_id == p.ctrl_id);
                listControlObj[objIndex] = controlproperty;
                return;
            }

            //页面控件独立处理
            if (controlproperty.ctrl_type.Equals(xinLongyuControlType.pageType))
            {
                //更新一下控件实体数组
                int Index = listControlObj.FindIndex(p => controlproperty.ctrl_id == p.ctrl_id);
                listControlObj[Index] = controlproperty;
                ct.Tag = controlproperty;
                return;
            }

            //图片控件
            if (xinLongyuControlType.imgType.Equals(controlproperty.ctrl_type) && !(ct.Tag as ControlDetailForPage).d0.Equals(controlproperty.d0))
            {
                (ct as PictureBox).Image = this.GetImageByUrl(controlproperty.d0);
            }
            //判断层级是否发生了改变
            if (!(ct.Tag as ControlDetailForPage).ctrl_level.Equals(controlproperty.ctrl_level))
            {
                SetControlLevel(ct, listControl, obj as ControlDetailForPage, listControlObj);
            }

            //判断控件ID是否改变
            if (oldControlProperty.ctrl_id != controlproperty.ctrl_id)
            {
                Control fatherCt = ct.Parent;
                ControlDetailForPage fatherCtObj = fatherCt.Tag as ControlDetailForPage;
                string controlliststr = _D0FatherControlList.Contains(fatherCtObj.ctrl_type) ? fatherCtObj.d0 : fatherCtObj.d17;
                List<int> controlList = _clsDocode.DecodeArray(controlliststr);
                controlList.Remove(oldControlProperty.ctrl_id);
                controlList.Add(controlproperty.ctrl_id);
                if (_D0FatherControlList.Contains(fatherCtObj.ctrl_type))
                {
                    fatherCtObj.d0 = ConvertarrayToString(controlList.ToArray());
                }
                else
                {
                    fatherCtObj.d17 = ConvertarrayToString(controlList.ToArray());
                }
                int ctObjIndex = listControlObj.FindIndex(p => fatherCtObj.ctrl_id.Equals(p.ctrl_id));
                listControlObj[ctObjIndex] = fatherCtObj;
                fatherCt.Tag = fatherCtObj;
            }

            ct.Tag = controlproperty;

            //更新一下控件实体数组
            int controlobjIndex = listControlObj.FindIndex(p => oldControlProperty.ctrl_id == p.ctrl_id);
            listControlObj[controlobjIndex] = controlproperty;
            //设置控件外观的显示
            ct.Width = controlproperty.d1;
            ct.Height = controlproperty.d2;
            ct.Location = new Point(controlproperty.d3, controlproperty.d4);
            ct.BackColor = System.Drawing.ColorTranslator.FromHtml(controlproperty.d8);
            //只有显性控件才进行改变
            if (toolboxController.IsVisible(controlproperty.ctrl_type))
            {
                ct.Text = controlproperty.d0;
            }

        }

        /// <summary>
        /// 设置控件层级
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="listControl"></param>
        /// <param name="childObj"></param>
        private void SetControlLevel(Control ct, List<Control> listControl, ControlDetailForPage newControlObj, List<ControlDetailForPage> listControlObj)
        {
            Control parent = ct.Parent;
            if (object.Equals(parent, null))
            {
                return;
            }
            ControlDetailForPage controlobj = parent.Tag as ControlDetailForPage;
            ControlDetailForPage oldControlObj = ct.Tag as ControlDetailForPage;

            ct.Tag = newControlObj;
            List<int> controlList = this._clsDocode.DecodeArray(controlobj.d0);
            List<ControlDetailForPage> listobj = listControlObj.Where(p => controlList.Contains(p.ctrl_id)).ToList();
            int index = listobj.FindIndex(p => newControlObj.ctrl_level.Equals(p.ctrl_level));
            if (index != -1)
            {
                int controlIndex = listControl.FindIndex(p => listobj[index].Equals(p.Tag as ControlDetailForPage));
                Control controlNeedToChange = listControl[controlIndex];
                ControlDetailForPage controlNeedToChangeObj = controlNeedToChange.Tag as ControlDetailForPage;
                controlNeedToChangeObj.ctrl_level = oldControlObj.ctrl_level;
                controlNeedToChange.Tag = controlNeedToChangeObj;
                listControl[controlIndex] = controlNeedToChange;
                listobj[index] = controlNeedToChangeObj;
            }
            List<Control> listControlinParent = new List<Control>();
            foreach (Control ctt in parent.Controls)
            {
                listControlinParent.Add(ctt);
            }
            listControlinParent = listControlinParent.OrderBy(p => (p.Tag as ControlDetailForPage).ctrl_level).ToList();
            foreach (Control ctt in listControlinParent)
            {
                ctt.BringToFront();
            }
        }

        /// <summary>
        /// 设置页面属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pageInfo"></param>
        public void SetPageProperty(Object obj, pageDetailReturnData pageInfo, List<ControlDetailForPage> list, List<Control> listControl, Form frm)
        {
            basePageProperty pageproperty = obj as basePageProperty;
            if (pageInfo.data.page_id != pageproperty.PageId || !pageInfo.data.page_name.Equals(pageproperty.PageName))
            {
                frm.Text = pageproperty.PageId.ToString() + "--" + pageproperty.PageName;
            }
            pageInfo.data.page_name = pageproperty.PageName;
            pageInfo.data.page_height = pageproperty.PageHeight;
            pageInfo.data.page_width = pageproperty.PageWidth;
            pageInfo.data.user_group = pageproperty.UserGroup;
            //是否需要更新ID，这里需要更新界面上所有控件ID的页面ID属性
            if (pageInfo.data.page_id != pageproperty.PageId)
            {
                //更新页面配置文件
                string oldFileName = this.GetFullPathName(pageInfo.data.page_id.ToString());
                string newFileName = this.GetFullPathName(pageproperty.PageId.ToString());
                File.Move(oldFileName, newFileName);//替换
                //
                pageInfo.data.page_id = pageproperty.PageId;

                foreach (ControlDetailForPage ct in list)
                {
                    int ctIndex = listControl.FindIndex(p => (p.Tag as ControlDetailForPage).ctrl_id == ct.ctrl_id);
                    if (ctIndex != -1)
                    {
                        ct.page_id = pageproperty.PageId;
                        listControl[ctIndex].Tag = ct;
                    }
                }
                
            }
        }

        /// <summary>
        /// 回调控件属性
        /// </summary>
        /// <param name="ct"></param>
        public void ChangeControlProperty(Control ct, List<ControlDetailForPage> list)
        {
            ControlDetailForPage obj = ct.Tag as ControlDetailForPage;
            if (!xinLongyuControlType.pageType.Equals(obj.ctrl_type))
            {
                int index = list.FindIndex(p => obj.ctrl_id.Equals(p.ctrl_id));
                if (index == -1)
                {
                    return;
                }
                obj.d1 = ct.Width;
                obj.d2 = ct.Height;

                obj.d3 = ct.Location.X;
                obj.d4 = ct.Location.Y;
                ct.Tag = obj;
                //list.Add(obj);
                list[index] = obj;
            }
            //else
            //{
            //    obj.d1 = ct.Width;
            //    obj.d2 = ct.Height;
            //    ct.Tag = obj;
            //}
        }

        /// <summary>
        /// 获取新建控件的实体
        /// </summary>
        /// <param name="list"></param>
        /// <param name="type"></param>
        /// <param name="pageid"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public ControlDetailForPage GetNewControlObj(List<ControlDetailForPage> list, string type, int pageid, Point pt, Boolean isvisible)
        {
            int newControlId = this.GetNewControlId(list);
            //if (list.Count < 1)
            //{
            //    newControlId = 1;
            //}
            //else
            //{
            //    int maxId = list.Max(x => x.ctrl_id);
            //    newControlId = maxId + 1;
            //}
            ControlDetailForPage obj = new ControlDetailForPage();
            obj.page_id = pageid;
            obj.ctrl_id = newControlId;
            obj.ctrl_type = type;
            //int ctMaxlevel = list.Max(p => p.ctrl_level);
            //obj.ctrl_level = ctMaxlevel + 1;

            obj.d1 = 100;
            obj.d2 = 50;
            obj.d3 = pt.X;
            obj.d4 = pt.Y;
            obj.d18 = isvisible ? "1" : "0";

            return obj;
        }

        /// <summary>
        /// 获取新的控件ID
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int GetNewControlId(List<ControlDetailForPage> list)
        {
            int newControlId = 0;
            if (list.Count < 1)
            {
                newControlId = 1;
            }
            else
            {
                int maxId = list.Where(p => !xinLongyuControlType.tabbarType.Equals(p.ctrl_type)).Max(x => x.ctrl_id);
                newControlId = maxId + 1;
            }
            return newControlId;
        }

        /// <summary>
        /// 保存缓存
        /// </summary>
        /// <param name="tempInfo"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public int SaveCache(pageDetailReturnData tempInfo, List<ControlDetailForPage> list)
        {
            if (!object.Equals(tempInfo, null))
            {
                object[][] objArray = ControlCaster.CastControlToObjectArray(list);
                tempInfo.data.control_list = objArray;

                CachePageInfo(tempInfo);
            }
            return 1;
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="tempInfo"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Task<int> Upload(pageDetailReturnData tempInfo, List<ControlDetailForPage> list)
        {
            //只处理control部分就可以了
            return Task.Run(() =>
            {
                pageSaveRequest request = new pageSaveRequest();
                if (tempInfo.data.group_id == -1)
                {
                    tempInfo.data.group_id = GetUnUploadedGroupId(tempInfo.data.page_id);
                }

                pageObjForSavePage pageObj = new pageObjForSavePage(tempInfo.data.page_id, tempInfo.data.page_name, tempInfo.data.group_id);

                pageObj.user_group = tempInfo.data.user_group;
                request.page = pageObj;
                request.ctrls = ControlCaster.CastControlToArrayForRequest(list).ToArray();
                CommonReturn objReturn = _bcController.SavePageInfo(request);
                if ("true".Equals(objReturn.data))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });
        }

        /// <summary>
        /// 获取上传后的组Id
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        private int GetUnUploadedGroupId(int pageid)
        {
            pageGroupReturnData obj = xmlController.ReadFromXmlFile<pageGroupReturnData>(ConfigureFilePath.LocalGroupInfo);
            if (object.Equals(obj, null))
            {
                return -1;
            }
            else
            {
                List<pageDetailForGroup> pagelist = new List<pageDetailForGroup>();
                foreach (pageGroupDetail group in obj.data)
                {
                    if (!object.Equals(group.page_list, null))
                    {
                        pagelist.Clear();
                        pagelist.AddRange(group.page_list);
                        if (pagelist.FindIndex(p => pageid == p.page_id) != -1)
                        {
                            return group.group_id;
                        }
                    }
                }

            }
            return -1;
        }

        /// <summary>
        /// 添加新控件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="obj"></param>
        public Control AddNewControlToPageD0(List<ControlDetailForPage> list, List<Control> listFormControl, ControlDetailForPage obj, Point originalPoint, Control pnlBoard, bool isPaste)
        {
            Control fatherControl = GetFatherControl(listFormControl, obj, list, pnlBoard);
            //AddToChildList(fatherControl.Tag as ControlDetailForPage, obj, list, listFormControl, fatherControl);
            //return fatherControl;
            ControlDetailForPage page;
            if (object.Equals(fatherControl, null))
            {
                page = list.Where(p => xinLongyuControlType.pageType.Equals(p.ctrl_type)).First();
            }
            else
            {
                page = fatherControl.Tag as ControlDetailForPage;
            }

            ClassDecode clsDecode = new ClassDecode();
            List<int> pageControlStrList = new List<int>();
            string controllist = _D0FatherControlList.Contains(page.ctrl_type) ? page.d0 : page.d17;
            if (!string.IsNullOrEmpty(controllist))
            {
                pageControlStrList = clsDecode.DecodeArray(controllist);
            }
            pageControlStrList.Add(obj.ctrl_id);
            if (_D0FatherControlList.Contains(page.ctrl_type))
            {
                page.d0 = ConvertarrayToString(pageControlStrList.ToArray());
            }
            else
            {
                page.d17 = ConvertarrayToString(pageControlStrList.ToArray());
            }
            int pageIndex = list.FindIndex(p => p.ctrl_id.Equals(page.ctrl_id));
            list[pageIndex] = page;
            if (!object.Equals(fatherControl, null))
            {
                if (!isPaste)
                {
                    Point newPt = (fatherControl as Panel).PointToClient(new Point(originalPoint.X, originalPoint.Y));
                    obj.d3 = newPt.X;
                    obj.d4 = newPt.Y;
                }
                list.Add(obj);
                fatherControl.Tag = page;
                int fatherControlIndex = listFormControl.FindIndex(p => p.Name.Equals(fatherControl.Name));
                listFormControl[fatherControlIndex] = fatherControl;
                return fatherControl;
            }
            else
            {
                list.Add(obj);
                return null;
            }
        }

        private void AddToChildList(ControlDetailForPage obj, List<ControlDetailForPage> list, List<Control> listFormControl,Control fatherControl)
        {
            ControlDetailForPage page = fatherControl.Tag as ControlDetailForPage;
            List<int> pageControlStrList = new List<int>();
            string controllist = _D0FatherControlList.Contains(page.ctrl_type) ? page.d0 : page.d17;
            if (!string.IsNullOrEmpty(controllist))
            {
                pageControlStrList = _clsDocode.DecodeArray(controllist);
            }
            pageControlStrList.Add(obj.ctrl_id);
            if (_D0FatherControlList.Contains(page.ctrl_type))
            {
                page.d0 = ConvertarrayToString(pageControlStrList.ToArray());
            }
            else
            {
                page.d17 = ConvertarrayToString(pageControlStrList.ToArray());
            }
            int pageIndex = list.FindIndex(p => p.ctrl_id.Equals(page.ctrl_id));
            list[pageIndex] = page;

            //list.Add(obj);
            fatherControl.Tag = page;
            int fatherControlIndex = listFormControl.FindIndex(p => p.Name.Equals(fatherControl.Name));
            listFormControl[fatherControlIndex] = fatherControl;
        }

        /// <summary>
        /// 处理黏贴的控件
        /// </summary>
        /// <param name="listAll"></param>
        /// <param name="listNew"></param>
        /// <param name="objParent"></param>
        public void DealWithPastedControlList(List<ControlDetailForPage> listAll, List<ControlDetailForPage> listNew, ControlDetailForPage objParent)
        {
            ControlDetailForPage page = listAll.First(p => xinLongyuControlType.pageType.Equals(p.ctrl_type));
            List<int> allControlList = new List<int>();
            if (!string.IsNullOrEmpty(page.d0))
            {
                allControlList = clsDecode.DecodeArray(page.d0);
            }
            allControlList.Add(objParent.ctrl_id);
            page.d0 = ConvertarrayToString(allControlList.ToArray());
            List<int> controlIdList = clsDecode.DecodeArray(objParent.d0);
            foreach (ControlDetailForPage obj in listNew)
            {
                if (listAll.FindIndex(p => obj.ctrl_id.Equals(p.ctrl_id)) != -1)
                {
                    int newId = this.GetNewControlId(listAll);

                    int idIndex = controlIdList.FindIndex(p => obj.ctrl_id == p);
                    controlIdList[idIndex] = newId;
                    obj.ctrl_id = newId;
                }
                obj.page_id = objParent.page_id;
                listAll.Add(obj);
            }
            objParent.d0 = ConvertarrayToString(controlIdList.ToArray());
            listAll.Add(objParent);
        }

        private ClassDecode _clsDocode = new ClassDecode();

        /// <summary>
        /// 父控件集合
        /// </summary>
        public List<string> _fatherControlList = new List<string>() { xinLongyuControlType.navigationBarType, xinLongyuControlType.superViewType
        , xinLongyuControlType.listsType, xinLongyuControlType.bannerType, xinLongyuControlType.cellType, xinLongyuControlType.channerBarType,
        xinLongyuControlType.sectionType, xinLongyuControlType.radioType, xinLongyuControlType.checkboxType, xinLongyuControlType.multilineListType,
            xinLongyuControlType.dynamicListsType, xinLongyuControlType.PCGrid};

        /// <summary>
        /// D0父控件集合
        /// </summary>
        private List<string> _D0FatherControlList = new List<string>() { xinLongyuControlType.pageType
                ,xinLongyuControlType.superViewType
                ,xinLongyuControlType.navigationBarType};

        private Control GetFatherControl(List<Control> list, ControlDetailForPage obj, List<ControlDetailForPage> listControlObj, Control pnlBoard)
        {
            List<Control> newlist = list.Where(p => _fatherControlList.Contains((p.Tag as ControlDetailForPage).ctrl_type)).ToList();

            List<Control> listFather = new List<Control>();
            foreach (Control ct in newlist)
            {
                if (IsFatherControl(obj, ct, pnlBoard))
                {
                    listFather.Add(ct);
                }
            }
            if (listFather.Count < 1)
            {
                return null;
            }
            Control target = null;
            bool bl = true;
            foreach (Control ct in listFather)
            {
                bl = true;
                ControlDetailForPage ctTag = ct.Tag as ControlDetailForPage;
                string controllist = _D0FatherControlList.Contains(ctTag.ctrl_type) ? ctTag.d0 : ctTag.d17;
                List<int> controlIdList = clsDecode.DecodeArray(controllist);
                if (object.Equals(controlIdList, null) || controlIdList.Count < 1)
                {
                    target = ct;
                    break;
                }
                foreach (Control ctSecond in listFather)
                {
                    if (controlIdList.Contains((ctSecond.Tag as ControlDetailForPage).ctrl_id))
                    {
                        bl = false;
                        break;
                    }
                }
                if (bl)
                {
                    target = ct;
                    break;
                }
            }
            return target;
        }



        /// <summary>
        /// 判断是否有父控件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fatherObj"></param>
        /// <returns></returns>
        private bool IsFatherControl(ControlDetailForPage obj, Control fatherObj, Control pnlPaintboard)
        {
            Point pt = new Point(obj.d3, obj.d4);
            Point pt1 = fatherObj.PointToScreen(Point.Empty);
            Point pt2 = pnlPaintboard.PointToScreen(Point.Empty);
            Point fatherPt = new Point(pt1.X - pt2.X, pt1.Y - pt2.Y);
            if (pt.X > fatherPt.X && pt.Y > fatherPt.Y && pt.X < (fatherPt.X + fatherObj.Width) && pt.Y < (fatherPt.Y + fatherObj.Height)/* && (obj.d3 + obj.d1) < (fatherObj.Location.X + fatherObj.Width) && (obj.d4 + obj.d2) < (fatherObj.Location.Y + fatherObj.Height)*/)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsCrossControl(ControlDetailForPage obj, Control fatherObj)
        {
            if ((obj.d3 > fatherObj.Location.X && obj.d4 > fatherObj.Location.Y && obj.d3 < (fatherObj.Location.X + fatherObj.Width) && obj.d4 < (fatherObj.Location.Y + fatherObj.Height)) ||
                (fatherObj.Location.X > obj.d3 && fatherObj.Location.Y > obj.d4 && fatherObj.Location.X < (obj.d3 + obj.d1) && fatherObj.Location.Y < (obj.d4 + obj.d2))/* && (obj.d3 + obj.d1) < (fatherObj.Location.X + fatherObj.Width) && (obj.d4 + obj.d2) < (fatherObj.Location.Y + fatherObj.Height)*/)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除子控件
        /// </summary>
        /// <param name="fatherControl"></param>
        private void RemoveControl(System.Windows.Forms.Control fatherControl, List<ControlDetailForPage> list, List<Control> listControl)
        {
            if (fatherControl.Controls.Count > 0)
            {
                foreach (System.Windows.Forms.Control ct in fatherControl.Controls)
                {
                    RemoveControl(ct, list, listControl);
                }
                Control fatherPanel = fatherControl.Parent;
                fatherPanel.Tag = RemoveControlObj(list, fatherPanel.Tag as ControlDetailForPage, fatherControl.Tag as ControlDetailForPage);
                fatherPanel.Controls.Remove(fatherControl);
                listControl.Remove(fatherControl);
            }
            else
            {
                Control fatherPanel = fatherControl.Parent;
                fatherPanel.Tag = RemoveControlObj(list, fatherPanel.Tag as ControlDetailForPage, fatherControl.Tag as ControlDetailForPage);
                fatherPanel.Controls.Remove(fatherControl);
                listControl.Remove(fatherControl);
            }
        }

        private ControlDetailForPage RemoveControlObj(List<ControlDetailForPage> list, ControlDetailForPage fatherObj, ControlDetailForPage childObj)
        {
            //如果是隐性控件的话默认使用page
            if (!xinLongyuConverter.StringToBool(childObj.d18))
            {
                fatherObj = list.First(p => xinLongyuControlType.pageType.Equals(p.ctrl_type));
            }
            list.Remove(childObj);
            List<int> pageControlStrList = new List<int>();
            string controllist = xinLongyuControlType.pageType.Equals(fatherObj.ctrl_type)
                || xinLongyuControlType.superViewType.Equals(fatherObj.ctrl_type)
                || xinLongyuControlType.navigationBarType.Equals(fatherObj.ctrl_type) ? fatherObj.d0 : fatherObj.d17;
            if (!string.IsNullOrEmpty(controllist))
            {
                pageControlStrList = clsDecode.DecodeArray(controllist);
            }
            else
            {
                return null;
            }
            pageControlStrList.Remove(childObj.ctrl_id);
            if (xinLongyuControlType.pageType.Equals(fatherObj.ctrl_type)
                 || xinLongyuControlType.superViewType.Equals(fatherObj.ctrl_type)
                || xinLongyuControlType.navigationBarType.Equals(fatherObj.ctrl_type))
            {
                fatherObj.d0 = ConvertarrayToString(pageControlStrList.ToArray());
            }
            else
            {
                fatherObj.d17 = ConvertarrayToString(pageControlStrList.ToArray());
            }
            int pageIndex = list.FindIndex(p => p.ctrl_id.Equals(fatherObj.ctrl_id));
            list[pageIndex] = fatherObj;
            return fatherObj;
        }

        /// <summary>
        /// 删除控件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="obj"></param>
        public void DeleteControlFromPageD0(List<ControlDetailForPage> list, List<Control> listControl, Control currentControl)
        {
            RemoveControl(currentControl, list, listControl);
        }

        #region 添加一些预设的控件
        /// <summary>
        /// 添加预设控件
        /// </summary>
        public pageDetailReturnData AddPreControlsForNewPage(int pageId, string pageName, int groupId, string userGroup)
        {
            List<ControlDetailForPage> list = new List<ControlDetailForPage>();
            //添加page
            ControlDetailForPage objPage = new ControlDetailForPage();
            objPage.d0 = "";
            objPage.ctrl_id = 0;
            objPage.ctrl_level = 1;
            objPage.ctrl_type = "page";
            objPage.d1 = 100;
            objPage.d2 = 500;
            objPage.d3 = 0;
            objPage.d4 = 0;
            objPage.d6 = "12";


            //添加navigationBar
            ControlDetailForPage objNavigationBar = new ControlDetailForPage();
            objNavigationBar.ctrl_id = 1;
            objNavigationBar.ctrl_level = 2;
            objNavigationBar.ctrl_type = "navigationBar";
            objNavigationBar.d0 = "[50, 51]";
            objNavigationBar.d1 = 375;
            objNavigationBar.d2 = 44;
            objNavigationBar.d3 = 0;
            objNavigationBar.d4 = 0;
            objNavigationBar.d6 = "16";
            objNavigationBar.d8 = "#ffffff";
            objNavigationBar.d12 = "0";
            objNavigationBar.d14 = "#000000";
            objNavigationBar.d15 = "1";
            objNavigationBar.d18 = "1";
            objNavigationBar.d19 = "1";
            objNavigationBar.d26 = "0";
            objNavigationBar.d36 = "15";
            //文本
            ControlDetailForPage objtext = new ControlDetailForPage();
            objtext.ctrl_id = 50;
            objtext.ctrl_level = 10;
            objtext.ctrl_type = "text";
            objtext.d0 = "测试页";
            objtext.d1 = 40;
            objtext.d2 = 35;
            objtext.d3 = 0;
            objtext.d4 = 0;
            objtext.d6 = "18";
            objtext.d7 = "#22798A";
            objtext.d8 = "#ffffff";
            objtext.d18 = "1";
            objtext.d19 = "1";
            objtext.d26 = "0";
            objtext.d36 = "15";
            //图片
            ControlDetailForPage objimg = new ControlDetailForPage();
            objimg.ctrl_id = 51;
            objimg.ctrl_level = 2;
            objimg.ctrl_type = "img";
            objimg.d0 = "http://img.sootuu.com/vector/2007-07-01/068/3/200.gif";
            objimg.d1 = 44;
            objimg.d2 = 44;
            objimg.d3 = 0;
            objimg.d4 = 0;
            objimg.d5 = "1";
            objimg.d6 = "11";
            objimg.d7 = "#000000";
            objimg.d11 = "#ffffff";
            objimg.d18 = "1";
            objimg.d19 = "1";
            objimg.d26 = "0";
            objimg.d36 = "15";
            //
            list.Add(objPage);
            list.Add(objNavigationBar);
            list.Add(objtext);
            list.Add(objimg);
            //
            foreach (ControlDetailForPage ct in list)
            {
                ct.page_id = pageId;
            }
            dataForPageDetail dfp = new dataForPageDetail();
            dfp.page_id = pageId;
            dfp.page_name = pageName;
            dfp.group_id = groupId;
            dfp.user_group = userGroup;

            dfp.control_list = ControlCaster.CastControlToObjectArray(list);
            pageDetailReturnData pdr = new pageDetailReturnData();
            pdr.data = dfp;
            return pdr;
        }

        /// <summary>
        /// 为pc端增加预设控件
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="pageName"></param>
        /// <param name="groupId"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public pageDetailReturnData AddPreControlsForNewPageForPC(int pageId, string pageName, int groupId, string userGroup)
        {
            List<ControlDetailForPage> list = new List<ControlDetailForPage>();
            //添加page
            ControlDetailForPage objPage = new ControlDetailForPage();
            objPage.ctrl_id = 0;
            objPage.ctrl_level = 1;
            objPage.ctrl_type = "page";
            objPage.d1 = 1366;
            objPage.d2 = 768;
            objPage.d3 = 100;
            objPage.d4 = 200;
            objPage.d6 = "12";
            objPage.page_id = pageId;
            list.Add(objPage);
            //
            dataForPageDetail dfp = new dataForPageDetail();
            dfp.page_id = pageId;
            dfp.page_name = pageName;
            dfp.group_id = groupId;
            dfp.user_group = userGroup;

            dfp.control_list = ControlCaster.CastControlToObjectArray(list);
            pageDetailReturnData pdr = new pageDetailReturnData();
            pdr.data = dfp;
            return pdr;
        }
        #endregion

        /// <summary>
        /// 将数组转换成数据字符串
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private string ConvertarrayToString(int[] array)
        {
            StringBuilder str = new StringBuilder(500);
            str.Append("[");
            for (int i = 0; i < array.Length; i++)
            {
                if (i == 0)
                {
                    str.Append(array[i]);
                }
                else
                {
                    str.Append("," + array[i]);
                }
            }
            str.Append("]");
            return str.ToString();
        }

        /// <summary>
        /// 用于拷贝黏贴的数据标识
        /// </summary>
        private string MyControlCopyFormat = "MyControl";
        private string MyChildControlList = "MyChildControls";

        /// <summary>
        /// 拷贝控件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="listControlObj"></param>
        public void CopyControl(Control control, List<ControlDetailForPage> listControlObj)
        {
            Clipboard.Clear();
            ControlDetailForPage obj = control.Tag as ControlDetailForPage;
            List<ControlDetailForPage> listTarget = new List<ControlDetailForPage>();
            this.GetAllChildControlList(obj, listTarget, listControlObj);
            listTarget.Add(obj);
            if (listTarget.Count == 1)
            {
                Clipboard.SetData(MyControlCopyFormat, obj);
            }
            else
            {
                Clipboard.SetData(MyChildControlList, listTarget);
            }
            
        }

        /// <summary>
        /// 获取所有的子控件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="listControl"></param>
        /// <param name="listAll"></param>
        private void GetAllChildControlList(ControlDetailForPage control, List<ControlDetailForPage> listControl, List<ControlDetailForPage> listAll)
        {
            //这里只需要递归获取到所有的控件就可以了，粘贴那一步才需要修改控件ID以及修改对应的子控件列表字段
            if (_fatherControlList.Contains(control.ctrl_type))
            {
                string controlList = _D0FatherControlList.Contains(control.ctrl_type) ? control.d0 : control.d17;
                if (!string.IsNullOrEmpty(controlList))
                {
                    List<int> listControlId = _clsDocode.DecodeArray(controlList);
                    List<ControlDetailForPage> listChild = listAll.Where(p => listControlId.Contains(p.ctrl_id)).ToList();
                    listControl.AddRange(listChild);
                    foreach (ControlDetailForPage obj in listChild)
                    {
                        GetAllChildControlList(obj, listControl, listAll);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 黏贴控件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="listControlObj"></param>
        /// <param name="listControl"></param>
        public ControlDetailForPage PasteControl(Point cursorPoint, Control control,  List<ControlDetailForPage> listControlObj, List<Control> listControl)
        {
            ControlDetailForPage obj = null;
            if (Clipboard.ContainsData(MyControlCopyFormat))
            {
                obj = Clipboard.GetData(MyControlCopyFormat) as ControlDetailForPage;
                obj.ctrl_id = this.GetNewControlId(listControlObj);
                obj.page_id = listControlObj[0].page_id;
                listControlObj.Add(obj);
            }
            else if (Clipboard.ContainsData(MyChildControlList))
            {
                List<ControlDetailForPage> objlist = Clipboard.GetData(MyChildControlList) as List<ControlDetailForPage>;
                obj = objlist[objlist.Count - 1];
                SetNewCtrlIdForList(obj, objlist, listControlObj);
            }

            if (!object.Equals(obj, null))
            {
                //重新给父控件计算一下坐标
                Point newPoint = control.PointToClient(cursorPoint);
                obj.d3 = newPoint.X;
                obj.d4 = newPoint.Y;
                //添加到父控件子控件列表字段中
                this.AddToChildList(obj, listControlObj, listControl, control);
                return obj;
            }
            else
            {
                return null;
            }
        }

        private void SetNewCtrlIdForList(ControlDetailForPage controlObj, List<ControlDetailForPage> list, List<ControlDetailForPage> listAll)
        {
            controlObj.ctrl_id = this.GetNewControlId(listAll);
            controlObj.page_id = listAll[0].page_id;
            listAll.Add(controlObj);
            if (_fatherControlList.Contains(controlObj.ctrl_type))
            {
                string controllist = _D0FatherControlList.Contains(controlObj.ctrl_type) ? controlObj.d0 : controlObj.d17;
                if (!string.IsNullOrEmpty(controllist))
                {
                    List<int> controlIdList = _clsDocode.DecodeArray(controllist);
                    List<ControlDetailForPage> childControlList = list.Where(p => controlIdList.Contains(p.ctrl_id)).ToList();
                    controlIdList.Clear();
                    foreach (ControlDetailForPage grandChild in childControlList)
                    {
                        SetNewCtrlIdForList(grandChild, list, listAll);
                        controlIdList.Add(grandChild.ctrl_id);
                    }
                    //修改子控件列表字段
                    if (_D0FatherControlList.Contains(controlObj.ctrl_type))
                    {
                        controlObj.d0 = ConvertarrayToString(controlIdList.ToArray());
                    }
                    else
                    {
                        controlObj.d17 = ConvertarrayToString(controlIdList.ToArray());
                    }
                }
            }
        }

    }
}
