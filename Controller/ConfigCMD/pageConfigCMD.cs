using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using xinLongIDE.Model.requestJson;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class pageConfigCMD
    {
        private string addGroupConfigFilePath = System.Windows.Forms.Application.StartupPath + "\\GroupConfig\\addGroupConfigFile.xml";

        private string addPageConfigFilePath = System.Windows.Forms.Application.StartupPath + "\\GroupConfig\\addPageConfigFile.xml";

        private string addPageForNewGroup = System.Windows.Forms.Application.StartupPath + "\\GroupConfig\\addPageForNewGroupConfigFile.xml";

        private string LocalGroupInfo = System.Windows.Forms.Application.StartupPath + "\\GroupConfig\\localGroupInfo.xml";

        public string errCode = string.Empty;

        private List<groupCreateRequest> _lstForNewGroup = new List<groupCreateRequest>();
        private List<pageCreateRequest> _lstForNewPage = new List<pageCreateRequest>();
        private List<pageCreateRequest> _lstForNewPageWithoutGroupID = new List<pageCreateRequest>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public pageConfigCMD()
        {
            Init();
        }

        public void Init()
        {
            if (File.Exists(addGroupConfigFilePath))
            {
                _lstForNewGroup = xmlController.ReadFromXmlFile<List<groupCreateRequest>>(addGroupConfigFilePath);
            }
            if (File.Exists(addPageConfigFilePath))
            {
                _lstForNewPage = xmlController.ReadFromXmlFile<List<pageCreateRequest>>(addPageConfigFilePath);
            }
            if (File.Exists(addPageForNewGroup))
            {
                _lstForNewPageWithoutGroupID = xmlController.ReadFromXmlFile<List<pageCreateRequest>>(addPageForNewGroup);
            }
        }

        public void SetCacheToTreeView(System.Windows.Forms.TreeView tvw)
        {
            //新建的组
            if (!object.Equals(_lstForNewGroup, null))
            {
                foreach (groupCreateRequest obj in _lstForNewGroup)
                {
                    TreeNode tn = new TreeNode(obj.group_name);
                    tvw.Nodes.Add(tn);
                }
            }
            //新建的界面
            if (!Object.Equals(_lstForNewPage, null))
            {
                foreach (pageCreateRequest obj in _lstForNewPage)
                {
                    foreach (TreeNode ParentNode in tvw.Nodes)
                    {
                        if (!object.Equals(ParentNode.Tag, null))
                        {
                            if (obj.group_id.Equals(ParentNode.Tag.ToString()))
                            {
                                TreeNode child = new TreeNode(obj.page_name);
                                child.Tag = obj.group_id;
                                ParentNode.Nodes.Add(child);
                            }
                        }
                    }
                }
            }
            //新建组中的界面
            if (!object.Equals(_lstForNewPageWithoutGroupID, null))
            {
                foreach (pageCreateRequest obj in _lstForNewPageWithoutGroupID)
                {
                    foreach (TreeNode ParentNode in tvw.Nodes)
                    {
                        if (obj.group_id.Equals(ParentNode.Text))
                        {
                            TreeNode child = new TreeNode(obj.page_name);
                            child.Tag = obj.group_id;
                            ParentNode.Nodes.Add(child);
                        }
                    }
                }
            }
        }

        public void AddGroup(string groupName, string platForm)
        {
            groupCreateRequest obj = new groupCreateRequest(groupName, platForm);
            if (object.Equals(_lstForNewGroup, null))
            {
                _lstForNewGroup = new List<groupCreateRequest>();
            }
            _lstForNewGroup.Add(obj);
            //WriteToXmlFile<groupCreateRequest>(addGroupConfigFilePath, obj);
        }

        public void AddPage(string pageName, string groupId, string platForm, string groupName)
        {
            pageCreateRequest obj = new pageCreateRequest(groupId, platForm, pageName);
            if (!string.IsNullOrEmpty(groupId))
            {
                if (object.Equals(_lstForNewPage, null))
                {
                    _lstForNewPage = new List<pageCreateRequest>();
                }
                _lstForNewPage.Add(obj);
                //WriteToXmlFile<pageCreateRequest>(addPageConfigFilePath, obj);
            }
            else
            {
                obj.group_id = groupName;
                if (object.Equals(_lstForNewPageWithoutGroupID, null))
                {
                    _lstForNewPageWithoutGroupID = new List<pageCreateRequest>();
                }
                _lstForNewPageWithoutGroupID.Add(obj);
                //WriteToXmlFile<pageCreateRequest>(addPageForNewGroup, obj);
            }
        }

        /// <summary>
        /// 上传页面相关信息
        /// </summary>
        public Task<int> Upload()
        {
            return Task.Run(() =>
            {
                Dictionary<string, string> dic = UploadGroup();
                UploadPage(dic);
                this.Refresh();
                return 1;
            });

        }

        public void SavePage()
        {
            
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        private void DeleteCache()
        {
            if (File.Exists(addGroupConfigFilePath))
            {
                File.Delete(addGroupConfigFilePath);
            }
            if (File.Exists(addPageConfigFilePath))
            {
                File.Delete(addPageConfigFilePath);
            }
            if (File.Exists(addPageForNewGroup))
            {
                File.Delete(addPageForNewGroup);
            }
        }

        /// <summary>
        /// 通讯业务层
        /// </summary>
        private BaseController _bsController = new BaseController();

        /// <summary>
        /// 上传组数据
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> UploadGroup()
        {
            if (!File.Exists(addGroupConfigFilePath))
            {
                return null;
            }
            List<groupCreateRequest> lstGroupRequest = xmlController.ReadFromXmlFile<List<groupCreateRequest>>(addGroupConfigFilePath);
            if (lstGroupRequest.Count < 1)
            {
                return null;
            }
            Dictionary<string, string> dicOfGroupName_id = new Dictionary<string, string>();

            foreach (groupCreateRequest obj in lstGroupRequest)
            {
                CommonReturn cmresult = _bsController.CreateGroup(obj);
                string groupId = cmresult.data;
                dicOfGroupName_id.Add(obj.group_name, groupId);
            }
            File.Delete(addGroupConfigFilePath);
            return dicOfGroupName_id;
        }

        /// <summary>
        /// 上传页面包括有groupid
        /// </summary>
        /// <param name="dic"></param>
        private void UploadPage(Dictionary<string, string> dic)
        {
            if (File.Exists(addPageConfigFilePath))
            {
                List<pageCreateRequest> lstGroupRequest = xmlController.ReadFromXmlFile<List<pageCreateRequest>>(addPageConfigFilePath);

                foreach (pageCreateRequest obj in lstGroupRequest)
                {
                    CommonReturn returnResult = _bsController.CreatePage(obj);
                    DemoTest(obj.page_name, returnResult.data);
                }
                File.Delete(addPageConfigFilePath);
            }
            //这部分是用于上传此前未有组ID的页面
            if (object.Equals(dic, null))
            {
                return;
            }
            if (File.Exists(addPageForNewGroup))
            {
                List<pageCreateRequest> lstGroupRequestNewGroup = xmlController.ReadFromXmlFile<List<pageCreateRequest>>(addPageForNewGroup);
                foreach (pageCreateRequest obj in lstGroupRequestNewGroup)
                {
                    string groupId = dic[obj.group_id];
                    obj.group_id = groupId;
                    _bsController.CreatePage(obj);
                }
                File.Delete(addPageForNewGroup);
            }
        }

        private void DemoTest(string name, string id)
        {
            pageSaveRequest request = new pageSaveRequest(name, id);
            ControlDetailForRequest objPage = new ControlDetailForRequest();
            objPage.ctrl_id = "0";
            objPage.ctrl_level = "1";
            objPage.ctrl_type = "page";
            objPage.d0 = "[6, 7]";
            objPage.d1 = "100";
            objPage.d2 = "500";
            objPage.d3 = "100";
            objPage.d4 = "200";
            objPage.d6 = "12";

            ControlDetailForRequest objNavigationBar = new ControlDetailForRequest();
            objNavigationBar.ctrl_id = "1";
            objNavigationBar.ctrl_level = "2";
            objNavigationBar.ctrl_type = "navigationBar";
            objNavigationBar.d0 = "[50, 51]";
            objNavigationBar.d1 = "320";
            objNavigationBar.d2 = "44";
            objNavigationBar.d3 = "0";
            objNavigationBar.d4 = "0";
            objNavigationBar.d6 = "16";
            objNavigationBar.d8 = "#ffffff";
            objNavigationBar.d12 = "0";
            objNavigationBar.d14 = "#000000";
            objNavigationBar.d15 = "1";
            objNavigationBar.d18 = "1";
            objNavigationBar.d19 = "1";
            objNavigationBar.d26 = "0";
            objNavigationBar.d36 = "15";

            ControlDetailForRequest objtext = new ControlDetailForRequest();
            objtext.ctrl_id = "50";
            objtext.ctrl_level = "10";
            objtext.ctrl_type = "text";
            objtext.d0 = "测试页";
            objtext.d1 = "320";
            objtext.d2 = "44";
            objtext.d3 = "0";
            objtext.d4 = "0";
            objtext.d6 = "18";
            objtext.d7 = "#22798A";
            objtext.d8 = "#ffffff";
            objtext.d18 = "1";
            objtext.d19 = "1";
            objtext.d26 = "0";
            objtext.d36 = "15";

            ControlDetailForRequest objimg = new ControlDetailForRequest();
            objimg.ctrl_id = "51";
            objimg.ctrl_level = "2";
            objimg.ctrl_type = "img";
            objimg.d0 = "http://img.sootuu.com/vector/2007-07-01/068/3/200.gif";
            objimg.d1 = "44";
            objimg.d2 = "44";
            objimg.d3 = "0";
            objimg.d4 = "0";
            objimg.d5 = "1";
            objimg.d6 = "11";
            objimg.d7 = "#000000";
            objimg.d11 = "#ffffff";
            objimg.d18 = "1";
            objimg.d19 = "1";
            objimg.d26 = "0";
            objimg.d36 = "15";

            request.ctrls = new ControlDetailForRequest[] { objPage, objNavigationBar, objtext, objimg };

            CommonReturn objReturn = _bsController.SagePageInfo(request);
        }

        /// <summary>
        /// 缓存
        /// </summary>
        public Task<int> SaveCache()
        {
            return Task.Run(() =>
            {
                if (!object.Equals(_lstForNewGroup, null) && _lstForNewGroup.Count > 0)
                {
                    xmlController.WriteToXmlFile<List<groupCreateRequest>>(addGroupConfigFilePath, _lstForNewGroup);
                }
                if (!object.Equals(_lstForNewPage, null) && _lstForNewPage.Count > 0)
                {
                    xmlController.WriteToXmlFile<List<pageCreateRequest>>(addPageConfigFilePath, _lstForNewPage);
                }
                if (!object.Equals(_lstForNewPageWithoutGroupID, null) && _lstForNewPageWithoutGroupID.Count > 0)
                {
                    xmlController.WriteToXmlFile<List<pageCreateRequest>>(addPageForNewGroup, _lstForNewPageWithoutGroupID);
                }
                return 1;
            });

        }
        /// <summary>
        /// 异步获取页面信息
        /// </summary>
        /// <param name="platForm"></param>
        /// <returns></returns>
        public Task<pageGroupReturnData> GetPageGroupInfo(string platForm)
        {
            return Task.Run(() =>
            {
                if (File.Exists(LocalGroupInfo))
                {
                    return GetPageGroupInfoLocal(platForm);
                }
                else
                {
                    BaseController bc = new BaseController();
                    try
                    {
                        pageGroupReturnData obj = bc.GetPageGroupInfo(platForm);
                        xmlController.WriteToXmlFile<pageGroupReturnData>(LocalGroupInfo, obj);
                        return obj;
                    }
                    catch (Exception ex)
                    {
                        errCode = "获取页面信息出错:" + ex.Message;
                        return null;
                    }
                }

            });
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            if (File.Exists(LocalGroupInfo))
            {
                File.Delete(LocalGroupInfo);
            }
        }

        /// <summary>
        /// 获取本地页面配置信息
        /// </summary>
        /// <param name="platForm"></param>
        /// <returns></returns>
        private pageGroupReturnData GetPageGroupInfoLocal(string platForm)
        {
            try
            {
                pageGroupReturnData obj = xmlController.ReadFromXmlFile<pageGroupReturnData>(LocalGroupInfo);
                return obj;
            }
            catch (Exception ex)
            {
                errCode = "获取页面信息出错:" + ex.Message;
                return null;
            }
        }

    }
}
