using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Controller.CommonController;
using xinLongIDE.Controller.dataDic;
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

        public paintBoardController()
        {
            _bcController = new BaseController();
            _localPageInfo = new List<pageDetailReturnData>();
        }

        /// <summary>
        /// 获取页面细节信息
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public Task<pageDetailReturnData> GetPageDetailInfo(int pageId)
        {
            return Task.Run(() =>
            {
                if (pageId == -1)
                {
                    return null;
                }
                string filepath = GetFullPathName(pageId.ToString());
                if (File.Exists(filepath))
                {
                    _isLocal = true;
                    return ReadLocalPage(filepath);
                }
                else
                {
                    _isLocal = false;
                    //暂时不知道这个时间参数有什么用有什么用
                    int time = 0;
                    pageDetailRequest requestObj = new pageDetailRequest(System.Convert.ToInt32(pageId), time);
                    pageDetailReturnData pd = null;
                    try
                    {
                        pd = _bcController.GetPageDetail(requestObj);
                    }
                    catch (WebException e)
                    {
                        Logging.Error(e.Message);
                        this.errorCode = -2;
                    }
                    //CachePageInfo(pd);
                    return pd;
                }
            });
        }

        /// <summary>
        /// 缓存页面信息
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public Task<int> SavePageDetail(int pageId)
        {
            return Task.Run(() =>
            {
                string filepath = GetFullPathName(pageId.ToString());
                if (File.Exists(filepath))
                {
                    return 1;
                }
                int time = 0;
                pageDetailRequest requestObj = new pageDetailRequest(pageId, time);
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

        private string GetFullPathNameForImage(string pageid, string ctrlid)
        {
            string filepath = @ConfigureFilePath.localImageFolder + "\\" + pageid + "_" + ctrlid + ".bmp";
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
        public Image GetImageByUrl(string url, string page_id, string ctrl_id)
        {
            Image localImage = this.GetLocalImage(page_id, ctrl_id);
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
                        image.Save(this.GetFullPathNameForImage(page_id, ctrl_id));
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
        /// <summary>
        /// 获取本地图片
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ctrlid"></param>
        /// <returns></returns>
        public Image GetLocalImage(string pageid, string ctrlid)
        {
            string filePath = this.GetFullPathNameForImage(pageid, ctrlid);
            if (File.Exists(filePath))
            {
                return Image.FromFile(filePath);
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
        public void SetControlProperty(Object obj, Control ct)
        {
            ControlDetailForPage controlproperty = obj as ControlDetailForPage;
            ct.Width = controlproperty.d1;
            ct.Height = controlproperty.d2;
            ct.Location = new Point(controlproperty.d3, controlproperty.d4);
            ct.Text = controlproperty.d0;
            ct.Tag = controlproperty;
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
            else
            {
                obj.d1 = ct.Width;
                obj.d2 = ct.Height;
                ct.Tag = obj;
            }
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
            int newControlId = 0;
            if (list.Count < 1)
            {
                newControlId = 1;
            }
            else
            {
                int maxId = list.Max(x => x.ctrl_id);
                newControlId = maxId + 1;
            }
            ControlDetailForPage obj = new ControlDetailForPage();
            obj.page_id = pageid;
            obj.ctrl_id = newControlId;
            obj.ctrl_type = type;
            obj.ctrl_level = 0;

            obj.d1 = 100;
            obj.d2 = 50;
            obj.d3 = pt.X;
            obj.d4 = pt.Y;
            obj.d18 = isvisible ? "1" : "0";

            return obj;
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
        public int Upload(pageDetailReturnData tempInfo, List<ControlDetailForPage> list)
        {
            //只处理control部分就可以了
            pageSaveRequest request = new pageSaveRequest();
            if (tempInfo.data.group_id == -1)
            {
                tempInfo.data.group_id = GetUnUploadedGroupId(tempInfo.data.page_id);
            }
            pageObjForSavePage pageObj = new pageObjForSavePage(tempInfo.data.page_id, tempInfo.data.page_name, tempInfo.data.group_id);
            //pageObj.user_group = "admin";
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
        public Control AddNewControlToPageD0(List<ControlDetailForPage> list, List<Control> listFormControl, ControlDetailForPage obj, Point originalPoint)
        {
            //list.Add(obj);
            Control fatherControl = GetFatherControl(listFormControl, obj);
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
            if (!string.IsNullOrEmpty(page.d0))
            {
                pageControlStrList = clsDecode.DecodeArray(page.d0);
            }
            pageControlStrList.Add(obj.ctrl_id);
            page.d0 = ConvertarrayToString(pageControlStrList.ToArray());
            int pageIndex = list.FindIndex(p => p.ctrl_id.Equals(page.ctrl_id));
            list[pageIndex] = page;
            if (!object.Equals(fatherControl, null))
            {
                Point newPt = (fatherControl as Panel).PointToClient(new Point(originalPoint.X, originalPoint.Y));
                obj.d3 = newPt.X;
                obj.d4 = newPt.Y;
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

        private Control GetFatherControl(List<Control> list, ControlDetailForPage obj)
        {
            foreach(Control ct in list)
            {
                if (IsFatherControl(obj, ct))
                {
                    return ct;
                }
            }
            return null;
        }

        /// <summary>
        /// 判断是否有父控件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fatherObj"></param>
        /// <returns></returns>
        private bool IsFatherControl(ControlDetailForPage obj, Control fatherObj)
        {
            if (obj.d3 > fatherObj.Location.X && obj.d4 > fatherObj.Location.Y && (obj.d3 + obj.d1) < (fatherObj.Location.X + fatherObj.Width) && (obj.d4 + obj.d2) < (fatherObj.Location.Y + fatherObj.Height))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除控件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="obj"></param>
        public void DeleteControlFromPageD0(List<ControlDetailForPage> list, ControlDetailForPage obj)
        {
            ClassDecode clsDecode = new ClassDecode();
            list.Remove(obj);
            ControlDetailForPage page = list.Where(p => !string.IsNullOrEmpty(p.d0) 
            && (xinLongyuControlType.pageType.Equals(p.ctrl_type) || xinLongyuControlType.superViewType.Equals(p.ctrl_type))
            && clsDecode.DecodeArray(p.d0).Contains(obj.ctrl_id)).First();
            //ControlDetailForPage page = list.Where(p => xinLongyuControlType.pageType.Equals(p.ctrl_type)).ToList()[0];
            
            List<int> pageControlStrList = new List<int>();
            if (!string.IsNullOrEmpty(page.d0))
            {
                pageControlStrList = clsDecode.DecodeArray(page.d0);
            }
            pageControlStrList.Remove(obj.ctrl_id);
            page.d0 = ConvertarrayToString(pageControlStrList.ToArray());
            int pageIndex = list.FindIndex(p => p.ctrl_id.Equals(page.ctrl_id));
            list[pageIndex] = page;
        }

        #region 添加一些预设的控件
        /// <summary>
        /// 添加预设控件
        /// </summary>
        public pageDetailReturnData AddPreControlsForNewPage(int pageId, string pageName, int groupId)
        {
            List<ControlDetailForPage> list = new List<ControlDetailForPage>();
            //添加page
            ControlDetailForPage objPage = new ControlDetailForPage();
            objPage.ctrl_id = 0;
            objPage.ctrl_level = 1;
            objPage.ctrl_type = "page";
            objPage.d1 = 100;
            objPage.d2 = 500;
            objPage.d3 = 100;
            objPage.d4 = 200;
            objPage.d6 = "12";


            //添加navigationBar
            ControlDetailForPage objNavigationBar = new ControlDetailForPage();
            objNavigationBar.ctrl_id = 1;
            objNavigationBar.ctrl_level = 2;
            objNavigationBar.ctrl_type = "navigationBar";
            objNavigationBar.d0 = "[50, 51]";
            objNavigationBar.d1 = 320;
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
            objtext.d1 = 320;
            objtext.d2 = 44;
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
            dataForPageDetail dfp = new dataForPageDetail();
            dfp.page_id = pageId;
            dfp.page_name = pageName;
            dfp.group_id = groupId;

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
    }
}
