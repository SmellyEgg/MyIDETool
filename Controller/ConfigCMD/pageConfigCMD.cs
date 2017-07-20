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

        public string errCode = string.Empty;

        private List<groupCreateRequest> _lstForNewGroup = new List<groupCreateRequest>();
        private List<pageCreateRequest> _lstForNewPage = new List<pageCreateRequest>();
        private List<pageCreateRequest> _lstForNewPageWithoutGroupID = new List<pageCreateRequest>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public pageConfigCMD()
        {
            if (File.Exists(addGroupConfigFilePath))
            {
                _lstForNewGroup = ReadFromXmlFile<List<groupCreateRequest>>(addGroupConfigFilePath);
            }
            if (File.Exists(addPageConfigFilePath))
            {
                _lstForNewPage = ReadFromXmlFile<List<pageCreateRequest>>(addPageConfigFilePath);
            }
            if (File.Exists(addPageForNewGroup))
            {
                _lstForNewPageWithoutGroupID = ReadFromXmlFile<List<pageCreateRequest>>(addPageForNewGroup);
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

        public void WriteToXmlFile<T>(string filePath, T objectToWrite) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, File.Exists(filePath) ? true : false);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        private T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
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
                DeleteCache();
                return 1;
            });
            
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
            List<groupCreateRequest> lstGroupRequest = ReadFromXmlFile<List<groupCreateRequest>>(addGroupConfigFilePath);
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
                List<pageCreateRequest> lstGroupRequest = ReadFromXmlFile<List<pageCreateRequest>>(addPageConfigFilePath);

                foreach (pageCreateRequest obj in lstGroupRequest)
                {
                    _bsController.CreatePage(obj);
                }
            }
            //这部分是用于上传此前未有组ID的页面
            if (object.Equals(dic, null))
            {
                return;
            }
            if (File.Exists(addPageForNewGroup))
            {
                List<pageCreateRequest> lstGroupRequestNewGroup = ReadFromXmlFile<List<pageCreateRequest>>(addPageForNewGroup);
                foreach (pageCreateRequest obj in lstGroupRequestNewGroup)
                {
                    string groupId = dic[obj.group_id];
                    obj.group_id = groupId;
                    _bsController.CreatePage(obj);
                }
            }
        }

        /// <summary>
        /// 缓存
        /// </summary>
        public Task<int> SaveCache()
        {
            return Task.Run(() =>
            {
                if (!object.Equals(_lstForNewGroup, null))
                {
                    WriteToXmlFile<List<groupCreateRequest>>(addGroupConfigFilePath, _lstForNewGroup);
                }
                if (!object.Equals(_lstForNewPage, null))
                {
                    WriteToXmlFile<List<pageCreateRequest>>(addPageConfigFilePath, _lstForNewPage);
                }
                if (!object.Equals(_lstForNewPageWithoutGroupID, null))
                {
                    WriteToXmlFile<List<pageCreateRequest>>(addPageForNewGroup, _lstForNewPageWithoutGroupID);
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
                BaseController bc = new BaseController();
                try
                {
                    pageGroupReturnData obj = bc.GetPageGroupInfo(platForm);
                    return obj;
                }
                catch (Exception ex)
                {
                    errCode = "获取页面信息出错:" + ex.Message;
                    return null;
                }

            });
        }



    }
}
